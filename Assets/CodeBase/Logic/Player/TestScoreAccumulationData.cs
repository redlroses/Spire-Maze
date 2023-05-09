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

        private InputAction _input;

        private void OnEnable()
        {
            _input = new InputAction("Press B", InputActionType.Button, "<Keyboard>/B");
            _input.started += AddItem;
            _input.Enable();
        }

        private void AddItem(InputAction.CallbackContext context) => _heroInventory.Inventory.Add(_scoreItem);

        private void OnDisable()
        {
            _input.started -= AddItem;
            _input.Disable();
        }
    }
}