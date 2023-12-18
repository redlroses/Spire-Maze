using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class BetterImageSetter : MonoBehaviour
    {
        [SerializeField] private BetterImage _betterImage;

        public void SetImage(Sprite image)
        {
            _betterImage.sprite = image;
        }
    }
}