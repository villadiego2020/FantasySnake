using FS.Asset.Players;
using FS.Characters.Heroes;
using FS.Characters.Obstacles;
using FS.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

namespace FS.Cores.MapGenerators
{
    [DefaultExecutionOrder(ScriptOrders.MAP_GENERATOR_ORDER)]
    public class MapGenerator : IJobState
    {
        public static MapGenerator Instance;

        [Header("Map Configuration")]
        [SerializeField] private MapConfiguration m_Config;
        [SerializeField] private GameObject m_GridParent;

        [Space(5)]
        [Header("Game Configuration")]
        [SerializeField] private GameConfiguration m_GameConfiguration;
        [SerializeField] private GameObject m_ObstacleParent;

        private GridValue[,] m_Grids;

        private bool m_IsGridCreated;
        private bool m_IsObstacleCreated;
        private bool m_IsCollectHeroCreated;

        private void Awake()
        {
            Instance = this;
        }

        #region IJobState

        public override void Initialize(params object[] objects)
        {
            StartCoroutine(nameof(InitializeState));

            StartCoroutine(nameof(GenerateGrid));
            StartCoroutine(nameof(GenerateObstacle));
            StartCoroutine(GenerateCollectHero(m_GameConfiguration.StartCollectHeroSpawn));
            StartCoroutine(nameof(GenerateMonster));
        }

        public override void Deinitialize()
        {

        }

        public override void Register()
        {

        }

        public override void Unregister()
        {

        }

        #endregion

        #region Internal

        private IEnumerator InitializeState()
        {
            yield return new WaitUntil(() => m_IsGridCreated && m_IsObstacleCreated && m_IsCollectHeroCreated);

            IsDone = true;
        }

        /// <summary>
        /// Generate Grid by Map Configuration
        /// </summary>
        /// <returns></returns>
        private IEnumerator GenerateGrid()
        {
            m_Grids = new GridValue[m_Config.SizeX, m_Config.SizeY];

            for (int i = 0; i < m_Config.SizeX; i++)
            {
                for (int j = 0; j < m_Config.SizeY; j++)
                {
                    Vector2Int coordinate = new Vector2Int(i, j);

                    Vector3 position = new Vector3
                    {
                        x = i * m_Config.CellSize,
                        y = 0,
                        z = j * m_Config.CellSize
                    };

                    GameObject blockObj = Instantiate(m_Config.GridPrefab);
                    blockObj.transform.position = position;
                    blockObj.transform.localScale = Vector3.one * (m_Config.CellSize - m_Config.Margin);
                    blockObj.transform.parent = m_GridParent.transform;

                    m_Grids[i, j] = new GridValue(coordinate, position);

                    yield return null;
                }

                yield return new WaitForEndOfFrame();
            }

            m_IsGridCreated = true;
        }

        private IEnumerator GenerateObstacle()
        {
            yield return new WaitUntil(() => m_IsGridCreated);

            for (int i = 0; i < m_GameConfiguration.NumberOfObstacle; i++)
            {
                int randomCellSize = 0; //Random.Range(0, m_GameConfiguration.ObstacleDatas.Length);
                ObstacleData obstacleData = m_GameConfiguration.ObstacleDatas[randomCellSize];
                GridValue gridValue = GetRandomPosition();

                GameObject obstacleObj = Instantiate(obstacleData.Prefab);
                obstacleObj.transform.parent = m_ObstacleParent.transform;
                obstacleObj.transform.position = gridValue.Position;

                ObstacleBehavior obstacleBehavior = obstacleObj.GetComponent<ObstacleBehavior>();
                SetMember(gridValue.Coordinate.x, gridValue.Coordinate.y, obstacleBehavior);

                yield return new WaitForEndOfFrame();
            }

            m_IsObstacleCreated = true;
        }

        private IEnumerator GenerateCollectHero(int amount)
        {
            yield return new WaitUntil(() => m_IsObstacleCreated);

            for (int i = 0; i < amount; i++)
            {
                int randomHero = Random.Range(0, m_GameConfiguration.Heroes.Length - 1);
                CharacterData characterData = m_GameConfiguration.Heroes[randomHero];
                GridValue gridValue = GetRandomPosition();

                GameObject characterObj = Instantiate(characterData.Prefab);
                characterObj.transform.position = gridValue.Position;

                HeroesBehavior hero = characterObj.GetComponent<HeroesBehavior>();
                hero.IsCollected = false;
                hero.Order = -1;
                hero.CharacterGrownup.Self = hero;
                hero.CharacterGrownup.Data = m_GameConfiguration.GrownCoefficient;
                hero.CharacterGrownup.PreviousUpdateStatTime = m_GameConfiguration.GrownCoefficient.GrownEveryTime;
                hero.Spawned(characterData.Stat, m_GameConfiguration.MovementSpeed);

                SetMember(gridValue.Coordinate.x, gridValue.Coordinate.y, hero);

                yield return new WaitForEndOfFrame();
            }

            m_IsCollectHeroCreated = true;
        }

