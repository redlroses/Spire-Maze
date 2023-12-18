using System;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Data;
using CodeBase.Services.StaticData;
using CodeBase.StaticData;
using CodeBase.Tools;
using Cysharp.Threading.Tasks;
using I2.Loc;
using UnityEngine;

namespace CodeBase.Leaderboards
{
    public class YandexLeaderboard : ILeaderboard
    {
        private readonly string _name;
        private readonly int _topPlayersCount;
        private readonly int _competingPlayersCount;
        private readonly bool _isIncludeSelf;
        private readonly IStaticDataService _staticData;
        private readonly ImageLoader _imageLoader;
        private readonly LocalizedString _anonymousName = "Anon";

        private int _unsavedScore;
        private string _unsavedAvatarName;

        private List<SingleRankData> _ranksData;
        private SingleRankData _selfRanksData;
        private bool _isLeaderboardDataReceived;

        public YandexLeaderboard(LeaderboardStaticData leaderboard, IStaticDataService staticData)
        {
            _staticData = staticData;
            _imageLoader = new ImageLoader();
            _name = leaderboard.Name;
            _topPlayersCount = leaderboard.TopPlayersCount;
            _competingPlayersCount = leaderboard.CompetingPlayersCount;
            _isIncludeSelf = leaderboard.IsIncludeSelf;
            CheckAuthorization();
            PlayerAccount.AuthorizedInBackground += CheckAuthorization;
        }

        public bool IsAuthorized { get; private set; }

        public async UniTask<RanksData> GetRanksData()
        {
            if (_unsavedScore != 0)
            {
                await SetScore(_unsavedScore, _unsavedAvatarName);
                _unsavedScore = 0;
            }

            bool isError = false;
            _isLeaderboardDataReceived = false;

            Leaderboard.GetPlayerEntry(_name,
                OnGetPlayerEntry,
                _ => OnGetPlayerEntry(null),
                ProfilePictureSize.small);

            Leaderboard.GetEntries(_name,
                OnGetLeaderBoardEntries,
                _ => isError = true, _topPlayersCount,
                _competingPlayersCount,
                _isIncludeSelf, ProfilePictureSize.small);

            while (_isLeaderboardDataReceived == false)
            {
                if (isError)
                    throw new TimeoutException("Can't get leaderboard data");

                await UniTask.Yield();
            }

            return new RanksData(GetTopRanks(), GetCompetingRanks(), _selfRanksData);
        }

        public async UniTask SetScore(int score, string avatarName)
        {
            bool isComplete = false;

            if (IsAuthorized == false)
            {
                _unsavedScore = score;
                _unsavedAvatarName = avatarName;
                return;
            }

            Leaderboard.GetPlayerEntry(_name,
                result =>
                {
                    if (result.score >= score)
                        return;

                    Leaderboard.SetScore(_name,
                        score,
                        () => isComplete = true,
                        _ => isComplete = true,
                        avatarName);
                },
                _ => isComplete = true);

            while (isComplete == false)
                await UniTask.Yield();
        }

        public async UniTask<bool> TryAuthorize()
        {
            bool isSuccess = false;
            bool isError = false;

            if (IsAuthorized)
                return true;

            PlayerAccount.Authorize(() => isSuccess = true, _ => isError = true);

            while (isSuccess == false && isError == false)
                await UniTask.Yield();

            IsAuthorized = PlayerAccount.IsAuthorized;
            Debug.Log($"{nameof(TryAuthorize)}: isSuccess: {isSuccess}, isError: {isError}");
            return isSuccess;
        }

        public async UniTask<bool> TryRequestPersonalData()
        {
            bool isSuccess = false;
            bool isError = false;

            if (IsAuthorized == false)
                return false;

            if (PlayerAccount.HasPersonalProfileDataPermission)
                return true;

            PlayerAccount.RequestPersonalProfileDataPermission(() => isSuccess = true, _ => isError = true);

            while (isSuccess == false && isError == false)
                await UniTask.Yield();

            Debug.Log($"{nameof(TryRequestPersonalData)}: isSuccess: {isSuccess}, isError: {isError}");
            return isSuccess;
        }

        private async void OnGetPlayerEntry(LeaderboardEntryResponse result)
        {
            if (result.Equals(null))
            {
                _selfRanksData = new SingleRankData(0, 0, _staticData.GetDefaultAvatar(), _anonymousName,
                    _staticData.GetSpriteByLang(YandexGamesSdk.Environment.browser.lang));
            }

            _selfRanksData = await LoadSingleRankData(result);
        }

        private void CheckAuthorization() =>
            IsAuthorized = PlayerAccount.IsAuthorized;

        private SingleRankData[] GetCompetingRanks() =>
            _ranksData.Count > _topPlayersCount
                ? _ranksData.GetRange(_topPlayersCount, _ranksData.Count - _topPlayersCount).ToArray()
                : Array.Empty<SingleRankData>();

        private SingleRankData[] GetTopRanks() =>
            _ranksData.Count == 0
                ? Array.Empty<SingleRankData>()
                : _ranksData.GetRange(0, Math.Min(_topPlayersCount, _ranksData.Count)).ToArray();

        private async void OnGetLeaderBoardEntries(LeaderboardGetEntriesResponse board)
        {
            _ranksData = new List<SingleRankData>(board.entries.Length);

            foreach (LeaderboardEntryResponse entry in board.entries)
                _ranksData.Add(await LoadSingleRankData(entry));

            _isLeaderboardDataReceived = true;
        }

        private async UniTask<SingleRankData> LoadSingleRankData(LeaderboardEntryResponse entry)
        {
            Sprite avatar = await LoadProfileImage(entry);
            Sprite flag = _staticData.GetSpriteByLang(entry.player.lang);

            return new SingleRankData(entry.rank, entry.score, avatar,
                string.IsNullOrEmpty(entry.player.publicName) ? _anonymousName : entry.player.publicName, flag);
        }

        private async UniTask<Sprite> LoadProfileImage(LeaderboardEntryResponse entry)
        {
            Sprite avatar = null;
            bool isLoading = true;

            _imageLoader.LoadImage(entry.player.profilePicture,
                sprite =>
                {
                    avatar = sprite;
                    isLoading = false;
                },
                () =>
                {
                    avatar = _staticData.GetDefaultAvatar();
                    isLoading = false;
                });

            while (isLoading)
                await UniTask.Yield();

            return avatar;
        }
    }
}