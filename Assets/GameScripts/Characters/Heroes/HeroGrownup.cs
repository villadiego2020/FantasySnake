using FS.Cores;
using UnityEngine;

namespace FS.Characters.Heroes
{
    public class HeroGrownup : CharacterGrownup
    {
        public HeroesBehavior Behavior => Self as HeroesBehavior;

        protected override void Update()
        {
            if (Self == null || Self.IsDead == true) return;

            int timer = Mathf.FloorToInt(GameManager.instance.GameTime * Data.GrownRate);

            if (timer >= PreviousUpdateStatTime)
            {
                PreviousUpdateStatTime += Data.GrownEveryTime;
             
                if(Behavior.IsCollected == true)
                {
                    OnCharacterGrownEvent?.Invoke(Data);
                }
            }
        }
    }
}