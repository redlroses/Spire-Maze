using System;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic
{
    public class InteractionHandler : MonoCache
    {
        public event Action PlacedPlates;

        private void OnCollisionPlate(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Plate plate))
            {
                PlacedPlates?.Invoke();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionPlate(collision);
        }
    }
}