using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.UI
{
    public class CompassArrowPanel : MonoCache
    {
        [SerializeField] private Transform _arrow;
        [SerializeField] private Vector3 _heroPositionOffset;

        private Vector3 _finishPosition;
        private Transform _hero;

        public void Construct(Vector3 finishPosition, Transform hero, float lifetime)
        {
            _hero = hero;
            _finishPosition = finishPosition;
            Destroy(gameObject, lifetime);
        }

        protected override void LateRun()
        {
            Vector3 lookDirection = (_finishPosition - (_hero.position + _heroPositionOffset)).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _arrow.rotation = Quaternion.Lerp(_arrow.rotation, rotation, Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            if (_hero == null)
            {
                return;
            }

            Gizmos.DrawSphere(_heroPositionOffset + _hero.position, 0.5f);
            Gizmos.DrawSphere(_finishPosition, 0.5f);
            Gizmos.DrawLine(_heroPositionOffset + _hero.position, _finishPosition);
        }
    }
}