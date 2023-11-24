using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.StaticData.Storable
{
    [CreateAssetMenu(fileName = "New item data", menuName = "Static Data/Storable")]
    public class StorableStaticData : ScriptableObject
    {
        public StorableType ItemType;
        public Sprite Sprite;
        public string Name;
        public string Description;
        public bool IsArtifact;
        public bool IsInteractive;
        public bool IsReloadable;

        [ShowIf(EConditionOperator.And, nameof(IsInteractive), nameof(IsReloadable))] [Range(0, 30f)]
        public float ReloadTime;
    }
}