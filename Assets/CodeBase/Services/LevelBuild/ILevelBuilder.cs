using CodeBase.LevelSpecification;
using CodeBase.StaticData;

namespace CodeBase.Services.LevelBuild
{
    public interface ILevelBuilder : IService
    {
        Level Build(LevelStaticData levelStaticData);
        void Construct();
        void Clear();
    }
}