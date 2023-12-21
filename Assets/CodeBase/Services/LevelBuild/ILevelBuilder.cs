using CodeBase.LevelSpecification;
using CodeBase.StaticData;
using Cysharp.Threading.Tasks;

namespace CodeBase.Services.LevelBuild
{
    public interface ILevelBuilder : IService
    {
        Level Build(LevelStaticData levelStaticData);

        UniTask ConstructLevel();

        void Clear();
    }
}