using UnityEngine;

namespace CodeBase.Logic
{
    public class ColorEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleAura;
        [SerializeField] private ParticleSystem _particleStar;

        public void SetColor(Color color)
        {
            ParticleSystem.MainModule particleMainModule = _particleAura.main;
            ParticleSystem.MinMaxGradient colorGradient = particleMainModule.startColor;
            colorGradient.color = color;
            particleMainModule.startColor = colorGradient;

            particleMainModule = _particleStar.main;
            particleMainModule.startColor = colorGradient;
        }
    }
}