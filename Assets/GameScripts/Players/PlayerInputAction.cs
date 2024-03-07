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
        public Action OnSwitchRotateHeroEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void OnMovevment(InputValue value)
        {
            OnMoveEvent?.Invoke(value.Get<Vector2>());
        }

        private void OnSwitchHero(InputValue value)
        {
            OnSwitchHeroEvent?.Invoke();
        }

        public void OnSwitchRotateHero(InputValue value)
        {
            OnSwitchRotateHeroEvent?.Invoke();
        }
    }
}