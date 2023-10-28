using AYellowpaper.SerializedCollections;
using CodeBase.Logic.AnimatorStateMachine;
using CodeBase.Logic.Player;
using CodeBase.Logic.Portal;
using CodeBase.Logic.Сollectible;
using UnityEngine;

namespace CodeBase.Sound
{
    public class HeroSound:AudioClipSource
    {
        [SerializeField] private HeroAnimator _heroAnimator;
        [SerializeField] private SerializedDictionary<AnimatorState, AudioClip> _clips;
        [SerializeField] private ItemCollector _itemCollector;
        [SerializeField] private AudioClip _collectSound;
        [SerializeField] private Teleportable _teleportable;
        [SerializeField] private AudioClip _teleportedSound;
        

        private void OnEnable()
        {
            _heroAnimator.StateEntered += OnStateEntered;
            _itemCollector.SoundPlayed += OnPlaySoundCollected;
            _teleportable.Teleportaded += OnPlaySoundTeleported;
        }

        private void OnDisable()
        {
            _heroAnimator.StateEntered -= OnStateEntered;
            _itemCollector.SoundPlayed -= OnPlaySoundCollected;
        }

        private void OnStateEntered(AnimatorState state)
        {
            if (_clips.TryGetValue(state, out AudioClip clip))
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
    }
}