using System;
using System.Collections.Generic;
using CodeBase.Data.Cell;
using CodeBase.StaticData;
using CodeBase.Tools.Extension;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.LevelEditor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string Minus = "-";
        private const string Plus = "+";
        private const string AddFloor = "Add floor";

        private Rect _buttonRect;

        private LevelStaticData _target;
        private SerializedProperty _dataMap;
        private SerializedProperty _width;
        private SerializedProperty _height;

        private GUIStyle _cellStyle;

        private CellData _pipetteCell;
        private CellData[] _palette;

        private Dictionary<Type, Texture2D> _baseTextures;
        private Dictionary<Type, Func<Colors, Texture2D>> _textures;
        private Dictionary<Type, Color32> _colors;

        private bool _isPaletteShow;

        private void OnEnable()
        {
            _dataMap = serializedObject.FindProperty("CellDataMap");
            _width = serializedObject.FindProperty("Width");
            _height = serializedObject.FindProperty("Height");

            _textures = new Dictionary<Type, Func<Colors, Texture2D>>
            {
                [typeof(Air)] = color => _baseTextures[typeof(Air)].Tint(_colors[typeof(Air)]),
                [typeof(Plate)] = color => _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)]),
                [typeof(Wall)] = color => _baseTextures[typeof(Wall)].Tint(_colors[typeof(Wall)]),
                [typeof(Door)] = color =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Door)].Tint(GetColor32(color))),
                [typeof(Key)] = color =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Key)].Tint(GetColor32(color))),
            };

            _baseTextures = new Dictionary<Type, Texture2D>
            {
                [typeof(Air)] = Resources.Load<Texture2D>("Textures/AirIcon"),
                [typeof(Plate)] = Resources.Load<Texture2D>("Textures/PlateIcon"),
                [typeof(Wall)] = Resources.Load<Texture2D>("Textures/WallIcon"),
                [typeof(Door)] = Resources.Load<Texture2D>("Textures/DoorIcon"),
                [typeof(Key)] = Resources.Load<Texture2D>("Textures/KeyIcon"),
            };

            _colors = new Dictionary<Type, Color32>
            {
                [typeof(Air)] = new Color32(0, 0, 0, 0),
                [typeof(Plate)] = new Color32(57, 181, 94, 255),
                [typeof(Wall)] = new Color32(57, 181, 94, 255),
            };

            _palette = new CellData[]
            {
                new Air(GetTextureByType<Air>()),
                new Plate(GetTextureByType<Plate>()),
                new Wall(GetTextureByType<Wall>()),
                new Key(GetTextureByType<Key>()),
                new Door(GetTextureByType<Door>())
            };

            UpdateTextures();
        }

        private void UpdateTextures()
        {
            for (int i = 0; i < _width.intValue * _height.intValue; i++)
            {
                CellData managedReferenceValue = _dataMap.GetArrayElementAtIndex(i).managedReferenceValue as CellData;

                if (managedReferenceValue is Key key)
                {
                    managedReferenceValue.SetTexture(GetTextureByType(managedReferenceValue, key.Color));
                    continue;
                }

                if (managedReferenceValue is Door door)
                {
                    managedReferenceValue.SetTexture(GetTextureByType(managedReferenceValue, door.Color));
                    continue;
                }

                managedReferenceValue.SetTexture(GetTextureByType(managedReferenceValue));
            }
        }

        public override void OnInspectorGUI()
        {
            _cellStyle = new GUIStyle(EditorStyles.iconButton)
            {
                stretchHeight = false, stretchWidth = false, fixedHeight = 0, fixedWidth = 0,
                alignment = TextAnchor.MiddleCenter
            };

            base.OnInspectorGUI();
            LevelStaticData data = (LevelStaticData) target;

            _isPaletteShow = EditorGUILayout.Toggle("Edit palette mode", _isPaletteShow);

            if (_isPaletteShow)
            {
                DrawPalette();
            }

            if (_height.intValue <= 0)
            {
                if (GUILayout.Button(AddFloor))
                {
                    AddNewFloor(0, data.Width);
                }
            }

            DrawMatrix(data.Width, data.Height);
            serializedObject.ApplyModifiedProperties();
        }

        public Texture2D GetTextureByType(CellData cellData, Colors color = Colors.None)
        {
            Texture2D texture2D = _textures[cellData.GetType()].Invoke(color);
            texture2D.Apply();
            return texture2D;
        }

        public Texture2D GetTextureByType<TCell>(Colors color = Colors.None)
        {
            Texture2D texture2D = _textures[typeof(TCell)].Invoke(color);
            texture2D.Apply();
            return texture2D;
        }

        private void DrawPalette()
        {
            EditorGUILayout.BeginHorizontal();

            foreach (var paletteCell in _palette)
            {
                DrawPaletteButton(paletteCell);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawPaletteButton(CellData cellData)
        {
            if (GUILayout.Button(new GUIContent(cellData.Texture),
                _cellStyle, GUILayout.MinWidth(10), GUILayout.MinHeight(10),
                GUILayout.MaxHeight(Screen.width / (float) _width.intValue)))
            {
                _pipetteCell = cellData;
            }
        }

        private void DrawCell(int index)
        {
            SerializedProperty arrayElementAtIndex = _dataMap.GetArrayElementAtIndex(index);

            if (GUILayout.Button(new GUIContent(((CellData) arrayElementAtIndex.managedReferenceValue).Texture),
                _cellStyle, GUILayout.MinWidth(10), GUILayout.MinHeight(10),
                GUILayout.MaxHeight(Screen.width / (_width.intValue + 1f))))
            {
                if (_isPaletteShow)
                {
                    arrayElementAtIndex.managedReferenceValue = CellData.Copy(_pipetteCell);
                }
                else
                {
                    PopupWindow.Show(_buttonRect, new CellSettings(ref arrayElementAtIndex, this));
                }
            }

            if (Event.current.type == EventType.Repaint)
            {
                DrawPopup();
            }
        }

        private void DrawPopup()
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();

            if (buttonRect.Contains(Event.current.mousePosition))
            {
                _buttonRect = buttonRect;
            }
        }

        private void DrawMatrix(int width, int height)
        {
            EditorGUILayout.BeginVertical();

            for (int i = height - 1; i >= 0; i--)
            {
                DrawLine(width * i, width);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawLine(int fromIndex, int length)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));

            for (int i = 0; i < length; i++)
            {
                DrawCell(fromIndex + i);
            }

            if (Screen.width > 700)
            {
                EditorGUILayout.BeginVertical();

                DrawAddFloorButton(fromIndex, length);
                DrawDeleteFloorButton(fromIndex, length);

                EditorGUILayout.EndVertical();
            }
            else
            {
                DrawAddFloorButton(fromIndex, length);
                DrawDeleteFloorButton(fromIndex, length);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawDeleteFloorButton(int fromIndex, int length)
        {
            if (GUILayout.Button(Minus, GUILayout.ExpandHeight(true), GUILayout.MinHeight(5)))
            {
                DeleteFloor(fromIndex, length);
            }
        }

        private void DeleteFloor(int fromIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _dataMap.DeleteArrayElementAtIndex(fromIndex);
            }

            _height.intValue--;
        }

        private void DrawAddFloorButton(int fromIndex, int length)
        {
            if (GUILayout.Button(Plus, GUILayout.ExpandHeight(true), GUILayout.MinHeight(5)))
            {
                AddNewFloor(fromIndex + length, length);
            }
        }

        private void AddNewFloor(int fromIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _dataMap.InsertArrayElementAtIndex(fromIndex + i);
                _dataMap.GetArrayElementAtIndex(fromIndex + i).managedReferenceValue =
                    new Plate(GetTextureByType<Plate>());
            }

            _height.intValue++;
        }

        private Color32 GetColor32(Colors from)
        {
            return from switch
            {
                Colors.None => new Color32(255, 255, 255, 255),
                Colors.Green => new Color32(54, 214, 69, 255),
                Colors.Blue => new Color32(91, 113, 214, 255),
                Colors.Red => new Color32(204, 41, 50, 255),
                _ => throw new ArgumentOutOfRangeException(nameof(from), from, null)
            };
        }
    }
}