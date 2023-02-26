using System;
using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public class TriggerObserver<TTarget> : MonoBehaviour
    {
        public event Action<TTarget> Entered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TTarget collectible) == false)
            {
                return;
            }

            Entered?.Invoke(collectible);
        }
    }
}