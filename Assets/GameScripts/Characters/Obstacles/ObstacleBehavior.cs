using FS.Cores;
using UnityEngine;

namespace FS.Characters.Obstacles
{
    public class ObstacleBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Obstacle;

        public override void Spawned(object[] objects)
        {

        }
    }
}