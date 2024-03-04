using FS.Cores;
using FS.Cores.Formulas;
using UnityEngine;

namespace FS.Characters.Heroes
{
    public class HeroesBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Hero;

        public int Order;

        public override void Spawned(object[] objects)
        {
            CharacterStat stat = objects[0] as CharacterStat;
            this.ApplyStat(stat);
        }

        public override void GetHit(IBehavior enemy)
        {
            base.GetHit(enemy);
        }
    }
}