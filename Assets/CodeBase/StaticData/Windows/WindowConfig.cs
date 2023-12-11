using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
    [CreateAssetMenu(menuName = "Static Data/Window Configs", fileName = "WindowConfigs")]
    public class WindowConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<WindowId, GameObject> _windows;

        public IReadOnlyDictionary<WindowId, GameObject> Windows => _windows;
    }
}