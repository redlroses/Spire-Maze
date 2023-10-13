using System;

namespace CodeBase.Data
{
    [Serializable]
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