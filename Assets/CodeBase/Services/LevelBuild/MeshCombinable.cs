using UnityEngine;

namespace CodeBase.Services.LevelBuild
{
    public class MeshCombinable : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private MeshFilter _meshFilter;

        public Renderer Renderer => _renderer;

        public MeshFilter Filter => _meshFilter;

        private void OnValidate()
        {
            _renderer ??= GetComponent<Renderer>();
            _meshFilter ??= GetComponent<MeshFilter>();
        }
    }
}