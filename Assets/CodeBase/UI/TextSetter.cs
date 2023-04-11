using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class TextSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            _text.text = text;
        }
        
        public void SetText(int text)
        {
            _text.text = text.ToString();
        }

        public void SetTextColor(Color32 color)
        {
            _text.color = color;
        }
    }
}