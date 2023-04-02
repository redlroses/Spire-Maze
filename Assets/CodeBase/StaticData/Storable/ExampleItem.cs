using UnityEngine;

namespace CodeBase.StaticData.Storable
{
    [CreateAssetMenu(fileName = "New Example Item", menuName = "Static Data/Storable")]
    public class ExampleItem : ScriptableObject, IStorable
    {
        public string Data;
    }
}