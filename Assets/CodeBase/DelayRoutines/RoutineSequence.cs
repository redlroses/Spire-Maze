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

        /// <summary>
        ///     Launch routine.
        /// </summary>
        /// <param name="mode">
        ///     Start type: AtFirst - start from the beginning,
        ///     Continue - continue from where it stopped.
        /// </param>
        public void Play(RoutinePlayMode mode = RoutinePlayMode.AtFirst)
        {
            if (_isAutoKill)
                LastRoutine.AddNext(new Executor(Kill));

            if (IsActive)
                Stop();

            _routines[mode == RoutinePlayMode.AtFirst ? 0 : _currentRoutineIndex].Play();
        }

        /// <summary>
        ///     Stops the coroutine, clears the list of tweens.
        /// </summary>
        public void Kill()
        {
            foreach (IRoutine routine in _routines)
                routine.Stop();

            _routines.Clear();
            _currentRoutineIndex = 0;
        }

        /// <summary>
        ///     Adds a command to stop the routine and unload resources at the end of the sequence (similar to Kill()).
        /// </summary>
        /// <param name="isAutoKill">False by default.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence SetAutoKill(bool isAutoKill)
        {
            _isAutoKill = isAutoKill;
            return this;
        }

        /// <summary>
        ///     Stops the routine but does not reset the pointer to the current tween.
        /// </summary>
        public void Stop() =>
            ActiveRoutine?.Stop();

        #region Wait

        /// <summary>
        ///     Waits for the specified amount of time.
        /// </summary>
        /// <param name="seconds">Waiting time.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence WaitForSeconds(float seconds)
        {
            AddToSequence(new ConstTimeAwaiter(seconds, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Waits for a random amount of time from a given range.
        /// </summary>
        /// <param name="timeRange">Random time selection range.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence WaitForRandomSeconds(Vector2 timeRange)
        {
            AddToSequence(new RandomTimeAwaiter(timeRange, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Unrealized part.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>Self routine.</returns>
        public RoutineSequence WaitForEvent(Action action)
        {
            AddToSequence(new EventAwaiter(action, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Waits for the given time awaiter.
        /// </summary>
        /// <param name="timeAwaiter">Awaiter for wait.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence Wait(TimeAwaiter timeAwaiter)
        {
            AddToSequence(timeAwaiter);
            return this;
        }

        /// <summary>
        ///     Waits until the condition is false.
        /// </summary>
        /// <param name="waitFor">Waiting func.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence WaitUntil(bool waitFor)
        {
            AddToSequence(new UntilAwaiter(() => waitFor, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Waits until the condition is met.
        /// </summary>
        /// <param name="waitFor">Waiting func.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence WaitUntil(Func<bool> waitFor)
        {
            AddToSequence(new UntilAwaiter(waitFor, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Waits while the condition is true.
        /// </summary>
        /// <param name="waitFor">Waiting func.</param>
        /// <returns>Self routine</returns>
        public RoutineSequence WaitWhile(bool waitFor)
        {
            AddToSequence(new WhileAwaiter(() => waitFor, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        /// <summary>
        ///     Waits while the condition is met.
        /// </summary>
        /// <param name="waitFor">Waiting func.</param>
        /// <returns>Self routine</returns>
        public RoutineSequence WaitWhile(Func<bool> waitFor)
        {
            AddToSequence(new WhileAwaiter(waitFor, _globalUpdate, _addUpdater, _removeUpdater));
            return this;
        }

        #endregion

        #region Then

        /// <summary>
        ///     Includes an action in a sequence.
        /// </summary>
        /// <param name="action">Action to include.</param>
        /// <returns>Self routine</returns>
        public RoutineSequence Then(Action action)
        {
            AddToSequence(new Executor(action));
            return this;
        }

        /// <summary>
        ///     Includes an Executor in a sequence.
        /// </summary>
        /// <param name="executor">Given executor.</param>
        /// <returns>Self routine</returns>
        public RoutineSequence Then(Executor executor)
        {
            AddToSequence(executor);
            return this;
        }

        #endregion

        #region LoopFor

        /// <summary>
        ///     Creates a loop between the start of the sequence and the current location in the sequence.
        /// </summary>
        /// <param name="times">Iterations count.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopFor(int times)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Includes a loop object in a sequence.
        /// </summary>
        /// <param name="loopFor">Given loop object.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopFor(LoopFor loopFor)
        {
            loopFor.AddLoopStart(FirstRoutine);
            AddToSequence(loopFor);
            return this;
        }

        /// <summary>
        ///     Creates a loop between the given position in the sequence and the current position in the sequence.
        /// </summary>
        /// <param name="times">Iterations count.</param>
        /// <param name="from">Where to start the loop.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopFor(int times, IRoutine from)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(from);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Creates a loop between the given position in the sequence and the current position in the sequence.
        /// </summary>
        /// <param name="times">Iterations count.</param>
        /// <param name="fromIndex">The index of the object in the sequence from which to start the loop.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopFor(int times, int fromIndex)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Includes the loop object in the sequence from the given position.
        /// </summary>
        /// <param name="loopFor">Given loop object.</param>
        /// <param name="from">Where to start the loop.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopFor(LoopFor loopFor, IRoutine from)
        {
            loopFor.AddLoopStart(from);
            AddToSequence(loopFor);
            return this;
        }

        #endregion

        #region LoopWhile

        /// <summary>
        ///     Creates a while loop from the beginning of the sequence to the current position.
        /// </summary>
        /// <param name="repeatCondition">Loop exit condition function.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopWhile(Func<bool> repeatCondition)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Creates a while loop from the beginning of the sequence to the current position.
        /// </summary>
        /// <param name="isRepeat">Loop exit condition boolean.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopWhile(bool isRepeat)
        {
            LoopWhile routine = new LoopWhile(() => isRepeat);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Creates a while loop from the given position of the sequence to the current position.
        /// </summary>
        /// <param name="repeatCondition">Loop exit condition.</param>
        /// <param name="from">Where to start the loop.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopWhile(Func<bool> repeatCondition, IRoutine from)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(from);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Creates a while loop from the given position of the sequence to the current position.
        /// </summary>
        /// <param name="repeatCondition">Loop exit condition.</param>
        /// <param name="fromIndex">The index of the element in the sequence from which to start the loop.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopWhile(Func<bool> repeatCondition, int fromIndex)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);
            return this;
        }

        /// <summary>
        ///     Adds a loop object to the sequence.
        /// </summary>
        /// <param name="loopWhile">Loop object.</param>
        /// <returns>Self routine.</returns>
        public RoutineSequence LoopWhile(LoopWhile loopWhile)
        {
            loopWhile.AddLoopStart(FirstRoutine);
            AddToSequence(loopWhile);
            return this;
        }

        /// <summary>
        ///     adds the loop object to the sequence starting from the given position.
        /// </summary>
        /// <param name="loopWhile">Loop object.</param>
        /// <param name="fromIndex">The index of the element in the sequence from which to start the loop.</param>
        /// <returns>Self routine.</returns>
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