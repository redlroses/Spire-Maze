using CodeBase.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Logic
{
    public class CrazyProfileImageSetter : MonoBehaviour
    {
        [SerializeField] private Image _userIcon;

        private ImageLoader _imageLoader;

#if !UNITY_EDITOR && CRAZY_GAMES
        private void Awake()
        {
            CrazyGames.CrazyUser.Instance.GetUser(
                user =>
                {
                    if (user != null)
                        _imageLoader.LoadImage(user.profilePictureUrl, sprite => _userIcon.sprite = sprite);
                });
        }
#endif
    }
}