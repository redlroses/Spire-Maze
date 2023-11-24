using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic.Portal
{
    [RequireComponent(typeof(PortalGateEffector))]
    public class PortalGateEffector : MonoBehaviour
    {
        [SerializeField] private PortalGate _portal;
        [SerializeField] private ParticleSystem _teleportedEffect;
        [SerializeField] private ParticleSystem _regularEffect;

        private void OnValidate()
        {
            _portal ??= GetComponent<PortalGate>();
        }

        public void Construct(Color32 regularEffectColor)
        {
            _portal.Teleported += OnTeleported;
            _regularEffect.Colorize(regularEffectColor);
        }

        private void OnTeleported()
        {
            _teleportedEffect.Play();
        }
    }
}