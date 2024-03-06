using FS.Characters;
using FS.Cores;
using System.Collections.Generic;
using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Game", menuName = "FS/Configiration/Game", order = 1)]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Start Control Hero")]
        public CharacterData StarterControlHero;

        [Header("Hero")]
        public Character Hero;

        [Header("Monster")]
        public Character Monster;

        [Header("Obstacle")]
        public int NumberOfObstacle;
        public ObstacleData[] ObstacleDatas;
    }

    [System.Serializable]
    public struct Character
    {
        public float Movmenet;
        public int StartAmount;
        public GrownUpData GrownCoefficient;
        public List<ChanceSpawnData> ChanceSpawn;
        public CharacterData[] Data;
    }

    [System.Serializable]
    public struct GrownUpData
    {
        public int GrownEveryTime;
        public float GrownRate;
        public GrownValue MinMaxHPStat;
        public GrownValue MinMaxAttackStat;
    }

    [System.Serializable]
    public struct GrownValue
    {
        public Vector2Int MinMaxStat;
        public float Value;
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

    [System.Serializable]
    public struct ChanceSpawnData
    {
        public float Chance;
        public int Amount;
    }
}