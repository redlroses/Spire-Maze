using CodeBase.Tools.Extension;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Sound.Music
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private CrossFadeAudioSource _audioSource;
        [SerializeField] private AudioClip[] _playlist;
        [SerializeField] private bool _isRandom;
        [SerializeField] private float _crossFadeTime;

#if UNITY_EDITOR
        [Space] [Header("Editor Settings")] [SerializeField] [ReadOnly]
        private string _playingTrackName;

        [SerializeField] [ProgressBar("TrackBar", nameof(_clipLength), EColor.Violet)]
        private float _trackBar;

        [SerializeField] [Range(0.01f, 0.99f)] private float _trackSlider;
#endif

        private int _currentTrackId;
        private float _clipLength;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_trackBar.EqualsApproximately(_trackSlider) == false)
            {
                _audioSource.PlayingSource.time = _clipLength * _trackSlider - float.Epsilon;
            }
        }
#endif

        private void Awake() =>
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _audioSource.Init(_crossFadeTime);
            PlayNextTrack();
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            _trackBar = _audioSource.PlayingSource.time;
            _trackSlider = _trackBar / _clipLength;
#endif

            if (_audioSource.PlayingSource.isPlaying == false)
            {
                Debug.LogWarning("AudioSource is not playing");
                PlayNextTrack();
            }

            if (_audioSource.PlayingSource.time >= _clipLength - _crossFadeTime)
            {
                PlayNextTrack();
            }
        }

        [Button(nameof(PlayNextTrack), EButtonEnableMode.Playmode)]
        private void PlayNextTrack()
        {
            AudioClip nextTrack = GetNextTrack();
            _clipLength = nextTrack.length;
            _audioSource.PlayClip(nextTrack);

#if UNITY_EDITOR
            _playingTrackName = nextTrack.name;
#endif
        }

        private AudioClip GetNextTrack()
        {
            if (_isRandom)
                return _playlist.GetRandom();

            int nextTrackId = (_currentTrackId + 1).ClampRound(0, _playlist.Length);
            _currentTrackId = nextTrackId;

            return _playlist[nextTrackId];
        }
    }
}