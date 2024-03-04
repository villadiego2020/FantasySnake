using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Game", menuName = "FS/Configiration/Game", order = 1)]
    public class GameConfiguration : ScriptableObject
    {
        public int NumberOfEntitySpawn;
        public int MinStat;
        public int MaxStat;
        public float GrowCoefficient;
        [Range(1,100)] public float ChanceHeroSpawn;
        [Range(1,100)] public float ChanceMonsterSpawn;
    }
}