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

        public Action<GrownUpData> OnCharacterGrownEvent;

        protected virtual void Update()
        {
            if(Self == null || Self.IsDead == true) return;

            int timer = Mathf.FloorToInt(GameManager.instance.GameTime * Data.GrownRate);

            if(timer >= PreviousUpdateStatTime)
            {
                PreviousUpdateStatTime += Data.GrownEveryTime;
                OnCharacterGrownEvent?.Invoke(Data);
            }
        }
    }
}