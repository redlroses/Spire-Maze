using NaughtyAttributes;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.Logic
{
    public class WindowAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private LocationAnimations[] _locationAnimations;
        [SerializeField] private ParticleSystem[] _particles;

        [Button]
        public void Play()
        {
            foreach (var animation in _locationAnimations)
            {
                animation.StartAnimation("Show");
            }

            foreach (var particle in _particles)
            {
                particle.Play();
            }
        }
    }
}