using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Sound
{
    public class FinishPortalClipSource : AudioClipSource
    {
        [SerializeField] private FinishPortal _finishPortal;
        [SerializeField] private AudioClip _clip;

        private void OnValidate()
        {
            _finishPortal ??= GetComponent<FinishPortal>();
        }

        private void OnEnable()
        {
            _finishPortal.Teleported += OnTeleported;
        }

        private void OnDisable()
        {
            _finishPortal.Teleported -= OnTeleported;
        }

        private void OnTeleported()
        {
            PlayOneShot(_clip);
        }
    }
}