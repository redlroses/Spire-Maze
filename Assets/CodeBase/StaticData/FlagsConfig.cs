using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "FlagsConfig", menuName = "Static Data/FlagsConfig")]
    public class FlagsConfig : ScriptableObject
    {
        private const string Default = "default";

        [SerializeField] private SerializedDictionary<string, Sprite> _flags;

        public IReadOnlyDictionary<string, Sprite> Flags => _flags;

        public Sprite DefaultFlag => Flags[Default];
    }
}