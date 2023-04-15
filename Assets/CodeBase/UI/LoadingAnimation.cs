using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class LoadingAnimation : MonoBehaviour
    {
        private const string RotateAnimation = "Rotate";

        [SerializeField] private Image _rotor;
        [SerializeField] private LocationAnimations _rotorLocationAnimations;

        public void Play()
        {
            _rotor.enabled = true;
            _rotorLocationAnimations.StartAnimation(RotateAnimation);
        }

        public void Stop()
        {
            _rotorLocationAnimations.StopCurrentAnimation();
            _rotor.enabled = false;
        }
    }
}