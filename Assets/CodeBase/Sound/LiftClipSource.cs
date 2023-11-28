using System;
using AYellowpaper;
using CodeBase.DelayRoutines;
using CodeBase.Logic.Lift;
using CodeBase.Tools.Constants;
using UnityEngine;

namespace CodeBase.Sound
{
    public class LiftClipSource : AudioClipSource
    {
        [SerializeField] private AudioClip _movingSound;
        [SerializeField] private InterfaceReference<ILiftPlate, MonoBehaviour> _liftPlate;

        private RoutineSequence _volumeUpdater;

        private void OnEnable() =>
            _liftPlate.Value.StateChanged += OnStateChanged;

        private void OnDisable() =>
            _liftPlate.Value.StateChanged -= OnStateChanged;

        private void OnDestroy() =>
            _volumeUpdater?.Kill();

        private void OnStateChanged(LiftState state)
        {
            switch (state)
            {
                case LiftState.Idle:
                    EndPlaying();
                    break;
                case LiftState.Moving:
                    BeginPlaying();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void EndPlaying()
        {
            StopPlaying();
            _volumeUpdater.Stop();
        }

        private void BeginPlaying()
        {
            Play(_movingSound, true);
            _volumeUpdater ??= CreateVolumeUpdater();
            _volumeUpdater.Play();
        }

        private RoutineSequence CreateVolumeUpdater()
        {
            return _volumeUpdater = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitWhile(() =>
                {
                    SetVolume(_liftPlate.Value.Mover.Velocity * Arithmetic.ToHalf);
                    return enabled;
                });
        }
    }
}