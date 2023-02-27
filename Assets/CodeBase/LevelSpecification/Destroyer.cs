using UnityEngine;

namespace CodeBase.LevelSpecification
{
    public class Destroyer : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}