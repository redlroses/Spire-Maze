using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase
{
    public class TestWindowWrapper : MonoBehaviour
    {
        [SerializeField] private LocationAnimations[] _locationAnimations;

        public void Play()
        {
            foreach (var animation in _locationAnimations)
            {
                animation.StartAnimation("Show");
            }
        }
    }
}