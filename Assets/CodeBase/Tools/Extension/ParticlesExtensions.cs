using System.Linq;
using UnityEngine;

namespace CodeBase.Tools.Extension
{
    public static class ParticlesExtensions
    {
        public static void Colorize(this ParticleSystem effect, Color32 color)
        {
            ParticleSystem.MainModule particleMainModule = effect.main;
            ParticleSystem.MinMaxGradient mainColorGradient = particleMainModule.startColor;
            ParticleSystem.ColorOverLifetimeModule colorOverLifeTime = effect.colorOverLifetime;

            ParticleSystem.MinMaxGradient lifeTimeGradient = colorOverLifeTime.color.gradient;

            Gradient modifiedGradient = new Gradient
            {
                alphaKeys = lifeTimeGradient.gradient.alphaKeys,
            };

            foreach (GradientColorKey originalColorKey in lifeTimeGradient.gradient.colorKeys)
            {
                GradientColorKey newColorKey = new GradientColorKey(
                    modifiedGradient.Evaluate(originalColorKey.time),
                    originalColorKey.time);

                modifiedGradient.SetKeys(
                    modifiedGradient.colorKeys.Append(newColorKey).ToArray(),
                    modifiedGradient.alphaKeys);
            }

            GradientAlphaKey[] alphaKeys = modifiedGradient.alphaKeys;

            lifeTimeGradient = new ParticleSystem.MinMaxGradient(modifiedGradient)
            {
                gradient =
                {
                    alphaKeys = alphaKeys,
                },
            };

            colorOverLifeTime.color = lifeTimeGradient;

            mainColorGradient.color = color;
            particleMainModule.startColor = mainColorGradient;
        }
    }
}