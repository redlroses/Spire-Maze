using CodeBase.Data;
using UnityEngine;

namespace CodeBase.UI
{
    public class TopRankViewConfigurator : MonoBehaviour
    {
        [SerializeField] private BetterImageSetter _image;
        [SerializeField] private TextSetter _text;
        [SerializeField] private TopRankViewSetUp[] _topRanksSetUpData = new TopRankViewSetUp[3];
        
        public void SetUp(int rank)
        {
            TopRankViewSetUp topRankViewSetUp = _topRanksSetUpData[rank];
            _image.SetImage(topRankViewSetUp.Image);
            _text.SetText(rank + 1);
            _text.SetTextColor(topRankViewSetUp.TextColor);
        }
    }
}