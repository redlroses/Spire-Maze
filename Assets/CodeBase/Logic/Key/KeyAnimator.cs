using AYellowpaper.SerializedCollections;
using CodeBase.StaticData.Storable;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Key
{
    public class KeyAnimator : MonoCache
    {
        [SerializeField] private ParticleSystem _particleAura;
        [SerializeField] private ParticleSystem _particleStar;
        [SerializeField] private SerializedDictionary<StorableType, Color> _colors;
        [SerializeField] private int _speedRotate = 50;

        private Vector3 _rotation = Vector3.zero;

        protected override void Run()
        {
            _rotation.y = _speedRotate * Time.deltaTime;
            transform.Rotate(_rotation);
        }

        public void SetColor(StorableType type)
        {
            ParticleSystem.MainModule particleMainModule = _particleAura.main;
            ParticleSystem.MinMaxGradient colorGradient = particleMainModule.startColor;
            colorGradient.color = _colors[type];
            particleMainModule.startColor = colorGradient;

            particleMainModule = _particleStar.main;
            particleMainModule.startColor = colorGradient;
        }
    }
}