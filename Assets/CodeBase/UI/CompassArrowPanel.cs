using CodeBase.Tools.Extension;
using NTC.Global.Cache;
using UnityEngine;

namespace CodeBase.UI
{
    public class CompassArrowPanel : MonoCache
    {
        private readonly Vector3 _modificationRotation = new Vector3(90, 0, 0);

        [SerializeField] private RectTransform _arrow;
        [SerializeField] private Vector3 _heroPositionOffset;

        private Vector3 _finishPosition;
        private Transform _hero;

        public void Construct(Vector3 finishPosition, Transform hero, float lifetime)
        {
            _hero = hero;
            _finishPosition = finishPosition;
        }

        protected override void LateRun()
        {
            Vector3 lookDirection = (_finishPosition - (_hero.position + _heroPositionOffset)).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, transform.position.ChangeY(0));
            rotation *= Quaternion.Euler(_modificationRotation);
            _arrow.rotation = Quaternion.Lerp(_arrow.rotation, rotation, Time.deltaTime);
        }
    }
}