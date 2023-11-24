using System;
using Agava.YandexGames;
using AYellowpaper.SerializedCollections;
using CodeBase.DelayRoutines;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.HealthEntity;
using CodeBase.Logic.HealthEntity.Damage;
using CodeBase.Logic.Hero;
using CodeBase.Logic.Portal;
using CodeBase.Logic.Сollectible;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Sound
{
    public class HeroSound : AudioClipSource
    {
        [Header("References")]
        [SerializeField] private HeroHealth _heroHealth;
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private ItemCollector _itemCollector;
        [SerializeField] private Teleportable _teleportable;

        [Space] [Header("AnimationBased")]
        [SerializeField] private SerializedDictionary<AnimatorState, AudioClip> _movementClips;

        [Space] [Header("Other")]
        [SerializeField] private AudioClip _collectSound;
        [SerializeField] private AudioClip _teleportedSound;

        [Space] [Header("Damage")]
        [SerializeField] private AudioClip _periodicDamagedSound;
        [SerializeField] private AudioClip _singleDamagedSound;
        [SerializeField] private float _periodicRate;

        [Space] [Header("Steps Collection")]
        [SerializeField] private AudioClip[] _stepSounds;

        private RoutineSequence _playPeriodicDamageSound;

        private void Awake()
        {
            _playPeriodicDamageSound = new RoutineSequence()
                .WaitForSeconds(_periodicRate)
                .Then(() => PlayOneShot(_periodicDamagedSound));
        }

        private void OnEnable()
        {
            _heroHealth.Damaged += OnDamaged;
            _heroAnimator.StateEntered += OnStateEntered;
            _heroAnimator.StateExited += OnStateExited;
            _heroAnimator.StepMoved += OnStepMoved;
            _itemCollector.Collected += OnPlaySoundCollected;
            _teleportable.Teleported += OnPlaySoundTeleported;
        }

        private void OnDamaged(int damage, DamageType type)
        {
            if (_heroHealth.IsAlive == false)
            {
                return;
            }

            if (type == DamageType.Periodic)
            {
                if (_playPeriodicDamageSound.IsActive == false)
                {
                    _playPeriodicDamageSound.Play();
                }
            }
            else
            {
                PlayOneShot(_singleDamagedSound);
            }
        }

        private void OnDisable()
        {
            _heroAnimator.StepMoved -= OnStepMoved;
            _heroAnimator.StateEntered -= OnStateEntered;
            _heroAnimator.StateExited -= OnStateExited;
            _itemCollector.Collected -= OnPlaySoundCollected;
            _teleportable.Teleported -= OnPlaySoundTeleported;
        }

        private void OnStateExited(AnimatorState state) { }

        private void OnStateEntered(AnimatorState state)
        {
            if (_movementClips.TryGetValue(state, out AudioClip clip))
                PlayOneShot(clip);
        }

        private void OnPlaySoundCollected(Sprite _, Vector3 __)
        {
            PlayOneShot(_collectSound);
        }

        private void OnPlaySoundTeleported()
        {
            PlayOneShot(_teleportedSound);
        }

        private void OnStepMoved()
        {
            PlayOneShot(_stepSounds.GetRandom());
        }
    }
}