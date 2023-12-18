using System;
using TMPro;
using UnityEngine;
#if !UNITY_EDITOR
using CodeBase.Infrastructure.AssetManagement;
#endif

namespace CodeBase.Tools.Builds
{
    public class BuildInfoSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
#if UNITY_EDITOR
            SetInfo(DateTime.Now);

#else
            SetInfo(Resources.Load<BuildInfo>(AssetPath.BuildInfo).BuildDateTime);
#endif
        }

        private void SetInfo(DateTime info)
        {
            Debug.Log($"Build Time Set: {info:dd/MM/yy HH:mm:ss}");
            _text.text = $"Build DateTime: {info:dd/MM/yy HH:mm:ss}";
        }
    }
}