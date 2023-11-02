using System;
using UnityEngine;

namespace CodeBase.Logic.Observer
{
    public class TriggerObserverExit<TTarget> : TriggerObserver<TTarget>, ITriggerObserverExit<TTarget>
    {
        public event Action<TTarget> Exited = _ => { };

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out TTarget collectible) == false)
            {
                return;
            }

            Exited.Invoke(collectible);
        }
    }
}