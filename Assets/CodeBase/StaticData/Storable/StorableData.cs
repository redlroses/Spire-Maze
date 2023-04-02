using UnityEngine;

namespace CodeBase.StaticData.Storable
{
    [CreateAssetMenu(fileName = "New item data", menuName = "Static Data/Storable")]
    public class StorableData : ScriptableObject, IStorable
    {
        public StorableType ItemType;
        public Sprite Sprite;
        public string Name;
        public string Description;
    }
}