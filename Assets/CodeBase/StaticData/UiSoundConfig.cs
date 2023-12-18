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
        [SerializeField] private SerializedDictionary<SelectionState, AudioClip> _dropdownClips;
        [SerializeField] private AudioClip _sliderClip;

        public IReadOnlyDictionary<SelectionState, AudioClip> ButtonClips => _buttonClips;

        public IReadOnlyDictionary<SelectionState, AudioClip> ToggleClips => _toggleClips;

        public IReadOnlyDictionary<SelectionState, AudioClip> DropdownClips => _dropdownClips;

        public AudioClip SliderClip => _sliderClip;
    }
}