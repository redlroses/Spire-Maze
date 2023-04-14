using UnityEngine;

namespace CodeBase.UI.SelectionGroup
{
    public class LanguageSelection : ToggleSelectionGroup<Languages>
    {
        private void Awake()
        {
            SetDefault(Languages.Russian);
        }

        protected override void OnSelectionChanged(Languages id)
        {
            Debug.Log($"Select language: {id}");
        }
    }
}