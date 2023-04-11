namespace CodeBase.Data
{
    public struct RanksData
    {
        public readonly SingleRankData[] TopThreeRanks;
        public readonly SingleRankData[] AroundRanks;
        public readonly SingleRankData SelfRank;

        public RanksData(SingleRankData[] topThreeRanks, SingleRankData[] aroundRanks, SingleRankData selfRank)
        {
            TopThreeRanks = topThreeRanks;
            AroundRanks = aroundRanks;
            SelfRank = selfRank;
        }
    }
}