using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS.Cores.Players
{
    [DefaultExecutionOrder(ScriptOrders.PLAYER_INPIUT_ORDER)]
    public class PlayerInputAction : MonoBehaviour
    {
        public static PlayerInputAction Instance;
        [SerializeField] private InputActionAsset m_InputAsset;

        public Action<Vector2> OnMoveEvent;
        public Vector2 MoveInput { get; private set; } = Vector2.zero;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
        }

        private void OnMovevment(InputValue value)
        {
            OnMoveEvent?.Invoke(value.Get<Vector2>());
        }
    }
}