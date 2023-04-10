using System;
using System.Collections.Generic;
using CodeBase.Leaderboard;
using UnityEngine;

namespace CodeBase.UI
{
    public class LeaderboardView : MonoBehaviour
    {
        [SerializeField] private LoadingAnimation _loadingAnimation;
        [SerializeField] private Transform _ranksViewContainer;
        [SerializeField] private string _leaderboardName;
        [SerializeField] private int _leaderboardTopPlayersCount;
        [SerializeField] private int _leaderboardCompetingPlayersCount;

        private List<RankView> _ranksView;
        private ILeaderBoard _leaderBoard;

        private bool _isLeaderboardDataReceived;

        private void Awake()
        {
            FindRanksView();
        }

        private void Start()
        {
#if UNITY_EDITOR
            _leaderBoard = new EditorLeaderBoard();
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
            _leaderBoard =
                new YandexLeaderBoard(_leaderboardName, _leaderboardTopPlayersCount, _leaderboardCompetingPlayersCount);
#endif
        }

        public async void ShowLeaderBoard()
        {
            _loadingAnimation.Play();
            var ranksData = await _leaderBoard.GetLeaderboardEntries();
            _loadingAnimation.Stop();
            int ranksCount = Math.Min(_ranksViewContainer.childCount, ranksData.Length);

            for (int i = 0; i < ranksCount; i++)
            {
                _ranksView[i].Set(ranksData[i]);
                _ranksView[i].gameObject.SetActive(true);
            }
        }

        public void HideLeaderBoard()
        {
            foreach (var rankView in _ranksView)
            {
                rankView.gameObject.SetActive(false);
            }
        }

        public void SetScore(int score, string avatarName)
        {
            _leaderBoard.SetScore(score, avatarName);
        }

        private void FindRanksView()
        {
            _ranksView = new List<RankView>(_ranksViewContainer.childCount);
            _ranksView.AddRange(_ranksViewContainer.GetComponentsInChildren<RankView>());

            foreach (var rank in _ranksView)
            {
                rank.gameObject.SetActive(false);
            }
        }
    }
}