using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CodeBase.Sound;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "UiSoundConfig", menuName = "Static Data/Ui Sound Config")]
    public class UiSoundConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<SelectionState, AudioClip> _buttonClips;
        [SerializeField] private SerializedDictionary<SelectionState, AudioClip> _toggleClips;
        [SerializeField] AudioClip _sliderClip;

        public IReadOnlyDictionary<SelectionState, AudioClip> ButtonClips => _buttonClips;
        public IReadOnlyDictionary<SelectionState, AudioClip> ToggleClips => _toggleClips;
        public AudioClip SliderClip => _sliderClip;
    }
}