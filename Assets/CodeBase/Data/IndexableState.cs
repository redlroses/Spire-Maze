using System;
using System.Diagnostics.CodeAnalysis;

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