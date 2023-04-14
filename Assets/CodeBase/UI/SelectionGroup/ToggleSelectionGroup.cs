using System;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.UI.SelectionGroup
{
    public abstract class ToggleSelectionGroup<TEnum> : MonoBehaviour where TEnum : Enum
    {
        [SerializeField] private SelectionToggle<TEnum>[] _selectionsToggle;

        private SelectionToggle<TEnum> _currentSelected = default;

        private void OnEnable()
        {
            Subscribe();
        }

        protected void SetDefault(TEnum defaultId)
        {
            _currentSelected = _selectionsToggle.First(toggle => Equals(toggle.Id, defaultId));
            _currentSelected.Select();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        [Button("Collect Selection Toggles")] [UsedImplicitly]
        private void CollectSelectionToggles()
        {
            _selectionsToggle = GetComponentsInChildren<SelectionToggle<TEnum>>();
        }

        private void Subscribe()
        {
            foreach (var toggle in _selectionsToggle)
            {
                toggle.Selected += OnToggleSelected;
            }
        }
        
        private void Unsubscribe()
        {
            foreach (var toggle in _selectionsToggle)
            {
                toggle.Selected -= OnToggleSelected;
            }
        }

        private void OnToggleSelected(SelectionToggle<TEnum> selected)
        {
            if (Equals(selected.Id, _currentSelected.Id))
            {
                return;
            }        
            
            _currentSelected.Unselect();
            selected.Select();
            _currentSelected = selected;
            
            OnSelectionChanged(_currentSelected.Id);
        }

        protected abstract void OnSelectionChanged(TEnum id);
    }
}