using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Logic
{
    public class MaterialChanger : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;

        private IGameFactory _gameFactory;

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void SetMaterial(Colors color)
        {
            _renderer.material = _gameFactory.CreateColoredMaterial(color);
        }
    }
}