using NaughtyAttributes;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Logic
{
    public class WindowAnimationPlayer : MonoBehaviour
    {
        private const string ShowAnimationName = "Show";

        [SerializeField] private LocationAnimations[] _locationAnimations;
        [SerializeField] private ParticleSystem[] _particles;

        [Button]
        public void Play()
        {
            foreach (LocationAnimations animation in _locationAnimations)
            {
                animation.StartAnimation(ShowAnimationName);
            }

            foreach (ParticleSystem particle in _particles)
            {
                particle.Play();
            }
        }
    }
}