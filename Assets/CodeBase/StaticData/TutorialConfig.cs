using System;
using System.Collections.Generic;
using CodeBase.Tutorial;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Tutorial Config", fileName = "TutorialConfig")]
    public class TutorialConfig : ScriptableObject
    {
        [SerializeField] [ReorderableList] private TutorialModule[] _modules;

        public int ModulesLength => _modules.Length;

        public IEnumerable<TutorialModule> Modules => _modules;

        public TutorialModule GetModule(int byIndex)
        {
            if (_modules.Length <= byIndex || byIndex < 0)
                throw new IndexOutOfRangeException();

            return _modules[byIndex];
        }
    }
}