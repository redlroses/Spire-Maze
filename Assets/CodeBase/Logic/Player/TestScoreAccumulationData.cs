using CodeBase.Logic.Inventory;
using CodeBase.StaticData.Storable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.Player
{
    public class TestScoreAccumulationData : MonoBehaviour
    {
        [SerializeField] private HeroInventory _heroInventory;
        [SerializeField] private StorableStaticData _scoreItem;

        private void OnEnable()
        {
            InputAction input = new InputAction("Press B", InputActionType.Button, "<Keyboard>/B");
            input.started += context =>
            {
                _heroInventory.Inventory.Add(_scoreItem);
            };

            input.Enable();
        }
    }
}
