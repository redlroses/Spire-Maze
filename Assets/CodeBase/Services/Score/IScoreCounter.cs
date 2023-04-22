using CodeBase.Data;

namespace CodeBase.Services.Score
{
    public interface IScoreCounter
    {
        public int UpdateScore(ScoreAccumulationData scoreAccumulationData);
    }
}