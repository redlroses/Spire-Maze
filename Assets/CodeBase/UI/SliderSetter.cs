using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderSetter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private bool _isAnimated;
        
    }
}
