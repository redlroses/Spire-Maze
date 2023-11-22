using System;
using CodeBase.Tutorial;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Tutorial Config", fileName = "TutorialConfig")]
    public class TutorialConfig : ScriptableObject
    {
        [SerializeField] [ReorderableList] private int[] _triggerCellIndices;
        [SerializeField] [ReorderableList] private TutorialModule[] _modules;

        public TutorialModule GetModule(int byIndex)
        {
            if (_modules.Length <= byIndex)
                throw new IndexOutOfRangeException();

            return _modules[byIndex];
        }
    }
}