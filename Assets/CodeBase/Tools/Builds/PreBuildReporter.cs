#if UNITY_EDITOR
using CodeBase.Infrastructure.AssetManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CodeBase.Tools.Builds
{
    public class PreBuildReporter : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildInfo buildInfo = Resources.Load<BuildInfo>(AssetPath.BuildInfo);
            buildInfo.SetNow();
        }
    }
}
#endif