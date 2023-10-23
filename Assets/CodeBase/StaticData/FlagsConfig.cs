using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "FlagsConfig", menuName = "Static Data/FlagsConfig")]
    public class FlagsConfig : ScriptableObject
    {
        private const string Default = "default";

        public SerializedDictionary<string, Sprite> Flags;
        public Sprite DefaultFlag => Flags[Default];
    }
}