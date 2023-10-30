using CodeBase.Sound;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Tools
{
    public class SoundConfigSetter : MonoBehaviour
    {
        [Button]
        private void Load()
        {
            foreach (var button in GetComponentsInChildren<ButtonClipSource>(true))
            {
                button.LoadConfig();
            }

            foreach (var button in GetComponentsInChildren<ToggleClipSource>(true))
            {
                button.LoadConfig();
            }
        }
    }
}