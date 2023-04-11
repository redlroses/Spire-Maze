using CodeBase.Data;
using TheraBytes.BetterUi;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class RankView : MonoBehaviour
    {
        [SerializeField] private TextSetter _rank;
        [SerializeField] private TextSetter _name;
        [SerializeField] private TextSetter _score;
        [SerializeField] private Image _avatar;
        [SerializeField] private Image _flag;
        [SerializeField] private BetterImage _selfIndication;
        
        public void Set(SingleRankData singleRankData)
        {
            _rank.SetText(singleRankData.Rank);
            _name.SetText(singleRankData.Name);
            _score.SetText(singleRankData.Score);
            _avatar.sprite = singleRankData.Avatar;
            _flag.sprite = singleRankData.Flag;
        }

        public void EnableSelfIndication()
        {
            _selfIndication.enabled = true;
        }
    }
}