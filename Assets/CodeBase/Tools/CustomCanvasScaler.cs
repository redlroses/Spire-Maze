using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Tools
{
    [RequireComponent(typeof(CanvasScaler))]
    public class CustomCanvasScaler : MonoBehaviour
    {
        [SerializeField] private CanvasScaler _canvas;

        private float _screenWidth;

        private void Update()
        {
            float currentScreenWidth = Screen.width;

            if (Mathf.Approximately(_screenWidth, currentScreenWidth))
            {
                return;
            }

            _screenWidth = currentScreenWidth;
            _canvas.scaleFactor = 0.00036420395421436004f * currentScreenWidth + 0.0007284079084287201f;
        }
    }
}
