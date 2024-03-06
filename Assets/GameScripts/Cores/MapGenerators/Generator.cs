using FS.Asset.Players;
using FS.Characters.Heroes;
using FS.Characters.Monsters;
using FS.Characters.Obstacles;
using FS.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FS.Cores.MapGenerators
{
    [DefaultExecutionOrder(ScriptOrders.MAP_GENERATOR_ORDER)]
    public class Generator : IJobState
    {
        public static Generator Instance;

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
        private bool m_IsMonsterCreated;

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
            StartCoroutine(GenerateCollectHero(m_GameConfiguration.Hero.StartAmount));
            StartCoroutine(GenerateMonster(m_GameConfiguration.Monster.StartAmount));
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

        /// <summary>
        /// Wait for all generator completed
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitializeState()
        {
            yield return new WaitUntil(() => m_IsGridCreated && m_IsObstacleCreated && m_IsCollectHeroCreated && m_IsMonsterCreated);

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

        /// <summary>
        /// Create an obstacle
        /// </summary>
        /// <returns></returns>
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
                AddMember(gridValue.Coordinate.x, gridValue.Coordinate.y, obstacleBehavior);

                yield return new WaitForEndOfFrame();
            }

            m_IsObstacleCreated = true;
        }

        /// <summary>
        /// Create Collect Hero by amount use from Starter or from remove
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private IEnumerator GenerateCollectHero(int amount)
        {
            yield return new WaitUntil(() => m_IsObstacleCreated);

            for (int i = 0; i < amount; i++)
            {
                int random = Random.Range(0, m_GameConfiguration.Hero.Data.Length - 1);
                CharacterData characterData = m_GameConfiguration.Hero.Data[random];

                GameObject characterObj = Instantiate(characterData.Prefab);
                HeroesBehavior hero = CreateHero(characterObj);
                hero.Order = -1;
                hero.IsCollected = false;

                yield return new WaitForEndOfFrame();
            }

            m_IsCollectHeroCreated = true;
        }

        /// <summary>
        /// Create Monster by amount use from Starter or from remove
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private IEnumerator GenerateMonster(int amount)
        {
            yield return new WaitUntil(() => m_IsCollectHeroCreated);

            for (int i = 0; i < amount; i++)
            {
                int random = Random.Range(0, m_GameConfiguration.Monster.Data.Length - 1);
                CharacterData characterData = m_GameConfiguration.Monster.Data[random];
                GridValue gridValue = GetRandomPosition();

                GameObject characterObj = Instantiate(characterData.Prefab);
                characterObj.transform.position = gridValue.Position;

                MonsterBehavior monster = characterObj.GetComponent<MonsterBehavior>();
                monster.CharacterGrownup.Self = monster;
                monster.CharacterGrownup.Data = m_GameConfiguration.Monster.GrownCoefficient;
                monster.CharacterGrownup.PreviousUpdateStatTime = m_GameConfiguration.Monster.GrownCoefficient.GrownEveryTime;
                monster.Spawned(characterData.Stat, m_GameConfiguration.Monster.Movmenet);

                monster.OnDeadEvent = PlayerController.Instance.Dead;

                AddMember(gridValue.Coordinate.x, gridValue.Coordinate.y, monster);

                yield return new WaitForEndOfFrame();
            }

            m_IsMonsterCreated = true;
        }

        /// <summary>
        /// Create Control hero
        /// </summary>
        /// <returns></returns>
        public IBehavior SpawnControlHero()
        {
            GameObject heroObj = Instantiate(m_GameConfiguration.StarterControlHero.Prefab);
            HeroesBehavior hero = CreateHero(heroObj);
            hero.IsCollected = true;

            return hero;
        }

        private HeroesBehavior CreateHero(GameObject heroObj)
        {
            GridValue gridValue = GetRandomPosition();
            HeroesBehavior hero = heroObj.GetComponent<HeroesBehavior>();
            hero.CharacterGrownup.Self = hero;
            hero.CharacterGrownup.Data = m_GameConfiguration.Hero.GrownCoefficient;
            hero.CharacterGrownup.PreviousUpdateStatTime = m_GameConfiguration.Hero.GrownCoefficient.GrownEveryTime;
            hero.transform.position = gridValue.Position;
            hero.Spawned(m_GameConfiguration.StarterControlHero.Stat, m_GameConfiguration.Hero.Movmenet);

            AddMember(gridValue.Coordinate.x, gridValue.Coordinate.y, hero);

            return hero;
        }

        #endregion

        #region Spawn with channce

        /// <summary>
        /// Create Collect Hero after previous hero was collected
        /// </summary>
        public void SpawnCollectHero()
        {
            int random = Random.Range(1, 100);
            int amount = 0;

            List<ChanceSpawnData> datas = m_GameConfiguration.Hero.ChanceSpawn;
            datas = datas.OrderBy(x => x.Chance).ToList();

            foreach (ChanceSpawnData data in datas)
            {
                if(random <= data.Chance)
                {
                    amount = data.Amount;
                    break;
                }
            }

            Debug.Log(amount);
            StartCoroutine(GenerateCollectHero(amount));
        }

        /// <summary>
        /// Create Monster after previous monster was defeated
        /// </summary>
        public void SpawnMonster()
        {
            int random = Random.Range(1, 100);
            int amount = 0;

            List<ChanceSpawnData> datas = m_GameConfiguration.Monster.ChanceSpawn;
            datas = datas.OrderBy(x => x.Chance).ToList();

            foreach (ChanceSpawnData data in datas)
            {
                if (random <= data.Chance)
                {
                    amount = data.Amount;
                    break;
                }
            }

            StartCoroutine(GenerateMonster(amount));
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

        public void AddMember(int x, int y, IBehavior newMember)
        {
            newMember.Coordinate = new Vector2Int(x, y);

            GridValue gridValue = m_Grids[x, y];
            gridValue.Members.Add(newMember);
        }

        public void RemoveMember(IBehavior remover)
        {
            for (int i = 0; i < m_Config.SizeX; i++)
            {
                for (int j = 0; j < m_Config.SizeY; j++)
                {
                    int indexOf = m_Grids[i, j].Members.IndexOf(remover);

                    if (indexOf > -1)
                    {
                        m_Grids[i, j].Members[indexOf].Coordinate = Vector2Int.one * -1;
                        m_Grids[i, j].Members.RemoveAt(indexOf);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Path

        public Vector3 GetNextGrid(IBehavior behavior, int x, int y, Vector2 direction, Vector2 previousDirection)
        {
            int collectedHeroes = PlayerController.Instance.Heroes.Count;

            if ((direction.x == 1 && (previousDirection.x != -1 || collectedHeroes == 1)) || (direction.x == -1 && (previousDirection.x != 1 || collectedHeroes == 1))) // move right or left
            {
                int newPosition = Mathf.Clamp(x + (int)direction.x, 0, m_Config.SizeX - 1);

                return UpdateGrid(x, y, newPosition, y, behavior);
            }
            else if ((direction.y == 1 && (previousDirection.y != -1 || collectedHeroes == 1)) || (direction.y == -1 && (previousDirection.y != 1 || collectedHeroes == 1))) // move up or down
            {
                int newPosition = Mathf.Clamp(y + (int)direction.y, 0, m_Config.SizeY - 1);

                return UpdateGrid(x, y, x, newPosition, behavior);
            }
            else // Not moving
            {
                return m_Grids[x, y].Position;
            }
        }

        public Vector3 UpdateGrid(int x, int y, int newPositionX, int newPositionY, IBehavior behavior)
        {
            RemoveMember(behavior);
            AddMember(newPositionX, newPositionY, behavior);

            return m_Grids[newPositionX, newPositionY].Position;
        }

        #endregion
    }
}