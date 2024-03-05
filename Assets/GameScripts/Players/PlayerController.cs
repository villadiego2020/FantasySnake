using FS.Characters;
using FS.Characters.Heroes;
using FS.Characters.Obstacles;
using FS.Cores;
using FS.Cores.MapGenerators;
using FS.Cores.Players;
using FS.Datas;
using FS.UIs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FS.Asset.Players
{
    [DefaultExecutionOrder(ScriptOrders.PLAYER_CONTROLLER_ORDER)]
    public class PlayerController : IJobState
    {
        public static PlayerController Instance { get; private set; }

        [Header("Hero")]
        [SerializeField] private CharacterData m_CharacterData;

        [Space(5)]
        [Header("Data")]
        public List<HeroesBehavior> Heroes;

        [Space(5)]
        [Header("UI")]
        [SerializeField] private UIResullt m_UIResult;

        private void Awake()
        {
            Instance = this;
        }

        #region IJobState
        public override void Initialize(params object[] objects)
        {
            SpawnControlHero();
            Register();

            IsDone = true;
        }

        public override void Deinitialize()
        {

        }

        public override void Register()
        {
            PlayerInputAction.Instance.OnMoveEvent += Move;
            GameManager.instance.OnGameStateUpdateEvent += GameStateChanged;
        }

        public override void Unregister()
        {
            PlayerInputAction.Instance.OnMoveEvent -= Move;
            GameManager.instance.OnGameStateUpdateEvent -= GameStateChanged;
        }

        #region Event
        private void Move(Vector2 direction)
        {
            if (Heroes.Count == 0)
                return;

            for (int i = 0; i < Heroes.Count; i++)
            {
                if(i == 0)
                {
                    Heroes[i].SetLeaderMove(direction);
                }
                else
                {
                    Vector2Int preCoordinate = Heroes[i - 1].PreviousCoordinate;
                    Vector2Int currentCoordinate = Heroes[i - 1].Coordinate;

                    if (preCoordinate == currentCoordinate)
                        break;

                    Heroes[i].SetFollowMove(direction, preCoordinate);
                }
            }
        }

        private void GameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.None:
                    break;
                case GameState.Prepare:
                    break;
                case GameState.Start:
                    break;
                case GameState.Pause:
                    break;
                case GameState.End:
                    Unregister();
                    m_UIResult.Open();
                    break;
            }
        }

        #endregion

        #endregion

        private void SpawnControlHero()
        {
            GameObject heroObj = Instantiate(m_CharacterData.Prefab);
            HeroesBehavior hero = heroObj.GetComponent<HeroesBehavior>();

            GridValue gridValue = MapGenerator.Instance.GetRandomPosition();
            MapGenerator.Instance.SetMember(gridValue.Coordinate.x, gridValue.Coordinate.y, hero);

            hero.transform.position = gridValue.Position;
            hero.Spawned(m_CharacterData.Stat);

            TriggerWithHero(hero);
        }

        #region Callback

        private void Dead(CharacterType type)
        {
            if (type == CharacterType.Hero)
            {
                GameManager.instance.SetState(GameState.End);
            }
        }

        private void TriggerWithHero(IBehavior newHero)
        {
            HeroesBehavior hero = newHero as HeroesBehavior;
            hero.OnDeadEvent = Dead;
            hero.OnTriggerWithHeroEvent = TriggerWithHero;
            hero.OnTriggerWithObstacleEvent = TriggerWithObstacle;
            hero.Order = Heroes.Count + 1;
            hero.IsCollected = true;

            Heroes.Add(hero);
        }

        private void TriggerWithObstacle(IBehavior hero, IBehavior obstacle)
        {
            HeroesBehavior heroBehavior = hero as HeroesBehavior;
            Heroes.Remove(heroBehavior);

            Destroy(hero.gameObject);

            CheckGameResult();
        }

        #endregion

        private void CheckGameResult()
        {
            if(Heroes.Count == 0)
            {
                GameManager.instance.SetState(GameState.End);
            }
            else
            {
                SortHeros();
            }
        }

        private void SortHeros()
        {
            int order = 1;

            foreach (var hero in Heroes)
            {
                hero.Order = order++;
                hero.OnDeadEvent = Dead;
                hero.OnTriggerWithHeroEvent = TriggerWithHero;
                hero.OnTriggerWithObstacleEvent = TriggerWithObstacle;
            }
        }
    }
}