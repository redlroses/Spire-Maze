using CodeBase.Data.Cell;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.LevelEditor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string Munus = "-";
        private const string Plus = "+";
        private const string AddFloor = "Add floor";

        private CellData[] _cellsData;
        private Rect _buttonRect;

        private LevelStaticData _target;
        private SerializedProperty _dataMap;
        private SerializedProperty _width;
        private SerializedProperty _height;

        private GUIStyle _cellStyle;
        private CellData _pipetteCell;
        private CellData[] _palette;

        private bool _isPaletteShow;

        private void OnEnable()
        {
            _dataMap = serializedObject.FindProperty("CellDataMap");
            _width = serializedObject.FindProperty("Width");
            _height = serializedObject.FindProperty("Height");

            _palette = new CellData[] {new Air(), new Plate(), new Wall(), new Key(), new Door()};
            _cellStyle = new GUIStyle(EditorStyles.iconButton)
            {
                stretchHeight = false, stretchWidth = false, fixedHeight = 0, fixedWidth = 0,
                alignment = TextAnchor.MiddleCenter
            };
        }

        public override void OnInspectorGUI()
        {
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
                _cellStyle, GUILayout.MinWidth(10), GUILayout.MinHeight(10), GUILayout.MaxHeight(Screen.width / (float) _width.intValue)))
            {
                _pipetteCell = cellData;
            }
        }

        private void DrawCell(int index)
        {
            SerializedProperty arrayElementAtIndex = _dataMap.GetArrayElementAtIndex(index);

            if (GUILayout.Button(new GUIContent(((CellData) arrayElementAtIndex.managedReferenceValue).Texture),
                _cellStyle, GUILayout.MinWidth(10), GUILayout.MinHeight(10), GUILayout.MaxHeight(Screen.width / (_width.intValue + 1f))))
            {
                if (_isPaletteShow)
                {
                    arrayElementAtIndex.managedReferenceValue = (CellData.Copy(_pipetteCell));
                }
                else
                {
                    PopupWindow.Show(_buttonRect, new CellSettings(ref arrayElementAtIndex));
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
            if (GUILayout.Button(Munus, GUILayout.ExpandHeight(true), GUILayout.MinHeight(5)))
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
                _dataMap.GetArrayElementAtIndex(fromIndex + i).managedReferenceValue = new Plate();
            }

            _height.intValue++;
        }
    }
}