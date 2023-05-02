using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.Rendering;
using Door = CodeBase.LevelSpecification.Cells.Door;
using EnemySpawnPoint = CodeBase.LevelSpecification.Cells.EnemySpawnPoint;
using FireTrap = CodeBase.LevelSpecification.Cells.FireTrap;
using Key = CodeBase.LevelSpecification.Cells.Key;
using Plate = CodeBase.LevelSpecification.Cells.Plate;
using Portal = CodeBase.LevelSpecification.Cells.Portal;
using Rock = CodeBase.LevelSpecification.Cells.Rock;
using Savepoint = CodeBase.LevelSpecification.Cells.Savepoint;
using SpikeTrap = CodeBase.LevelSpecification.Cells.SpikeTrap;
using Wall = CodeBase.LevelSpecification.Cells.Wall;

namespace CodeBase.Services.LevelBuild
{
    public class LevelConstructor
    {
        private const string Colliders = "Colliders";
        private const string Spire = "Spire";
        private const string Ground = "Ground";
        private const int MaxCellsInCollider = 8;

        private readonly CellConstructor _cellConstructor = new CellConstructor();

        private IGameFactory _gameFactory;

        public void Construct(IGameFactory gameFactory, IStaticDataService staticData, Level level)
        {
            _gameFactory = gameFactory;
            _cellConstructor.Construct<Plate>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Plate).ToArray());
            _cellConstructor.Construct<Wall>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Wall).ToArray());
            _cellConstructor.Construct<Key>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Key).ToArray());
            _cellConstructor.Construct<Door>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Door).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(gameFactory, staticData,
                level.Where(cell => cell.CellData is MovingMarker).ToArray());
            _cellConstructor.Construct<Portal>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Portal).ToArray());
            _cellConstructor.Construct<SpikeTrap>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.SpikeTrap).ToArray());
            _cellConstructor.Construct<FireTrap>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.FireTrap).ToArray());
            _cellConstructor.Construct<Rock>(gameFactory, staticData,
                level.Where((cell => cell.CellData is EditorCells.Rock)).ToArray());
            _cellConstructor.Construct<Savepoint>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.Savepoint).ToArray());
            _cellConstructor.Construct<EnemySpawnPoint>(gameFactory, staticData,
                level.Where(cell => cell.CellData is EditorCells.EnemySpawnPoint).ToArray());
               CombineCells(level);
        }

        private void CombineCells(Level level)
        {
            GameObject groupsHolder = CombineAllColliders(level);
            // CombineAllMeshes(level.SelfTransform, groupsHolder);
        }

        private GameObject CombineAllColliders(Level level)
        {
            GameObject collidersHolder = new GameObject(Colliders);
            collidersHolder.transform.parent = level.SelfTransform;

            CombineByType<EditorCells.Plate>(level, collidersHolder);
            CombineByType<EditorCells.Wall>(level, collidersHolder);

            collidersHolder.isStatic = true;

            return collidersHolder;
        }

        private void CombineByType<TCell>(Level level, GameObject collidersHolder) where TCell : CellData
        {
            for (int i = 0; i < level.Height; i++)
            {
                if (HasFloorCellType<TCell>(level, i) == false)
                {
                    continue;
                }

                int airIndex = FindFirstGap<TCell>(level.Container[i]);
                CombineColliderGroups<TCell>(level.Container[i], airIndex, collidersHolder.transform);
            }
        }

        private static bool HasFloorCellType<TCell>(Level level, int floor) where TCell : CellData
        {
            for (int j = 0; j < level.Width; j++)
            {
                if (level.GetCell(floor, j).CellData is TCell)
                {
                    return true;
                }
            }

            return false;
        }

        private void CombineAllMeshes(Transform spire, GameObject groupsHolder)
        {
            MeshFilter[] meshesFilters = groupsHolder.GetComponentsInChildren<MeshFilter>();
            MeshFilter[] meshFilters = meshesFilters.Append(spire.GetComponent<MeshFilter>()).ToArray();
            CombineInstance[] combine = CombineInstances(meshFilters);
            ApplyMesh(spire.gameObject, combine, true);
            spire.gameObject.isStatic = true;
        }

        private void CombineColliderGroups<TCell>(Floor floor, int beginIndex, Transform collidersHolder)
            where TCell : CellData
        {
            if (beginIndex == -1)
            {
                CombineColliders(floor.Container.GetRange(0, floor.Container.Count), collidersHolder);
                return;
            }

            int firstGroupIndex = FindFirstGroupIndex<TCell>(floor, beginIndex);
            int initialGroupIndex = firstGroupIndex;

            do
            {
                int lastGroupIndex = FindLastGroupIndex<TCell>(floor, firstGroupIndex);

                if (lastGroupIndex < firstGroupIndex)
                {
                    List<Cell> combined =
                        floor.Container.GetRange(firstGroupIndex, floor.Container.Count - firstGroupIndex);
                    combined.AddRange(floor.Container.GetRange(0, lastGroupIndex));
                    CombineColliders(combined, collidersHolder);
                }
                else
                {
                    CombineColliders(floor.Container.GetRange(firstGroupIndex, lastGroupIndex - firstGroupIndex),
                        collidersHolder);
                }

                firstGroupIndex = FindFirstGroupIndex<TCell>(floor, lastGroupIndex);
            } while (initialGroupIndex != firstGroupIndex);
        }

        private void CombineColliders(List<Cell> cells, Transform parent)
        {
            if (cells.Count > MaxCellsInCollider)
            {
                CombineColliders(cells.GetRange(0, cells.Count / 2), parent);
                CombineColliders(cells.GetRange(cells.Count / 2, Mathf.FloorToInt(cells.Count / 2f)), parent);
                return;
            }
            
            GameObject cell = cells.First().Container.gameObject;
            GameObject colliderHolder = new GameObject($"{cell.name} collider");
            colliderHolder.transform.parent = parent;
            colliderHolder.isStatic = true;

            // MeshFilter[] meshesFilters =
            //     cells.Select(container => container.Container.GetComponentInChildren<MeshFilter>()).ToArray();

            MeshCollider[] meshColliders =
                cells.Select(container => container.Container.GetComponentInChildren<MeshCollider>()).ToArray();

            MeshFilter[] meshesFilters =
                meshColliders.Select(container => container.gameObject.GetComponent<MeshFilter>()).ToArray();

            // MeshFilter[] meshesFilters;
            // List<MeshFilter> meshesFiltersList;
            // meshesFiltersList = new List<MeshFilter>();

            // meshesFiltersList = cells.Aggregate(meshesFiltersList, (current1, cell1) => cell1.Container.GetComponentsInChildren<MeshFilter>().Aggregate(current1, (current, componentsInChild) => current.Append(componentsInChild).ToList()));

            // meshesFilters = meshesFiltersList.ToArray();

            foreach (MeshCollider meshCollider in meshColliders)
            {
                meshCollider.enabled = false;
            }

            CombineInstance[] combine = CombineInstances(meshesFilters);

            Mesh mesh = ApplyMesh(colliderHolder, combine, false);
            CreateMeshCollider(colliderHolder, mesh, cell.transform.GetChild(0).gameObject.layer);
        }

        private void CreateMeshCollider(GameObject colliderHolder, Mesh mesh, int layer)
        {
            MeshCollider meshCollider = colliderHolder.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            meshCollider.material = _gameFactory.CreatePhysicMaterial(Ground);
            colliderHolder.layer = layer;
        }

        private CombineInstance[] CombineInstances(MeshFilter[] meshesFilters)
        {
            CombineInstance[] combine = new CombineInstance[meshesFilters.Length];

            for (int i = 0; i < meshesFilters.Length; i++)
            {
                combine[i].mesh = meshesFilters[i].sharedMesh;
                combine[i].transform = meshesFilters[i].transform.localToWorldMatrix;
            }

            return combine;
        }

        private Mesh ApplyMesh(GameObject holder, CombineInstance[] combine, bool isEnableRenderer)
        {
            if (holder.TryGetComponent(out MeshFilter meshFilter) == false)
            {
                meshFilter = holder.AddComponent<MeshFilter>();
            }

            if (holder.TryGetComponent(out MeshRenderer meshRenderer) == false)
            {
                meshRenderer = holder.AddComponent<MeshRenderer>();
            }

            Mesh mesh = new Mesh {indexFormat = IndexFormat.UInt32};
            meshFilter.mesh = mesh;
            meshFilter.mesh.CombineMeshes(combine);
            meshRenderer.material = _gameFactory.CreateMaterial(Spire);
            meshRenderer.enabled = isEnableRenderer;
            return mesh;
        }

        private int FindFirstGroupIndex<TCell>(Floor floor, int beginIndex) where TCell : CellData
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                if (floor.Container[currentIndex].CellData is TCell)
                {
                    break;
                }

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            return currentIndex;
        }

        private int FindLastGroupIndex<TCell>(Floor floor, int beginIndex) where TCell : CellData
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                if (floor.Container[currentIndex].CellData is TCell == false)
                {
                    break;
                }

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            return currentIndex.ClampRound(0, floor.Container.Count);
        }

        private int FindFirstGap<TCell>(Floor floor) where TCell : CellData
        {
            for (int i = 0; i < floor.Container.Count; i++)
            {
                if (floor.Container[i].CellData is TCell == false)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}