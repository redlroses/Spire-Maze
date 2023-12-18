using System;
using NTC.Global.Cache;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace CodeBase.DelayRoutines
{
    public abstract class Awaiter : Routine, IRunSystem, IFixedRunSystem
    {
        private readonly Action<GlobalUpdate, Awaiter> _addUpdater;
        private readonly GlobalUpdate _globalUpdate;
        private readonly Action<GlobalUpdate, Awaiter> _removeUpdater;

        protected Awaiter(GlobalUpdate globalUpdate,
            Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater)
        {
            _globalUpdate = globalUpdate;
            _addUpdater = addUpdater;
            _removeUpdater = removeUpdater;
        }

        void IFixedRunSystem.OnFixedRun() =>
            OnUpdate(Time.fixedDeltaTime);

        void IRunSystem.OnRun() =>
            OnUpdate(Time.deltaTime);

        public void Pause()
        {
            if (IsActive)
                Deactivate();
        }

        public void Resume()
        {
            if (IsActive == false)
                Activate();
        }

        protected abstract void OnUpdate(float deltaTime);

        protected override void OnPlay() =>
            Activate();

        protected void Activate() =>
            _addUpdater.Invoke(_globalUpdate, this);

        protected void Deactivate() =>
            _removeUpdater.Invoke(_globalUpdate, this);
    }
}