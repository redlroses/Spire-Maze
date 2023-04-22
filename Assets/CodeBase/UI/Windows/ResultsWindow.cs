using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.UI.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private WindowAnimationPlayer _windowAnimationPlayer;

        protected override void Initialize()
        {
            _windowAnimationPlayer.Play();
        }
    }
}