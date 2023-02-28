using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[ExecuteInEditMode]
public class TestMeshCombine : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private MeshFilter[] _meshesFilters;

    [ContextMenu("Combine")]
    public void Combine()
    {
        CombineInstance[] combine = new CombineInstance[_meshesFilters.Length];

        Debug.Log(_meshesFilters.Length);
        for (int i = 0; i < _meshesFilters.Length; i++)
        {
            combine[i].mesh = _meshesFilters[i].sharedMesh;
            combine[i].transform = _meshesFilters[i].transform.localToWorldMatrix;
            _meshesFilters[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
        GetComponent<MeshRenderer>().material = _material;
    }
}
