using CodeBase.Data.Cell;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Logic.Сollectible
{
    public class KeyCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private Colors _color;
        [SerializeField] private MaterialChanger _materialChanger;

        public Colors Color => _color;

        public void Construct(IGameFactory gameFactory, Colors color)
        {
            _materialChanger.Construct(gameFactory);
            _materialChanger.SetMaterial(color);
            _color = color;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}