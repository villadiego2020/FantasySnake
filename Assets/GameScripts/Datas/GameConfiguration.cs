using FS.Characters;
using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Game", menuName = "FS/Configiration/Game", order = 1)]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Hero & Monster")]
        public int StartCollectHeroSpawn;
        public int StartMonsterSpawn;
        public int MinStat;
        public int MaxStat;
        public float GrowCoefficient;
        [Range(0.1f,1f)] public float ChanceCollectHeroSpawn;
        [Range(0.1f,1f)] public float ChanceMonsterSpawn;

        [Header("Obstacle")]
        public int NumberOfObstacle;
        public ObstacleData[] ObstacleDatas;

        [Header("Hero")]
        public CharacterData[] Heroes;

        [Header("Monster")]
        public CharacterData[] Monsters;
    }

    [System.Serializable]
    public struct ObstacleData
    {
        public GameObject Prefab;
        public Vector2 CellSize;
    }

    [System.Serializable]
    public struct CharacterData
    {
        public GameObject Prefab;
        public CharacterStat Stat;
    }
}