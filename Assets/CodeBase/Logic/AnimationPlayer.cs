using NTC.Global.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeBase.Logic
{
    public class AnimationPlayer : MonoBehaviour
    {
        [FormerlySerializedAs("_rotor")] [SerializeField]
        private Animation _animation;

        public void Play()
        {
            _animation.gameObject.Enable();
            _animation.Play();
        }

        public void Stop()
        {
            _animation.Stop();
            _animation.gameObject.Disable();
        }
    }
}