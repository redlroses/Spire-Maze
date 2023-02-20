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
                    // var newKey = new Key(_editor.GetTextureByType(key, key.Color), key.Color);
                    key.SetTexture(_editor.GetTextureByType(key, key.Color));
                    key.Color = key.Color;
                    break;
                }
                case Door door:
                {
                    var newDoor = new Door(_editor.GetTextureByType(door, door.Color), door.Color);
                    door.SetTexture(newDoor.Texture);
                    break;
                }
            }
        }
    }
}