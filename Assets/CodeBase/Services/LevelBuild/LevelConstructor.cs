using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Factory;
using CodeBase.LevelSpecification;
using CodeBase.LevelSpecification.Cells;
using CodeBase.LevelSpecification.Constructor;
using CodeBase.Tools.Extension;
using UnityEngine;

namespace CodeBase.Services.LevelBuild
{
    public class LevelConstructor
    {
        private readonly CellConstructor _cellConstructor = new CellConstructor();

        private IGameFactory _gameFactory;

        public void Construct(IGameFactory gameFactory, Level level)
        {
            _gameFactory = gameFactory;
            _cellConstructor.Construct<Wall>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Wall).ToArray());
            _cellConstructor.Construct<Plate>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Plate).ToArray());
            // CombineMashes(gameFactory, level); //TODO: Комбинирование мешей целиком и коллайдеров по слитным участкам
            CombineColliders(level);
            _cellConstructor.Construct<Key>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Key).ToArray());
            _cellConstructor.Construct<Door>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.Door).ToArray());
            _cellConstructor.Construct<MovingPlateMarker>(gameFactory, level.Where(cell => cell.CellData is Data.Cell.MovingMarker).ToArray());
        }

        private void Combine(List<Cell> cells, Transform floorTransform)
        {
            Debug.Log("Begin Group");

            foreach (var item in cells)
            {
                Debug.Log(item.Container.name);
            }

            Debug.Log("End Group");

            MeshFilter[] meshesFilters = cells.Select(container => container.Container.GetComponentInChildren<MeshFilter>()).ToArray();

            var cell = cells.First().Container.gameObject;

            CombineInstance[] combine = new CombineInstance[meshesFilters.Length];
            Debug.Log(meshesFilters.Length);

            for (int i = 0; i < meshesFilters.Length; i++)
            {
                combine[i].mesh = meshesFilters[i].sharedMesh;
                combine[i].transform = meshesFilters[i].transform.localToWorldMatrix;
                meshesFilters[i].gameObject.SetActive(false);
            }

            MeshFilter meshFilter = cell.AddComponent<MeshFilter>();
            MeshCollider meshCollider = cell.AddComponent<MeshCollider>();
            MeshRenderer meshRenderer = cell.AddComponent<MeshRenderer>();
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combine);
            meshCollider.sharedMesh = meshFilter.mesh;
            meshCollider.convex = true;
            meshRenderer.material = _gameFactory.CreateMaterial("Spire");
            cell.transform.position = Vector3.zero;
            cell.transform.rotation = Quaternion.identity;
            cell.layer = cell.transform.GetChild(0).gameObject.layer;
        }

        private void CombineColliders(Level level)
        {
            for (int i = 0; i < level.Height; i++)
            {
                int airIndex = FindFirstGap(level.Container[i]);
                CombineGroups(level.Container[i], airIndex);
            }
        }

        private void CombineGroups(Floor floor, int beginIndex)
        {
            if (beginIndex == -1)
            {
                Combine(floor.Container.GetRange(0, floor.Container.Count), floor.SelfTransform);
                return;
            }

            int firstGroupIndex = FindFirstGroupIndex(floor, beginIndex);
            int initialGroupIndex = firstGroupIndex;

            do
            {
                int lastGroupIndex = FindLastGroupIndex(floor, firstGroupIndex);

                if (lastGroupIndex < firstGroupIndex)
                {
                    List<Cell> combined = floor.Container.GetRange(firstGroupIndex, floor.Container.Count - firstGroupIndex);
                    combined.AddRange(floor.Container.GetRange(0, lastGroupIndex));
                    Combine(combined, floor.SelfTransform);
                }
                else
                {
                    Combine(floor.Container.GetRange(firstGroupIndex, lastGroupIndex - firstGroupIndex), floor.SelfTransform);
                }

                firstGroupIndex = FindFirstGroupIndex(floor, lastGroupIndex);
            } while (initialGroupIndex != firstGroupIndex);
        }

        private int FindFirstGroupIndex(Floor floor, int beginIndex)
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                if (floor.Container[currentIndex].CellData is Data.Cell.Plate)
                {
                    break;
                }

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            // Debug.Log($"first index {currentIndex}");
            return currentIndex;
        }

        private int FindLastGroupIndex(Floor floor, int beginIndex)
        {
            int currentIndex = (beginIndex + 1).ClampRound(0, floor.Container.Count);

            while (currentIndex != beginIndex)
            {
                // Debug.Log($"currentIndex in last {currentIndex}");

                if (floor.Container[currentIndex].CellData is Data.Cell.Plate == false)
                {
                    break;
                }

                currentIndex = (currentIndex + 1).ClampRound(0, floor.Container.Count);
            }

            // Debug.Log($"last index {currentIndex}");
            return (currentIndex).ClampRound(0, floor.Container.Count);
        }

        private int FindFirstGap(Floor floor)
        {
            for (int i = 0; i < floor.Container.Count; i++)
            {
                if (floor.Container[i].CellData is Data.Cell.Plate == false)
                {
                    // Debug.Log($"First non Plate {i} {floor.Container[i].CellData.GetType()}");
                    return i;
                }
            }

            return -1;
        }

        private void CombineMeshes(IGameFactory gameFactory, Level level)
        {
            Transform levelSelfTransform = level.SelfTransform;
            MeshFilter[] meshesFilters = levelSelfTransform.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshesFilters.Length];
            Debug.Log(meshesFilters.Length);

            for (int i = 0; i < meshesFilters.Length; i++)
            {
                combine[i].mesh = meshesFilters[i].sharedMesh;
                combine[i].transform = meshesFilters[i].transform.localToWorldMatrix;
                meshesFilters[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            MeshFilter meshFilter = levelSelfTransform.gameObject.GetComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combine);
            MeshCollider meshCollider = levelSelfTransform.gameObject.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;
            MeshRenderer meshRenderer = levelSelfTransform.gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = gameFactory.CreateMaterial("Spire");
        }
    }
}