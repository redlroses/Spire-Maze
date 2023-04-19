using UnityEngine;

namespace CodeBase.UI.Windows
{
    public class ResultsWindow : WindowBase
    {
        [SerializeField] private TestWindowWrapper _testWindowWrapper;

        protected override void Initialize()
        {
            _testWindowWrapper.Play();
        }
    }
}