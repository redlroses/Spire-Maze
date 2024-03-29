using System;
using System.Collections.Generic;
using System.Diagnostics;
using CodeBase.DelayRoutines;
using JetBrains.Annotations;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Tools
{
    public abstract class Curtain<TValue> : MonoCache, IShowable, IHidable
    {
        private readonly Queue<Action> _commands = new Queue<Action>();

        [Header("Main Settings")]
        [SerializeField] private TValue _from;
        [SerializeField] private TValue _to;
        [SerializeField] private float _speed;

        [SerializeField] [CurveRange(0, 0, 1, 1, EColor.Green)]
        private AnimationCurve _modifyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] [Label("Use Command Queue")] private bool _isUseCommandQueue;

        private RoutineSequence _commandAwaiter;
        private TowardMover<TValue> _towardMover;

        private Action _onEndCallback = () => { };

        private void Awake()
        {
            enabled = false;

            _towardMover = new TowardMover<TValue>(_from, _to, GetLerpFunction(), _modifyCurve);

            _commandAwaiter = new RoutineSequence()
                .WaitWhile(enabled)
                .Then(ExecuteCommand)
                .LoopWhile(_commands.Count > 0);

            OnInitialize();
        }

        public void Show(Action onShowCallback = null)
        {
            if (onShowCallback is null)
                onShowCallback = OnShow;
            else
                onShowCallback += OnShow;

            if (enabled && _isUseCommandQueue)
            {
                _commands.Enqueue(
                    () =>
                    {
                        PlayForward();
                        AppendCallback(onShowCallback);
                    });

                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();

                return;
            }

            PlayForward();
            AppendCallback(onShowCallback);
        }

        public void ShowInstantly()
        {
            PlayForward();
            _towardMover.TryUpdate(1f, out TValue lerpValue);
            ApplyLerpValue(lerpValue);
            OnShow();
        }

        public void Hide(Action onHideCallback = null)
        {
            if (onHideCallback is null)
                onHideCallback = OnHide;
            else
                onHideCallback += OnHide;

            if (enabled && _isUseCommandQueue)
            {
                _commands.Enqueue(
                    () =>
                    {
                        PlayReverse();
                        AppendCallback(onHideCallback);
                    });

                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();

                return;
            }

            PlayReverse();
            AppendCallback(onHideCallback);
        }

        public void HideInstantly()
        {
            PlayReverse();
            _towardMover.TryUpdate(1f, out TValue lerpValue);
            ApplyLerpValue(lerpValue);
            OnHide();
        }

        protected abstract Func<TValue, TValue, float, TValue> GetLerpFunction();

        protected abstract void ApplyLerpValue(TValue lerpValue);

        protected virtual void OnInitialize()
        {
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected override void Run()
        {
            bool isActive = _towardMover.TryUpdate(Time.smoothDeltaTime * _speed, out TValue lerpValue);
            ApplyLerpValue(lerpValue);

            if (isActive != enabled)
            {
                enabled = isActive;
                OnEnd();
            }
        }

        private void OnEnd()
        {
            _onEndCallback.Invoke();
            _onEndCallback = () => { };
        }

        private void PlayForward()
        {
            _towardMover.Forward();
            enabled = true;
        }

        private void PlayReverse()
        {
            _towardMover.Reverse();
            enabled = true;
        }

        private void AppendCallback(Action callback) =>
            _onEndCallback = callback;

        private void ExecuteCommand()
        {
            if (_commands.TryDequeue(out Action action))
                action.Invoke();
        }

        #region Test

        [Button] [UsedImplicitly]
        [Conditional("UNITY_EDITOR")]
        private void TestShow() =>
            Show();

        [Button] [UsedImplicitly]
        [Conditional("UNITY_EDITOR")]
        private void TestHide() =>
            Hide();

        #endregion
    }
}