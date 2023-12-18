﻿using CodeBase.Data;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class TopRankViewConfigurator : MonoBehaviour
    {
        [SerializeField] private BetterImageSetter _image;
        [SerializeField] private TextSetter _text;
        [SerializeField] private TopRankViewData[] _topRanksSetUpData = new TopRankViewData[3];

        public void SetUp(int rank)
        {
            TopRankViewData topRankViewSetUp = _topRanksSetUpData[rank];
            _image.SetImage(topRankViewSetUp.Image);
            _text.SetText(rank + 1);
            _text.SetTextColor(topRankViewSetUp.TextColor);
        }
    }
}