using FS.Datas;
using System.Collections;
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

        private GridValue[,] m_Grids;

        private void Awake()
        {
            Instance = this;
        }

        #region IJobState

        public override void Initialize(params object[] objects)
        {
            StartCoroutine(nameof(GenerateGrid));

            IsDone = true;
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
        /// Generate Grid by Map Configuration
        /// </summary>
        /// <returns></returns>
        private IEnumerator GenerateGrid()
        {
            m_Grids = new GridValue[m_Config.SizeX, m_Config.SizeY];

            for (int i = 0; i < m_Config.SizeX; i++)
            {
                for(int j = 0; j < m_Config.SizeY; j++)
                {
                    Vector2 coordinate = new Vector2
                    {
                        x = i,
                        y = j
                    };

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

            yield return new WaitForEndOfFrame();
        }

        private void GenerateHero()
        {

        }

        private void GenerateMonster()
        {

        }

        private void GenerateObstacle()
        {

        }

        #endregion

        #region Member

        public void SetMember(int x, int y, IBehavior newMember)
        {
            m_Grids[x, y].Member = newMember;
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
    }
}