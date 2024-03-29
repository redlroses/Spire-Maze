﻿using CodeBase.Data;
using CodeBase.Logic.StateMachine;
using CodeBase.Logic.StateMachine.States;
using CodeBase.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Hero
{
    public class HeroReviver : MonoBehaviour, ISavedProgress
    {
        private PlayerStateMachine _playerStateMachine;
        private int _totalReviveTokens;
        private int _leftReviveTokens;

        public int LeftReviveTokens => _leftReviveTokens;

        public void Construct(PlayerStateMachine stateMachine)
        {
            _playerStateMachine = stateMachine;
            _totalReviveTokens = 1;
            _leftReviveTokens = _totalReviveTokens;
        }

        public bool TryRevive()
        {
            if (--_leftReviveTokens > 0)
                return false;

            _totalReviveTokens = 1;
            _leftReviveTokens = _totalReviveTokens;
            _playerStateMachine.Enter<ReviveState>();

            return true;
        }

        public void LoadProgress(PlayerProgress progress) =>
            progress.WorldData.AccumulationData.TotalReviveTokens = _totalReviveTokens;

        public void UpdateProgress(PlayerProgress progress) =>
            _totalReviveTokens = progress.WorldData.AccumulationData.TotalReviveTokens;
    }
}