using CodeBase.EditorCells;
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
            new Vector2(150, 100);

        public override void OnGUI(Rect rect)
        {
            EditorGUIUtility.labelWidth = 90;
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
                case Portal cell:
                    cell.Key = EditorGUILayout.IntField("Link Id", cell.Key);
                    cell.Color = EditorGUILayout.ColorField("Color", cell.Color);
                    break;
                case Rock rock:
                    rock.IsDirectionToRight = EditorGUILayout.Toggle("Is move to right", rock.IsDirectionToRight);
                    break;
                case EnemySpawnPoint enemy:
                    enemy.Type = (EnemyType)EditorGUILayout.EnumPopup("Type of enemy", enemy.Type);
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
                    cell.SetTexture(_editor.GetTextureByType(cell));
                    break;
                case MovingMarker cell:
                    cell.SetTexture(_editor.GetTextureByType(cell));
                    break;
                case Portal cell:
                    cell.SetTexture(_editor.GetTextureByType(cell));
                    break;
            }
        }
    }
}