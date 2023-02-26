using System;
using System.Collections.Generic;
using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Logic
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private IGameFactory _gameFactory;
        private Dictionary<Colors, Material> _materials;

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _materials = new Dictionary<Colors, Material>
            {
                [Colors.Red] = _gameFactory.CreateColoredMaterial(Colors.Red),
                [Colors.Green] = _gameFactory.CreateColoredMaterial(Colors.Green),
                [Colors.Blue] = _gameFactory.CreateColoredMaterial(Colors.Blue),
            };
        }

        public void SetMaterial(Colors color)
        {
            if (_materials.TryGetValue(color, out Material material) == false)
            {
                throw new ArgumentException("Invalid Color type", nameof(color));
            }

            _renderer.material = material;
        }
    }
}