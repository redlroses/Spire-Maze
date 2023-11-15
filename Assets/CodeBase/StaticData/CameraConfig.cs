using CodeBase.Logic.Cameras;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(menuName = "Static Data/CameraConfig", fileName = "CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private Orientations _orientation;
        [SerializeField] private float _fieldOfView;
        [SerializeField] private Vector3 _offsetRotation;
        [SerializeField] private Vector3 _offsetPosition;

        public Orientations Orientation => _orientation;
        public float FieldOfView => _fieldOfView;
        public Vector3 OffsetRotation => _offsetRotation;
        public Vector3 OffsetPosition => _offsetPosition;
    }
}