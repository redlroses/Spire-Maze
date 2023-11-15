using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{
    [HelpURL("https://documentation.therabytes.de/better-ui/BetterTextMeshPro-Dropdown.html")]
    [AddComponentMenu("Better UI/TextMeshPro/Better TextMeshPro - Dropdown", 30)]
    public class BetterTextMeshProDropdown : TMP_Dropdown, IBetterTransitionUiElement
    {
        private const int ShownState = 5;
        private const int HiddenState = 6;

        public List<Transitions> BetterTransitions { get { return betterTransitions; } }
        public List<Transitions> ShowHideTransitions { get { return showHideTransitions; } }

        [SerializeField, ShowHideTransitionStatesAttribute]
        List<Transitions> showHideTransitions = new List<Transitions>();

        [SerializeField, DefaultTransitionStates]
        List<Transitions> betterTransitions = new List<Transitions>();

        [SerializeField] private Canvas _topLayout;

        public event Action<int> StateChanged = _ => { };

        protected void OnValidate()
        {
            _topLayout ??= GetComponent<Canvas>();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (!(base.gameObject.activeInHierarchy))
                return;

            StateChanged.Invoke((int) state);

            foreach (var info in betterTransitions)
            {
                info.SetState(state.ToString(), instant);
            }
        }

        protected override GameObject CreateDropdownList(GameObject template)
        {
            foreach (var tr in showHideTransitions)
            {
                tr.SetState("Show", false);
            }

            if (_topLayout != null)
            {
                _topLayout.overrideSorting = true;
            }

            StateChanged.Invoke(ShownState);

            return base.CreateDropdownList(template);
        }

        protected override void DestroyDropdownList(GameObject dropdownList)
        {
            foreach (var tr in showHideTransitions)
            {
                tr.SetState("Hide", false);
            }

            if (_topLayout != null)
            {
                _topLayout.overrideSorting = false;
            }

            StateChanged.Invoke(HiddenState);

            base.DestroyDropdownList(dropdownList);
        }
    }
}