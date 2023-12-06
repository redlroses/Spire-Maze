using System;
using CodeBase.Data;
using CodeBase.Tools.Extension;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Tools.Builds
{
    [CreateAssetMenu(menuName = "Create Build Info", fileName = "New Build Info", order = 0)]
    public class BuildInfo : ScriptableObject
    {
        [SerializeField] private string _buildDateTimeString;

        public DateTime BuildDateTime
        {
            get => _buildDateTimeString.ToDeserialized<DateTimeData>().AsDateTime();
            set => _buildDateTimeString = value.AsDateTimeData().ToJson();
        }

        private void OnValidate() =>
            SetNow();

        [Button]
        private void SetNow() =>
            BuildDateTime = DateTime.Now;
    }
}