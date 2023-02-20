using CodeBase.Data.Cell;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor.LevelEditor
{
    public class CellSettings : PopupWindowContent
    {
        private SerializedProperty _cellData;
        private Colors _color;

        public CellSettings(ref SerializedProperty cellData)
        {
            _cellData = cellData;
        }

        public override Vector2 GetWindowSize() =>
            new Vector2(200, 100);

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Cell Settings", EditorStyles.boldLabel);

            switch (_cellData.managedReferenceValue)
            {
                case Key key:
                    key.Color = (Colors) EditorGUILayout.EnumPopup("Color", key.Color);
                    break;
                case Door door:
                    door.Color = (Colors) EditorGUILayout.EnumPopup("Color", door.Color);
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
                case Key key:
                {
                    var newKey = new Key(key.Color);
                    key.SetTexture(newKey.Texture);
                    break;
                }
                case Door door:
                {
                    var newDoor = new Door(door.Color);
                    door.SetTexture(newDoor.Texture);
                    break;
                }
            }
        }
    }
}