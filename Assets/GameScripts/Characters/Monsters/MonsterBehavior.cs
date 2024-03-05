using FS.Cores;
using FS.Cores.Formulas;
using UnityEngine;

namespace FS.Characters.Monsters
{
    public class MonsterBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Monster;

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