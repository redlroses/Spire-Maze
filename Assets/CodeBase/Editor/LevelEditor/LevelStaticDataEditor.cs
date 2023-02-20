using CodeBase.LevelSpecification;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticEditor : UnityEditor.Editor
    {
        private const string Munus = "-";
        private const string Plus = "+";
        private const string AddFloor = "Add floor";

        private LevelStaticData _target;
        private SerializedProperty _map;
        private SerializedProperty _levelKey;
        private SerializedProperty _width;
        private SerializedProperty _height;

        private GUIStyle _cellStyle;

        private void OnEnable()
        {
            _map = serializedObject.FindProperty("CellMap");
            _levelKey = serializedObject.FindProperty("LevelKey");
            _width = serializedObject.FindProperty("Width");
            _height = serializedObject.FindProperty("Height");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _cellStyle = EditorStyles.miniLabel;
            _cellStyle.fontSize = 11;
            LevelStaticData data = (LevelStaticData) target;

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

            DrawAddFloorButton(fromIndex, length);
            DrawDeleteFloorButton(fromIndex, length);

            EditorGUILayout.EndVertical();
        }

        private void DrawDeleteFloorButton(int fromIndex, int length)
        {
            if (GUILayout.Button(Munus))
            {
                for (int i = 0; i < length; i++)
                {
                    _map.DeleteArrayElementAtIndex(fromIndex);
                }

                _height.intValue--;
            }
        }

        private void DrawAddFloorButton(int fromIndex, int length)
        {
            if (GUILayout.Button(Plus))
            {
                AddNewFloor(fromIndex + length, length);
            }
        }

        private void AddNewFloor(int fromIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _map.InsertArrayElementAtIndex(fromIndex + i);
                _map.GetArrayElementAtIndex(fromIndex + i).enumValueFlag = (int) CellType.Air;
            }

            _height.intValue++;
        }

        private void DrawCell(int index)
        {
            SerializedProperty arrayElementAtIndex = _map.GetArrayElementAtIndex(index);

            if (arrayElementAtIndex == null)
            {
                _map.InsertArrayElementAtIndex(index);
                arrayElementAtIndex.enumValueFlag = (int) CellType.Air;
            }

            SetColor((CellType) arrayElementAtIndex.enumValueFlag);

            CellType cellType = (CellType) EditorGUILayout.EnumFlagsField(GUIContent.none,
                (CellType) arrayElementAtIndex.enumValueFlag, _cellStyle, GUILayout.MinWidth(5f));
            arrayElementAtIndex.enumValueFlag = (int) cellType;
        }

        private void SetColor(CellType by)
        {
            _cellStyle.normal.textColor = by switch
            {
                CellType.Air => Color.cyan,
                CellType.Plate => Color.green,
                CellType.Wall => Color.red,
                CellType.Door => Color.yellow,
                CellType.Key => new Color32(114, 173, 114, 255),
                CellType.Left => Color.grey,
                CellType.Up => Color.grey,
                CellType.Right => Color.grey,
                CellType.Down => Color.grey,
                CellType.MovingPlate | CellType.Left => Color.blue,
                CellType.MovingPlate | CellType.Up => Color.blue,
                CellType.MovingPlate | CellType.Right => Color.blue,
                CellType.MovingPlate | CellType.Down => Color.blue,
                _ => Color.black
            };
        }
    }
}