using AYellowpaper.SerializedCollections;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Player;
using CodeBase.Logic.Portal;
using CodeBase.Logic.Сollectible;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Sound
{
    public class HeroSound : AudioClipSource
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private ItemCollector _itemCollector;
        [SerializeField] private Teleportable _teleportable;
        [SerializeField] private SerializedDictionary<AnimatorState, AudioClip> _movementClips;
        [SerializeField] private AudioClip _collectSound;
        [SerializeField] private AudioClip _teleportedSound;
        [SerializeField] private AudioClip[] _stepSounds;

        private void OnEnable()
        {
            _heroAnimator.StateEntered += OnStateEntered;
            _heroAnimator.StemMoved += OnStepMoved;
            _itemCollector.SoundPlayed += OnPlaySoundCollected;
            _teleportable.Teleported += OnPlaySoundTeleported;
        }

        private void OnDisable()
        {
            _heroAnimator.StemMoved -= OnStepMoved;
            _heroAnimator.StateEntered -= OnStateEntered;
            _itemCollector.SoundPlayed -= OnPlaySoundCollected;
            _teleportable.Teleported -= OnPlaySoundTeleported;
        }

        private void OnStateEntered(AnimatorState state)
        {
            if (_movementClips.TryGetValue(state, out AudioClip clip))
                PlayOneShot(clip);
        }

        private void OnPlaySoundCollected()
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