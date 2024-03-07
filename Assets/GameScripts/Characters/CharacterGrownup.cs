using FS.Cores;
using FS.Datas;
using System;
using UnityEngine;

namespace FS.Characters
{
    public class CharacterGrownup : MonoBehaviour
    {
        [HideInInspector] public IBehavior Self;
        [HideInInspector] public int PreviousUpdateStatTime;
        [HideInInspector] public GrownUpData Data;
        private float m_CrazyTimeRate = 1f;

        public Action<GrownUpData> OnCharacterGrownEvent;


        private void OnEnable()
        {
            GameManager.instance.OnGameStateUpdateEvent += GameStateChange;
        }

        private void OnDisable()
        {
            GameManager.instance.OnGameStateUpdateEvent -= GameStateChange;
        }

        private void GameStateChange(GameState state)
        {
            if (state == GameState.CrazyTime)
            {
                if(Self.CharacterType == CharacterType.Monster) // In crazy time monster wil boost grown rate
                {
                    m_CrazyTimeRate += 2f;
                }
            }
        }

        protected virtual void Update()
        {
            if(Self == null || Self.IsDead == true) return;

            int timer = Mathf.FloorToInt(GameManager.instance.GameTime * Data.GrownRate * m_CrazyTimeRate);

            if(timer >= PreviousUpdateStatTime)
            {
                PreviousUpdateStatTime += Data.GrownEveryTime;
                OnCharacterGrownEvent?.Invoke(Data);
            }
        }
    }
}