        private IEnumerator GenerateMonster()
        {
            yield return new WaitUntil(() => m_IsObstacleCreated);

            //for (int i = 0; i < m_GameConfiguration.StartMonsterSpawn; i++)
            //{

            //}
        }

        public IBehavior SpawnControlHero()
        {
            GridValue gridValue = GetRandomPosition();
            GameObject heroObj = Instantiate(m_GameConfiguration.StarterControlHero.Prefab);
            HeroesBehavior hero = heroObj.GetComponent<HeroesBehavior>();
            hero.CharacterGrownup.Self = hero;
            hero.CharacterGrownup.Data = m_GameConfiguration.GrownCoefficient;
            hero.CharacterGrownup.PreviousUpdateStatTime = m_GameConfiguration.GrownCoefficient.GrownEveryTime;
            hero.transform.position = gridValue.Position;
            hero.Spawned(m_GameConfiguration.StarterControlHero.Stat, m_GameConfiguration.MovementSpeed);

            SetMember(gridValue.Coordinate.x, gridValue.Coordinate.y, hero);

            return hero;
        }

        #endregion

        #region Spawn with channce

        public void SpawnCollectHero()
        {
            int random = Random.Range(1, 100);
            int amount = 0;

            List<ChanceSpawnData> datas = m_GameConfiguration.ChanceCollectHeroSpawn;
            datas = datas.OrderByDescending(x => x.Chance).ToList();

            foreach (ChanceSpawnData data in datas)
            {
                if(random <= data.Chance)
                {
                    amount = data.Amount;
                    break;
                }
            }

            StartCoroutine(GenerateCollectHero(amount));
        }

        #endregion

        #region Member

        public GridValue GetRandomPosition()
        {
            GridValue newGridValue = null;

            while (newGridValue == null)
            {
                int x = Random.Range(0, m_Config.SizeX - 1);
                int y = Random.Range(0, m_Config.SizeX - 1);

                GridValue gridValue = m_Grids[x, y];

                if (gridValue.HasMember == false)
                {
                    newGridValue = gridValue;
                }
            }

            return newGridValue;
        }

        public void SetMember(int x, int y, IBehavior newMember)
        {
            newMember.Coordinate = new Vector2Int(x, y);

            GridValue gridValue = m_Grids[x, y];
            gridValue.Member = newMember;
        }

        public IBehavior GetMember(int x, int y)
        {
            return m_Grids[x, y].Member;
        }

        public void RemoveMember(IBehavior remover)
        {
            for (int i = 0; i < m_Config.SizeX; i++)
            {
                for (int j = 0; j < m_Config.SizeY; j++)
                {
                    if (m_Grids[i, j].Member == remover)
                    {
                        m_Grids[i, j].Member.Coordinate = Vector2Int.one * -1;
                        m_Grids[i, j].Member = null;
                        break;
                    }
                }
            }
        }

        public void RemoveMember(int x, int y)
        {
            m_Grids[x, y].Member = null;
        }

        #endregion

        #region Path

        public Vector3 GetNextGrid(int x, int y, Vector2 direction, Vector2 previousDirection)
        {
            int collectedHeroes = PlayerController.Instance.Heroes.Count;

            if ((direction.x == 1 && (previousDirection.x != -1 || collectedHeroes == 1)) || (direction.x == -1 && (previousDirection.x != 1 || collectedHeroes == 1))) // move right or left
            {
                int newPosition = Mathf.Clamp(x + (int)direction.x, 0, m_Config.SizeX - 1);

                return UpdateGrid(x, y, newPosition, y);
            }
            else if ((direction.y == 1 && (previousDirection.y != -1 || collectedHeroes == 1)) || (direction.y == -1 && (previousDirection.y != 1 || collectedHeroes == 1))) // move up or down
            {
                int newPosition = Mathf.Clamp(y + (int)direction.y, 0, m_Config.SizeY - 1);

                return UpdateGrid(x, y, x, newPosition);
            }
            else // Not moving
            {
                return m_Grids[x, y].Position;
            }
        }

        public Vector3 UpdateGrid(int x, int y, int newPositionX, int newPositionY, IBehavior existMember = null)
        {
            IBehavior member = m_Grids[x, y].Member;

            if (existMember != null)
            {
                member = existMember;
            }
            else if (member != null)
            {
                m_Grids[x, y].Member = null;
            }

            SetMember(newPositionX, newPositionY, member);

            return m_Grids[newPositionX, newPositionY].Position;
        }

        #endregion
    }
}