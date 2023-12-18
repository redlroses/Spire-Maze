using System;
using System.Collections.Generic;
using CodeBase.DelayRoutines;
using CodeBase.Tools;
using CodeBase.Tools.Extension;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Sound.Music
{
    [Serializable]
    public class CrossFadeAudioSource
    {
        [SerializeField] private AudioSource _firstAudioSource;
        [SerializeField] private AudioSource _secondAudioSource;
        [SerializeField] [Range(0f, 1f)] private float _maxVolume = 1f;

        [SerializeField] [CurveRange(0f, 0f, 1f, 1f, EColor.Indigo)]
        private AnimationCurve _crossCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private Queue<AudioSource> _audioSourcesQueue;
        private TowardMover<float> _crossFader;
        private RoutineSequence _crossFadeRoutine;

        public AudioSource PlayingSource { get; private set; }

        public void Init(float crossFadeTime)
        {
            _crossFader = CreateCrossFader();
            _crossFadeRoutine = CreateCrossFadeRoutine(crossFadeTime);
            _audioSourcesQueue = new Queue<AudioSource>(2);
            _audioSourcesQueue.Enqueue(_firstAudioSource);
            _audioSourcesQueue.Enqueue(_secondAudioSource);
        }

        public void PlayClip(AudioClip clip)
        {
            AudioSource source = _audioSourcesQueue.Dequeue();
            PlayingSource = source;
            source.clip = clip;
            source.time = 0;
            source.Play();
            _audioSourcesQueue.Enqueue(source);
            CrossFadeVolume();
        }

        private void CrossFadeVolume()
        {
            _crossFader.Switch();
            _crossFadeRoutine.Play();
        }

        private TowardMover<float> CreateCrossFader() =>
            new TowardMover<float>(0f, _maxVolume, Mathf.Lerp, _crossCurve);

        private RoutineSequence CreateCrossFadeRoutine(float crossFadeTime)
        {
            return new RoutineSequence()
                .WaitWhile(() =>
                {
                    bool isProcess = _crossFader.TryUpdate(Time.deltaTime / crossFadeTime, out float volume);

                    _firstAudioSource.volume = _maxVolume - volume;
                    _secondAudioSource.volume = volume;

                    return isProcess;
                }).Then(() =>
                {
                    if (_firstAudioSource.volume.EqualsApproximately(0))
                    {
                        _firstAudioSource.Stop();
                    }
                    else
                    {
                        _secondAudioSource.Stop();
                    }
                });
        }
    }
}