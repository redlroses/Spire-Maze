using System;
using System.Collections.Generic;
using CodeBase.DelayRoutines;
using CodeBase.Logic.Сollectible;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class ItemCollectedView : MonoCache
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _target;
        [SerializeField] private RectTransform _destination;
        [SerializeField] private RectTransform _lights;
        [SerializeField] private Image _itemIcon;

        [Space] [Header("Speed")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private float _rotateSpeed;

        [Space] [Header("Settings")]
        [SerializeField] private float _verticalPositionOffset;
        [SerializeField] private float _scalingPositionOffset;
        [SerializeField] private float _delay;

        [Space] [Header("Curves")]
        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _scaleCurve;

        private RoutineSequence _animation;
        private Camera _camera;
        private Queue<Action> _commandQueue = new Queue<Action>();
        private ItemCollector _collector;

        private TowardMover<Vector2> _mover;
        private TowardMover<Vector2> _scaler;

        private Vector3 _fromPosition;

        private void Awake()
        {
            this.Disable();
            _camera = Camera.main;

            _mover = new TowardMover<Vector2>(Vector2.Lerp, _moveCurve);
            _scaler = new TowardMover<Vector2>(Vector2.Lerp, _scaleCurve);

            _animation = new RoutineSequence()
                .Then(InitUpscaling)
                .WaitWhile(TryProcess)
                .Then(InitMoving)
                .WaitForSeconds(_delay)
                .WaitWhile(TryProcess)
                .Then(EndAnimation);
        }

        private void OnDestroy()
        {
            _collector.Collected -= OnCollected;
            _animation.Kill();
        }

        public void Construct(ItemCollector collector)
        {
            _collector = collector;
            collector.Collected += OnCollected;
        }

        protected override void Run() =>
            _lights.Rotate(Vector3.forward, Time.deltaTime * _rotateSpeed);

        private void EndAnimation()
        {
            if (_commandQueue.TryDequeue(out Action action))
                action.Invoke();
            else
                this.Disable();
        }

        private void InitUpscaling()
        {
            this.Enable();

            _mover.SetFrom(_fromPosition);
            _mover.SetTo(_fromPosition.ChangeY(_fromPosition.y + _scalingPositionOffset));
            _mover.Reset();

            _scaler.SetFrom(Vector2.zero);
            _scaler.SetTo(Vector2.one);
            _scaler.Reset();
        }

        private void InitMoving()
        {
            _mover.SetFrom(_fromPosition.ChangeY(_fromPosition.y + _scalingPositionOffset));
            _mover.SetTo(GetDestinationPosition());
            _mover.Reset();

            _scaler.SetFrom(Vector2.one);
            _scaler.SetTo(Vector2.zero);
            _scaler.Reset();
        }

        private bool TryProcess()
        {
            bool inProcess = _mover.TryUpdate(Time.deltaTime * _moveSpeed, out Vector2 position);
            _target.anchoredPosition = position;

            inProcess = _scaler.TryUpdate(Time.deltaTime * _scaleSpeed, out Vector2 scale) && inProcess;
            _target.localScale = scale;

            return inProcess;
        }

        private void OnCollected(Sprite icon, Vector3 at)
        {
            if (_animation.IsActive)
            {
                Vector3 position = AsScreenPosition(at);
                _commandQueue.Enqueue(() => PlayAnimation(icon, position));
            }
            else
            {
                PlayAnimation(icon, AsScreenPosition(at));
            }
        }

        private void PlayAnimation(Sprite icon, Vector3 at)
        {
            _itemIcon.sprite = icon;
            _fromPosition = at.ChangeX(0);
            _animation.Play();
        }

        private Vector3 AsScreenPosition(Vector3 at)
        {
            Vector3 worldToScreenPoint = _camera.WorldToScreenPoint(at.ChangeY(at.y + _verticalPositionOffset));
            return _canvas.transform.InverseTransformPoint(worldToScreenPoint);
        }

        private Vector3 GetDestinationPosition() =>
            _canvas.transform.InverseTransformPoint(_destination.position);
    }
}