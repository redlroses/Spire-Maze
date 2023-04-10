using System;
using UnityEngine;

namespace CodeBase.UI
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform _rotor;
        [SerializeField] private float _duration;

        public void Play()
        {
            throw new NotImplementedException();
            _rotor.gameObject.SetActive(true);
        }

        public void Stop()
        {
            throw new NotImplementedException();
            _rotor.gameObject.SetActive(false);
        }
    }
}