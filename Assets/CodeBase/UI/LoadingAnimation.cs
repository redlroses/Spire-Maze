using UnityEngine;

namespace CodeBase.UI
{
    public class LoadingAnimation : MonoBehaviour
    {
        private const string RotateAnimation = "Rotate";

        [SerializeField] private GameObject _rotor;
        
        public void Play()
        {
            _rotor.SetActive(true);
        }

        public void Stop()
        {
            _rotor.SetActive(false);
        }
    }
}