using AYellowpaper.SerializedCollections;
using CodeBase.Logic.Items;
using CodeBase.StaticData.Storable;
using CodeBase.Tools.Extension;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class Key : Collectible
    {
        [SerializeField] private ParticleSystem[] _effects;
        [SerializeField] private SerializedDictionary<StorableType, GameObject> _objects;
        [SerializeField] private SerializedDictionary<StorableType, Color> _colors;

        protected override void OnConstruct(IItem item)
        {
            EnableKey(item.ItemType);
        }

        protected override void OnCollected() =>
            gameObject.Disable();

        private void EnableKey(StorableType type)
        {
            _objects[type].SetActive(true);

            foreach (ParticleSystem effect in _effects)
                effect.Colorize(_colors[type]);
        }
    }
}