using System;
using System.Collections.Generic;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace CodeBase.DelayRoutines
{
    public partial class RoutineSequence : IDisposable
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly List<IRoutine> _routines = new List<IRoutine>();

        private readonly Action<GlobalUpdate, Awaiter> _addUpdater;
        private readonly Action<GlobalUpdate, Awaiter> _removeUpdater;

        private int _currentRoutineIndex = -1;
        private bool _isAutoKill;

        public RoutineSequence(RoutineUpdateMod updateMod = RoutineUpdateMod.Run)
        {
            _globalUpdate = Singleton<GlobalUpdate>.Instance;

            if (updateMod == RoutineUpdateMod.Run)
            {
                _addUpdater = (globalUpdate, self) => globalUpdate.AddRunSystem(self);
                _removeUpdater = (globalUpdate, self) => globalUpdate.RemoveRunSystem(self);
            }
            else
            {
                _addUpdater = (globalUpdate, self) => globalUpdate.AddFixedRunSystem(self);
                _removeUpdater = (globalUpdate, self) => globalUpdate.RemoveFixedRunSystem(self);
            }
        }

        public bool IsActive => ActiveRoutine is not null;

        private IRoutine FirstRoutine => _routines[0];

        private IRoutine LastRoutine => _routines[_currentRoutineIndex];

        private IRoutine ActiveRoutine => _routines.Find(routine => routine.IsActive);

        void IDisposable.Dispose() =>
            Kill();

        public void Play(RoutinePlayMode mode = RoutinePlayMode.AtFirst)
        {
            if (_isAutoKill)
                LastRoutine.AddNext(new Executor(Kill));

            if (IsActive)
                Stop();

            _routines[mode == RoutinePlayMode.AtFirst ? 0 : _currentRoutineIndex].Play();
        }

        public void Kill()
        {
            foreach (IRoutine routine in _routines)
                routine.Stop();

            _routines.Clear();
            _currentRoutineIndex = 0;
        }

        public RoutineSequence SetAutoKill(bool isAutoKill)
        {
            _isAutoKill = isAutoKill;

            return this;
        }

        public void Stop() =>
            ActiveRoutine?.Stop();

        #region Wait

        public RoutineSequence WaitForSeconds(float seconds)
        {
            AddToSequence(new ConstTimeAwaiter(seconds, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence WaitForRandomSeconds(Vector2 timeRange)
        {
            AddToSequence(new RandomTimeAwaiter(timeRange, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence WaitForEvent(Action action)
        {
            AddToSequence(new EventAwaiter(action, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence Wait(TimeAwaiter timeAwaiter)
        {
            AddToSequence(timeAwaiter);

            return this;
        }

        public RoutineSequence WaitUntil(bool waitFor)
        {
            AddToSequence(new UntilAwaiter(() => waitFor, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence WaitUntil(Func<bool> waitFor)
        {
            AddToSequence(new UntilAwaiter(waitFor, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence WaitWhile(bool waitFor)
        {
            AddToSequence(new WhileAwaiter(() => waitFor, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        public RoutineSequence WaitWhile(Func<bool> waitFor)
        {
            AddToSequence(new WhileAwaiter(waitFor, _globalUpdate, _addUpdater, _removeUpdater));

            return this;
        }

        #endregion

        #region Then

        public RoutineSequence Then(Action action)
        {
            AddToSequence(new Executor(action));

            return this;
        }

        public RoutineSequence Then(Executor executor)
        {
            AddToSequence(executor);

            return this;
        }

        #endregion

        #region LoopFor

        public RoutineSequence LoopFor(int times)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopFor(LoopFor loopFor)
        {
            loopFor.AddLoopStart(FirstRoutine);
            AddToSequence(loopFor);

            return this;
        }

        public RoutineSequence LoopFor(int times, IRoutine from)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(from);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopFor(int times, int fromIndex)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopFor(LoopFor loopFor, IRoutine from)
        {
            loopFor.AddLoopStart(from);
            AddToSequence(loopFor);

            return this;
        }

        #endregion

        #region LoopWhile

        public RoutineSequence LoopWhile(Func<bool> repeatCondition)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopWhile(bool isRepeat)
        {
            LoopWhile routine = new LoopWhile(() => isRepeat);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopWhile(Func<bool> repeatCondition, IRoutine from)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(from);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopWhile(Func<bool> repeatCondition, int fromIndex)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);

            return this;
        }

        public RoutineSequence LoopWhile(LoopWhile loopWhile)
        {
            loopWhile.AddLoopStart(FirstRoutine);
            AddToSequence(loopWhile);

            return this;
        }

        public RoutineSequence LoopWhile(LoopWhile loopWhile, int fromIndex)
        {
            loopWhile.AddLoopStart(_routines[fromIndex]);
            AddToSequence(loopWhile);

            return this;
        }

        #endregion

        private void AddToSequence(IRoutine routine)
        {
            if (_currentRoutineIndex >= 0)
                LastRoutine.AddNext(routine);

            _routines.Add(routine);
            _currentRoutineIndex++;
        }
    }
}