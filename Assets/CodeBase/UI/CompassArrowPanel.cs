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

        protected override void LateRun()
        {
            Vector3 lookDirection = (_finishPosition - (_hero.position + _heroPositionOffset)).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            _arrow.rotation = Quaternion.Lerp(_arrow.rotation, rotation, Time.deltaTime);
        }

        public void Construct(Vector3 finishPosition, Transform hero)
        {
            _hero = hero;
            _finishPosition = finishPosition;
        }

        public void Destroy() =>
            Destroy(gameObject);

        private void OnDrawGizmos()
        {
            if (_hero == null)
                return;

            Vector3 heroPosition = _hero.position;
            Gizmos.DrawSphere(_heroPositionOffset + heroPosition, 0.5f);
            Gizmos.DrawSphere(_finishPosition, 0.5f);
            Gizmos.DrawLine(_heroPositionOffset + heroPosition, _finishPosition);
        }
    }
}