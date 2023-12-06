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
            Debug.Log($"Prebuild started at: {report.summary.buildStartedAt}");
            BuildInfo buildInfo = Resources.Load<BuildInfo>(AssetPath.BuildInfo);
            buildInfo.BuildDateTime = report.summary.buildStartedAt.AddHours(5);
        }
    }
}
#endif