using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ItemHeart : MonoCache
    {
        [SerializeField] private int _health = 10;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Hero player) == false)
            {
                return;
            }

            player.Heal(_health);
            Destroy(gameObject);
        }
    }
}