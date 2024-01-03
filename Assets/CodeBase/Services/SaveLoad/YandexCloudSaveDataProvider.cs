using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.SaveLoad
{
    public class YandexCloudSaveDataProvider : ICloudSaveDataProvider
    {
        public bool IsPlayerAuthorized() =>
            PlayerAccount.IsAuthorized;

        public async UniTask<string> Load()
        {
            bool isLoading = true;
            string saveData = null;

            PlayerAccount.GetCloudSaveData(
                saves =>
                {
                    saveData = saves;
                    isLoading = false;
                },
                error =>
                {
                    isLoading = false;
                });

            while (isLoading)
                await UniTask.Yield();

            return saveData;
        }

        public void Save(string saveData)
        {
            PlayerAccount.SetCloudSaveData(
                saveData,
                null,
                error => throw new Exception($"Cloud save error: {error}"));
        }
    }
}