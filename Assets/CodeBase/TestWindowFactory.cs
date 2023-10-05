using CodeBase.Services;
using CodeBase.UI.Services.Windows;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase
{
    public class TestWindowFactory : MonoBehaviour
    {
        private IWindowService _windowService;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
        }

        [Button("Open Leaderboard")]
        private void OpenLeaderboard()
        {
            _windowService.Open(WindowId.Leaderboard);
        }

        [Button("Open Settings")]
        private void OpenSettings()
        {
            _windowService.Open(WindowId.Settings);
        }
        
        [Button("Open Pause")]
        private void OpenPause()
        {
            _windowService.Open(WindowId.Pause);
        }
        
        [Button("Open Result")]
        private void OpenResult()
        {
            _windowService.Open(WindowId.Results);
        }
        
        [Button("Open Lose")]
        private void OpenLose()
        {
            _windowService.Open(WindowId.Lose);
        }
        
    }
}