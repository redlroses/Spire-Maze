using System;
using System.Collections.Generic;
using CodeBase.FileRead;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.Debug
{
    public class TestMonoCash : MonoCache
    {
        private void Start()
        {
            GetNew();
        }

        private void GetNew()
        {
            MapReader mapReader = new MapReader();
            MapData mapdata = mapReader.GetMapData(1);
            print(mapdata.Data);
        }

        private void GetOld()
        {
            string textFile = Resources.Load("MapFiles/Level1").ToString();
            List<String> words = new List<string>(textFile.Split('\n'));
            foreach (var item in words)
            {
                print(item);
            }
        }
    }
}
