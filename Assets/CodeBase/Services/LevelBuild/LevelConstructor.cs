using System.Collections.Generic;
using System.Linq;
using CodeBase.EditorCells;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.States;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using CodeBase.Services.SaveLoad;
using CodeBase.Services.StaticData;
using CodeBase.Tools.Extension;
using UnityEngine;
using UnityEngine.Rendering;
using Door = CodeBase.LevelSpecification.Cells.Door;
using EnemySpawnPoint = CodeBase.LevelSpecification.Cells.EnemySpawnPoint;
using FinishPortal = CodeBase.LevelSpecification.Cells.FinishPortal;
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

        private readonly IGameFactory _gameFactory;
        private readonly CellConstructor _cellConstructor;

        public LevelConstructor(IGameFactory gameFactory,
            IStaticDataService staticData,
            GameStateMachine stateMachine, ISaveLoadService saveLoadService)
        {
            _cellConstructor = new CellConstructor(stateMachine, saveLoadService, staticData);
            _gameFactory = gameFactory;
        }

        public void Construct(Level level)
        {
            _cellConstructor.Construct<Plate>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Plate).ToArray());
            _cellConstructor.Construct<Wall>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Wall).ToArray());
            _cellConstructor.Construct<Key>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Key).ToArray());
            _cellConstructor.Construct<Door>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Door).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(_gameFactory,
                level.Where(cell => cell.CellData is MovingMarker).ToArray());
            _cellConstructor.Construct<Portal>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Portal).ToArray());
            _cellConstructor.Construct<FinishPortal>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.FinishPortal).ToArray());
            _cellConstructor.Construct<SpikeTrap>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.SpikeTrap).ToArray());
            _cellConstructor.Construct<FireTrap>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.FireTrap).ToArray());
            _cellConstructor.Construct<Rock>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Rock).ToArray());
            _cellConstructor.Construct<Savepoint>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.Savepoint).ToArray());
            _cellConstructor.Construct<EnemySpawnPoint>(_gameFactory,
                level.Where(cell => cell.CellData is EditorCells.EnemySpawnPoint).ToArray());
            CombineCells(level);
        }

        private void CombineCells(Level level)
        {
            var groupsHolder = CombineAllColliders(level);
            // CombineAllMeshes(level.SelfTransform, groupsHolder);
        }

        private GameObject CombineAllColliders(Level level)
        {
            var collidersHolder = new GameObject(Colliders);
            collidersHolder.transform.parent = level.SelfTransform;

            CombineByType<EditorCells.Plate>(level, collidersHolder);
            CombineByType<EditorCells.Wall>(level, collidersHolder);

            collidersHolder.isStatic = true;

            return collidersHolder;
        }

        private void CombineByType<TCell>(Level level, GameObject collidersHolder) where TCell : CellData
        {
            for (var i = 0; i < level.Height; i++)
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
            for (var j = 0; j < level.Width; j++)
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
            var meshesFilters = groupsHolder.GetComponentsInChildren<MeshFilter>();
            var meshFilters = meshesFilters.Append(spire.GetComponent<MeshFilter>()).ToArray();
            var combine = CombineInstances(meshFilters);
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
                    var combined =
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

            var cell = cells.First().Container.gameObject;
            var colliderHolder = new GameObject($"{cell.name} collider");
            colliderHolder.transform.parent = parent;
            colliderHolder.isStatic = true;

            // MeshFilter[] meshesFilters =
            //     cells.Select(container => container.Container.GetComponentInChildren<MeshFilter>()).ToArray();

            var meshColliders =
                cells.Select(container => container.Container.GetComponentInChildren<MeshCollider>()).ToArray();

            var meshesFilters =
                meshColliders.Select(container => container.gameObject.GetComponent<MeshFilter>()).ToArray();

            // MeshFilter[] meshesFilters;
            // List<MeshFilter> meshesFiltersList;
            // meshesFiltersList = new List<MeshFilter>();

            // meshesFiltersList = cells.Aggregate(meshesFiltersList, (current1, cell1) => cell1.Container.GetComponentsInChildren<MeshFilter>().Aggregate(current1, (current, componentsInChild) => current.Append(componentsInChild).ToList()));

            // meshesFilters = meshesFiltersList.ToArray();

            foreach (var meshCollider in meshColliders)
            {
                meshCollider.enabled = false;
            }

            var combine = CombineInstances(meshesFilters);

            var mesh = ApplyMesh(colliderHolder, combine, false);
            CreateMeshCollider(colliderHolder, mesh, cell.transform.GetChild(0).gameObject.layer);
        }

        private void CreateMeshCollider(GameObject colliderHolder, Mesh mesh, int layer)
        {
            var meshCollider = colliderHolder.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;
            meshCollider.material = _gameFactory.CreatePhysicMaterial(Ground);
            colliderHolder.layer = layer;
        }

        private CombineInstance[] CombineInstances(MeshFilter[] meshesFilters)
        {
            var combine = new CombineInstance[meshesFilters.Length];

            for (var i = 0; i < meshesFilters.Length; i++)
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

            var mesh = new Mesh {indexFormat = IndexFormat.UInt32};
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
            for (var i = 0; i < floor.Container.Count; i++)
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