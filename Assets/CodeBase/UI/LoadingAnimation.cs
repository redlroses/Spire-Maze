using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI
{
    public class LoadingAnimation : MonoBehaviour
    {
        private const string RotateAnimation = "Rotate";

        [SerializeField] private GameObject _rotor;
        [SerializeField] private LocationAnimations _rotorLocationAnimations;

        public void Play()
        {
            _rotor.SetActive(true);
            _rotorLocationAnimations.StartAnimation(RotateAnimation);
        }

        public void Stop()
        {
            _rotorLocationAnimations.StopCurrentAnimation();
            _rotor.SetActive(false);
        }
    }
}