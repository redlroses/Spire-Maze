using CodeBase.Data.Cell;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.LevelEditor
{
    public class CellSettings : PopupWindowContent
    {
        private readonly SerializedProperty _cellData;
        private readonly LevelStaticDataEditor _editor;

        private Colors _color;

        public CellSettings(ref SerializedProperty cellData, LevelStaticDataEditor editor)
        {
            _cellData = cellData;
            _editor = editor;
        }

        public override Vector2 GetWindowSize() =>
            new Vector2(200, 100);

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Cell Settings", EditorStyles.boldLabel);

            switch (_cellData.managedReferenceValue)
            {
                case ColoredCell cell:
                    cell.Color = (Colors) EditorGUILayout.EnumPopup("Color", cell.Color);
                    break;
                case MovingMarker plate:
                    plate.Direction = (PlateMoveDirection) EditorGUILayout.EnumPopup("Direction", plate.Direction);
                    plate.IsLiftHolder = EditorGUILayout.Toggle("Is lift holder", plate.IsLiftHolder);
                    break;
            }
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
            switch (_cellData.managedReferenceValue)
            {
                case ColoredCell cell:
                {
                    cell.SetTexture(_editor.GetTextureByType(cell));
                    break;
                }
                case MovingMarker cell:
                {
                    cell.SetTexture(_editor.GetTextureByType(cell));
                    break;
                }
            }
        }
    }
}