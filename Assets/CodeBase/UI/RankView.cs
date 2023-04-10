using CodeBase.Data;
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

        public void Set(RanksData ranksData)
        {
            _rank.SetText(ranksData.Rank);
            _name.SetText(ranksData.Name);
            _score.SetText(ranksData.Score);
            _avatar.sprite = ranksData.Avatar;
            _flag.sprite = ranksData.Flag;
        }
    }
}