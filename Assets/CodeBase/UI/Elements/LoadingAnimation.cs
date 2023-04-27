using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LoadingAnimation : MonoBehaviour
    {
        private const string RotateAnimation = "Rotate";

        [SerializeField] private Animation _rotor;
        
        public void Play()
        {
            _rotor.gameObject.SetActive(true);
            _rotor.Play();
        }

        public void Stop()
        {
            _rotor.Stop();
            _rotor.gameObject.SetActive(false);
        }
    }
}