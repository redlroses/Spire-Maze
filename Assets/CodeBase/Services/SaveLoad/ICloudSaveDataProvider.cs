using Cysharp.Threading.Tasks;

namespace CodeBase.Services.SaveLoad
{
    public interface ICloudSaveDataProvider
    {
        bool IsPlayerAuthorized();

        UniTask<string> Load();

        void Save(string saveData);
    }
}