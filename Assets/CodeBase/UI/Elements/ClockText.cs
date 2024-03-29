﻿using System;
using CodeBase.Infrastructure;
using CodeBase.Services.Watch;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.UI.Elements
{
    public class ClockText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _format = @"mm\:ss";

        private IWatchService _watchService;

        private void OnDestroy() =>
            Cleanup();

        public void Construct(IWatchService watchService)
        {
            if (IsBuildableLevel() == false)
            {
                gameObject.SetActive(false);

                return;
            }

            _watchService = watchService;
            SetTime(_watchService.ElapsedSeconds);
            Subscribe();
        }

        private void SetTime(int seconds) =>
            _text.text = TimeSpan.FromSeconds(seconds).ToString(_format);

        private void Cleanup()
        {
            if (_watchService is null)
                return;

            _watchService.SecondTicked -= SetTime;
        }

        private void Subscribe() =>
            _watchService.SecondTicked += SetTime;

        private bool IsBuildableLevel() =>
            string.Equals(SceneManager.GetActiveScene().name, LevelNames.BuildableLevel);
    }
}