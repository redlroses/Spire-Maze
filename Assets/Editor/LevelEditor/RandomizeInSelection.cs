using System.Collections.Generic;
using System.Linq;
using CodeBase.Editor.LevelEditor;
using CodeBase.EditorCells;
using TMPro;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

// Simple script that randomizes the rotation of the selected GameObjects.
// It also lists which objects are currently selected.

public class EditorWindowWithPopup : EditorWindow
{
    private CellData[] _cellsData;

    private const string TexturesPlateIcon = "Textures/PlateIcon";
    private const string TexturesKeyIcon = "Textures/KeyIcon";
    private const string TexturesDoorIcon = "Textures/DoorIcon";

    private readonly Color32 _plateColor = new Color32(97, 204, 111, 255);
    private readonly Color32 _keyColor = new Color32(204, 66, 67, 255);
    private readonly Color32 _doorColor = new Color32(117, 81, 204, 255);

    private CellData _plate;
    private CellData _key;
    private CellData _door;

    private Rect _buttonRect;
    private GUIStyle _cellStyle;
    private Texture2D _temp;

    // SetUp menu item

    [MenuItem("Example/Popup Example")]
    static void Init()
    {
        EditorWindow window = CreateInstance<EditorWindowWithPopup>();
        window.Show();
    }

    private void OnEnable()
    {
        // _plate = new Plate(Resources.Load<Texture2D>(TexturesPlateIcon), _plateColor);
        // _key = new Key(Resources.Load<Texture2D>(TexturesKeyIcon), _keyColor, Colors.Blue);
        // _door = new Door(Resources.Load<Texture2D>(TexturesDoorIcon), _doorColor, Colors.Green);

        _cellsData = new[] {_plate, _key, _door};

        Debug.Log(_cellsData[0] is Plate);
        Debug.Log(_cellsData[1] is Key);
        Debug.Log(_cellsData[2] is Key);
        Debug.Log(_cellsData[2] is Plate);

        _temp = CombineTextures(_plate, _key);
    }
    private static Texture2D CombineTextures(CellData first, CellData second)
    {
        NativeArray<Color32> firstData = first.Texture.GetRawTextureData<Color32>();
        NativeArray<Color32> secondData = second.Texture.GetRawTextureData<Color32>();
        Color32[] newData = new Color32[firstData.Count()];

        for (int i = 0; i < firstData.Length; i++)
        {
            if (firstData[i].a != 0)
            {
                newData[i] = firstData[i];
            }
            else if (secondData[i].a != 0)
            {
                newData[i] = secondData[i];
            }
            else
            {
                newData[i] = new Color32(0,0,0,0);
            }
        }

        NativeArray<Color32> newColorData = new NativeArray<Color32>(newData, Allocator.Temp);
        Texture2D newTexture = new Texture2D(first.Texture.width, first.Texture.height, first.Texture.format, false);
        newTexture.SetPixelData(newColorData, 0);
        newTexture.Apply();
        return newTexture;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(EditorStyles.iconButton) {fixedHeight = 50, fixedWidth = 50};

        for (int j = 0; j < 5; j++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < 16; i++)
            {
                DrawButton(_temp, style);
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawButton(Texture icon, GUIStyle style)
    {
        if (GUILayout.Button(new GUIContent(icon), style))
        {
            // PopupWindow.Show(_buttonRect, new PopupExample());
        }

        if (Event.current.type == EventType.Repaint)
        {
            Vector2 mousePos = Event.current.mousePosition;
            _buttonRect = new Rect(mousePos, Vector2.zero);
        }
    }
}