using AYellowpaper.SerializedCollections;
using CodeBase.Logic.Items;
using CodeBase.Logic.Сollectible;
using CodeBase.StaticData.Storable;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.Logic.Key
{
    public class Key : Collectible
    {
        [SerializeField] private ColorEffects _colorSetter;
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
            _colorSetter.SetColor(_colors[type]);
        }
    }
}