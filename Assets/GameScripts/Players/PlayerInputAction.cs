using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FS.Cores.Players
{
    [DefaultExecutionOrder(ScriptOrders.PLAYER_INPIUT_ORDER)]
    public class PlayerInputAction : MonoBehaviour
    {
        public static PlayerInputAction Instance;

        public Action<Vector2> OnMoveEvent;
        public Action OnSwitchHeroEvent;

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

        private void OnSwitchHero(InputValue value)
        {
            OnSwitchHeroEvent?.Invoke();
        }
    }
}