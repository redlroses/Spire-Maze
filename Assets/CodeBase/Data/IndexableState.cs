using System;
using System.Data.Common;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
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