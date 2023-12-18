using System;
using System.Diagnostics.CodeAnalysis;

namespace CodeBase.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ItemData
    {
        public int Count;
        public int StorableType;

        public ItemData(int count, int storableType)
        {
            Count = count;
            StorableType = storableType;
        }
    }
}