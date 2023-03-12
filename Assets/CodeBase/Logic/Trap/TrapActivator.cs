using System;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Logic.Trap
{
    public class TrapActivator : MonoCache
    {
        private bool _isActivated;

        public event Action IsActived;

        private void OnTriggerEnter(Collider other)
        {
            if(_isActivated)
            {
                return;
            }

            if (other.TryGetComponent(out Player.Hero _) == false)
            {
                return;
            }

            _isActivated = true;
            IsActived?.Invoke();
        }
    }
}