using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Logic
{
    public class ConverterTest : MonoBehaviour
    {
        [SerializeField] private Vector2 _position;
        [SerializeField] private Transform _obj;

        private void Update()
        {
            _obj.transform.position = _position.ToWorldPosition(Spire.DistanceToCenter);
        }
    }
}