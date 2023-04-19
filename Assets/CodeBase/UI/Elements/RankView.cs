using CodeBase.Data;
using TheraBytes.BetterUi;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class RankView : MonoBehaviour
    {
        [SerializeField] private TextSetter _rank;
        [SerializeField] private TextSetter _name;
        [SerializeField] private TextSetter _score;
        [SerializeField] private BetterImage _avatar;
        [SerializeField] private BetterImage _flag;
        [SerializeField] private GameObject _selfIndication;
        
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
            _selfIndication.SetActive(true);
        }
    }
}