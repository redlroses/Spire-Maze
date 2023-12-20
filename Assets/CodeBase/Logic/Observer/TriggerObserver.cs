using System;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public class TriggerObserver<TTarget> : MonoBehaviour, ITriggerObserver<TTarget>
    {
        public event Action<TTarget> Entered = _ => { };

        private void OnValidate()
        {
            if (GetComponent<Collider>() == null)
                Debug.LogWarning("TriggerObserver component must be attached to a Collider");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out TTarget collectible) == false)
                return;

            Entered.Invoke(collectible);
        }
    }
}