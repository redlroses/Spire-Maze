using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.Logic;
using CodeBase.Tools.Constants;
using CodeBase.Tools.Extension;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using Plate = CodeBase.EditorCells.Plate;
using Wall = CodeBase.EditorCells.Wall;

namespace CodeBase.MeshCombine
{
    public class MeshCombiner
    {
        private const string Colliders = "Colliders";
        private const int MaxCellsInCollider = 8;

        private PhysicMaterial _physicMaterial;

        public async UniTask CombineAllMeshes(Transform origin, Material material)
        {
            MeshCombineMarker[] meshCombinables = origin.GetComponentsInChildren<MeshCombineMarker>();
            await UniTask.Yield();

            CombineInstance[] combine = CreateCombineInstances(meshCombinables);
            Mesh newMesh = new Mesh { indexFormat = IndexFormat.UInt32 };
            ConstructMeshHolder(origin.gameObject, combine, material, newMesh, true);
            origin.gameObject.isStatic = true;

            foreach (MeshCombineMarker meshCombinable in meshCombinables)
                meshCombinable.Renderer.enabled = false;
        }

        public async UniTask CombineAllColliders(Level level, PhysicMaterial physicMaterial)
        {
            _physicMaterial = physicMaterial;

            GameObject collidersHolder = new GameObject(Colliders)
            {
                transform =
                {
                    parent = level.Origin,
                },
            };

            await CombineByType<Plate>(level, collidersHolder);
            await CombineByType<Wall>(level, collidersHolder);

            collidersHolder.isStatic = true;
        }

        private CombineInstance[] CreateCombineInstances(MeshCombineMarker[] meshCombinables)
        {
            CombineInstance[] combine = new CombineInstance[meshCombinables.Length];

            for (int i = 0; i < meshCombinables.Length; i++)
            {
                combine[i].mesh = meshCombinables[i].Filter.sharedMesh;
                combine[i].transform = meshCombinables[i].Filter.transform.localToWorldMatrix;
            }

            return combine;
        }

        private CombineInstance[] CreateCombineInstances(MeshFilter[] meshesFilters)
        {
            CombineInstance[] combine = new CombineInstance[meshesFilters.Length];

            for (int i = 0; i < meshesFilters.Length; i++)
            {
                combine[i].mesh = meshesFilters[i].sharedMesh;
                combine[i].transform = meshesFilters[i].transform.localToWorldMatrix;
            }

            return combine;
        }

        private async UniTask CombineByType<TCell>(Level level, GameObject collidersHolder)
            where TCell : CellData
        {
            for (int i = 0; i < level.Height; i++)
            {
                if (HasFloorCellType<TCell>(level, i) == false)
                    continue;

                int airIndex = FindFirstGap<TCell>(level.Container[i]);
                await CombineColliderGroups<TCell>(level.Container[i], airIndex, collidersHolder.transform);
            }
        }

        private async UniTask CombineColliderGroups<TCell>(Floor floor, int beginIndex, Transform collidersHolder)
            where TCell : CellData
        {
            if (beginIndex == -1)
            {
                await CombineColliders(floor.Container.GetRange(0, floor.Container.Count), collidersHolder);

                return;
            }

            int firstGroupIndex = FindFirstGroupIndex<TCell>(floor, beginIndex);
            int initialGroupIndex = firstGroupIndex;

            do
            {
                int lastGroupIndex = FindLastGroupIndex<TCell>(floor, firstGroupIndex);

                if (lastGroupIndex < firstGroupIndex)
                {
                    List<Cell> combined = floor.Container
                        .GetRange(firstGroupIndex, floor.Container.Count - firstGroupIndex);

                    combined.AddRange(floor.Container.GetRange(0, lastGroupIndex));
                    await CombineColliders(combined, collidersHolder);
                }
                else
                {
                    await CombineColliders(
                        floor.Container.GetRange(firstGroupIndex, lastGroupIndex - firstGroupIndex),
                        collidersHolder);
                }

                firstGroupIndex = FindFirstGroupIndex<TCell>(floor, lastGroupIndex);
                await UniTask.Yield();
            } while (initialGroupIndex != firstGroupIndex);
        }

        private async UniTask CombineColliders(List<Cell> cells, Transform parent)
        {
            if (cells.Count > MaxCellsInCollider)
            {
                int halfCellsCount = Mathf.FloorToInt(cells.Count * Arithmetic.ToHalf);
                await CombineColliders(cells.GetRange(0, halfCellsCount), parent);
                cells = cells.GetRange(halfCellsCount, cells.Count - halfCellsCount);
                await UniTask.Yield();
            }

            GameObject cell = cells.First().Container.gameObject;

            GameObject colliderHolder = new GameObject($"{cell.name} collider")
            {
                transform =
                {
                    parent = parent,
                },
                isStatic = true,
            };

            MeshCollider[] meshColliders = cells
                .Select(container => container.Container.GetComponentInChildren<MeshCollider>(true))
                .ToArray();

            MeshFilter[] meshesFilters = meshColliders
                .Select(container => container.gameObject.GetComponent<MeshFilter>())
                .ToArray();

            foreach (MeshCollider meshCollider in meshColliders)
                meshCollider.enabled = false;

            CombineInstance[] combined = CreateCombineInstances(meshesFilters);
            Mesh mesh = new Mesh { indexFormat = IndexFormat.UInt16 };
            ConstructMeshHolder(colliderHolder, combined, null, mesh, false);
            CreateMeshCollider(colliderHolder, mesh, cell.transform.GetChild(0).gameObject.layer);
        }

        private void ConstructMeshHolder(GameObject holder, CombineInstance[] combined, Material material, Mesh mesh, bool isEnableRenderer)
        {
            if (holder.TryGetComponent(out MeshFilter meshFilter) == false)
                meshFilter = holder.AddComponent<MeshFilter>();

            if (holder.TryGetComponent(out MeshRenderer meshRenderer) == false)
                meshRenderer = holder.AddComponent<MeshRenderer>();

            meshFilter.mesh = mesh;
            meshFilter.mesh.CombineMeshes(combined);
            meshRenderer.sharedMaterial = material;
            meshRenderer.enabled = isEnableRenderer;
        }

        private void CreateMeshCollider(GameObject colliderHolder, Mesh mesh, int layer)
        {
            MeshCollider meshCollider = colliderHolder.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            meshCollider.material = _physicMaterial;
            colliderHolder.layer = layer;
        }

        private int FindFirstGroupIndex<TCell>(Floor floor, int beginIndex)
            where TCell : CellData
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                if (floor.Container[currentIndex].CellData is TCell)
                    break;

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            return currentIndex;
        }

        private int FindLastGroupIndex<TCell>(Floor floor, int beginIndex)
            where TCell : CellData
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                if (floor.Container[currentIndex].CellData is TCell == false)
                    break;

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            return currentIndex.ClampRound(0, floor.Container.Count);
        }

        private int FindFirstGap<TCell>(Floor floor)
            where TCell : CellData
        {
            for (int i = 0; i < floor.Container.Count; i++)
            {
                if (floor.Container[i].CellData is TCell == false)
                    return i;
            }

            return -1;
        }

        private bool HasFloorCellType<TCell>(Level level, int floor)
            where TCell : CellData
        {
            for (int j = 0; j < level.Width; j++)
            {
                if (level.GetCell(floor, j).CellData is TCell)
                    return true;
            }

            return false;
        }
    }
}