﻿using System;
using System.Collections.Generic;
using CodeBase.EditorCells;
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
        private Dictionary<Type, Func<CellData, Texture2D>> _textures;
        private Dictionary<Type, Color32> _colors;

        private bool _isPaletteShow;

        private void OnEnable()
        {
            _dataMap = serializedObject.FindProperty("CellDataMap");
            _width = serializedObject.FindProperty("Width");
            _height = serializedObject.FindProperty("Height");

            _textures = new Dictionary<Type, Func<CellData, Texture2D>>
            {
                [typeof(Air)] = data => _baseTextures[typeof(Air)].Tint(_colors[typeof(Air)]),
                [typeof(Plate)] = data => _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)]),
                [typeof(InitialPlate)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(InitialPlate)].Tint(_colors[typeof(InitialPlate)])),
                [typeof(Wall)] = data => _baseTextures[typeof(Wall)].Tint(_colors[typeof(Wall)]),
                [typeof(Door)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Door)]
                            .Tint(GetColor32(((Door) data)?.Color ?? Colors.None))),
                [typeof(Key)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Key)]
                            .Tint(GetColor32(((Key) data)?.Color ?? Colors.None))),
                [typeof(MovingMarker)] = data =>
                    _baseTextures[typeof(MovingMarker)].Tint(_colors[typeof(MovingMarker)])
                        .RotateTo(((MovingMarker) data)?.Direction ?? PlateMoveDirection.None)
                        .CombineTexture(((MovingMarker) data)?.IsLiftHolder == true
                            ? _baseTextures[typeof(MovingPlate)].Tint(_colors[typeof(MovingPlate)])
                            : _baseTextures[typeof(Air)]),
                [typeof(Portal)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Portal)]
                            .Tint(((Portal) data)?.Color ?? _colors[typeof(Portal)])),
                [typeof(FinishPortal)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Plate)].Tint(_colors[typeof(FinishPortal)])),
                [typeof(SpikeTrap)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(SpikeTrap)].Tint(_colors[typeof(SpikeTrap)])),
                [typeof(FireTrap)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(FireTrap)].Tint(_colors[typeof(FireTrap)])),
                [typeof(Rock)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Rock)].Tint(_colors[typeof(Rock)])),
                [typeof(Savepoint)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(Savepoint)].Tint(_colors[typeof(Savepoint)])),
                [typeof(EnemySpawnPoint)] = data =>
                    _baseTextures[typeof(Plate)].Tint(_colors[typeof(Plate)])
                        .CombineTexture(_baseTextures[typeof(EnemySpawnPoint)].Tint(_colors[typeof(EnemySpawnPoint)])),
            };

            _baseTextures = new Dictionary<Type, Texture2D>
            {
                [typeof(Air)] = Resources.Load<Texture2D>("Textures/AirIcon"),
                [typeof(Plate)] = Resources.Load<Texture2D>("Textures/PlateIcon"),
                [typeof(InitialPlate)] = Resources.Load<Texture2D>("Textures/InitialPlateIcon"),
                [typeof(Wall)] = Resources.Load<Texture2D>("Textures/WallIcon"),
                [typeof(Door)] = Resources.Load<Texture2D>("Textures/DoorIcon"),
                [typeof(Key)] = Resources.Load<Texture2D>("Textures/KeyIcon"),
                [typeof(MovingMarker)] = Resources.Load<Texture2D>("Textures/MovingMarkerIcon"),
                [typeof(MovingPlate)] = Resources.Load<Texture2D>("Textures/MovingPlateIcon"),
                [typeof(Portal)] = Resources.Load<Texture2D>("Textures/PortalIcon"),
                [typeof(FinishPortal)] = Resources.Load<Texture2D>("Textures/FinishPortalIcon"),
                [typeof(SpikeTrap)] = Resources.Load<Texture2D>("Textures/SpikeTrapIcon"),
                [typeof(FireTrap)] = Resources.Load<Texture2D>("Textures/FireTrapIcon"),
                [typeof(Rock)] = Resources.Load<Texture2D>("Textures/RockIcon"),
                [typeof(Savepoint)] = Resources.Load<Texture2D>("Textures/SaveIcon"),
                [typeof(EnemySpawnPoint)] = Resources.Load<Texture2D>("Textures/EnemyIcon"),
            };

            _colors = new Dictionary<Type, Color32>
            {
                [typeof(Air)] = new Color32(0, 0, 0, 0),
                [typeof(Plate)] = new Color32(57, 181, 94, 255),
                [typeof(InitialPlate)] = new Color32(192, 105, 55, 255),
                [typeof(Wall)] = new Color32(57, 181, 94, 255),
                [typeof(MovingMarker)] = new Color32(77, 181, 177, 255),
                [typeof(MovingPlate)] = new Color32(199, 195, 74, 255),
                [typeof(Portal)] = new Color32(129, 93, 199, 255),
                [typeof(FinishPortal)] = new Color32(0, 0, 0, 255),
                [typeof(SpikeTrap)] = new Color32(76, 128, 144, 255),
                [typeof(FireTrap)] = new Color32(255, 170, 52, 255),
                [typeof(Rock)] = new Color32(233, 117, 50, 255),
                [typeof(Savepoint)] = new Color32(0, 180, 255, 255),
                [typeof(EnemySpawnPoint)] = new Color32(255, 255, 255, 255),
            };

            _palette = new CellData[]
            {
                new Air(GetTextureByType<Air>()),
                new Plate(GetTextureByType<Plate>()),
                new InitialPlate(GetTextureByType<InitialPlate>()),
                new Wall(GetTextureByType<Wall>()),
                new Key(GetTextureByType<Key>()),
                new Door(GetTextureByType<Door>()),
                new MovingMarker(GetTextureByType<MovingMarker>()),
                new Portal(GetTextureByType<Portal>()) {Color = _colors[typeof(Portal)]},
                new FinishPortal(GetTextureByType<Portal>()),
                new SpikeTrap(GetTextureByType<SpikeTrap>()),
                new FireTrap(GetTextureByType<FireTrap>()),
                new Rock(GetTextureByType<Rock>()),
                new Savepoint(GetTextureByType<Savepoint>()),
                new EnemySpawnPoint(GetTextureByType<EnemySpawnPoint>()),
            };

            UpdateTextures();
        }

        private void UpdateTextures()
        {
            for (int i = 0; i < _width.intValue * _height.intValue; i++)
            {
                CellData managedReferenceValue = (CellData) _dataMap.GetArrayElementAtIndex(i).managedReferenceValue;
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

            _isPaletteShow = EditorGUILayout.Toggle("Edit mode on", _isPaletteShow);

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

        public Texture2D GetTextureByType(CellData cellData)
        {
            Texture2D texture2D = _textures[cellData.GetType()].Invoke(cellData);
            texture2D.Apply();
            return texture2D;
        }

        private Texture2D GetTextureByType<TCell>()
        {
            Texture2D texture2D = _textures[typeof(TCell)].Invoke(null);
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
                    arrayElementAtIndex.managedReferenceValue = _pipetteCell.Copy();
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