using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class IndexableState
    {
        public int Id;
        public bool IsActivated;

        public IndexableState(int id, bool isActivated)
        {
            Id = id;
            IsActivated = isActivated;
        }
    }
}