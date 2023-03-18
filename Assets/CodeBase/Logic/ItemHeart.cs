using CodeBase.Logic.HealthEntity;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ItemHeart : MonoBehaviour
    {
        [SerializeField] private int _health = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHealable healable) == false)
            {
                return;
            }

            healable.Heal(_health);
            Destroy(gameObject);
        }
    }
}