using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class GraphicLevelDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown.value = QualitySettings.GetQualityLevel();
            _dropdown.onValueChanged.AddListener(OnLevelChanged);
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }

        private void OnLevelChanged(int level)
        {
            QualitySettings.SetQualityLevel(level);
        }
    }
}