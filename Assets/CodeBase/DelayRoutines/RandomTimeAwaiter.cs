using System;
using NTC.Global.Cache;
using UnityEngine;
using Random = UnityEngine.Random;


namespace CodeBase.DelayRoutines
{
    public sealed class RandomTimeAwaiter : TimeAwaiter
    {
        private readonly Vector2 _timeRange;

        public RandomTimeAwaiter(Vector2 timeRange, GlobalUpdate globalUpdate, Action<GlobalUpdate, Awaiter> addUpdater,
            Action<GlobalUpdate, Awaiter> removeUpdater) : base(globalUpdate, addUpdater, removeUpdater) =>
            _timeRange = timeRange;

        protected override float GetWaitTime() =>
            Random.Range(_timeRange.x, _timeRange.y);
    }
}