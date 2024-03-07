using FS.Characters;
using FS.Characters.Heroes;
using FS.Cores;
using FS.Cores.Generators;
using FS.Cores.Players;
using FS.Statistics;
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
            Register();

            IsDone = true;
        }

        public override void Deinitialize()
        {

        }

        public override void Register()
        {
            GameManager.instance.OnGameStateUpdateEvent += GameStateChanged;

            PlayerInputAction.Instance.OnMoveEvent += Move;
            PlayerInputAction.Instance.OnSwitchHeroEvent += SwitchHero;
            PlayerInputAction.Instance.OnSwitchRotateHeroEvent += SwitchRotateHero;
        }

        public override void Unregister()
        {
            GameManager.instance.OnGameStateUpdateEvent -= GameStateChanged;

            PlayerInputAction.Instance.OnMoveEvent -= Move;
            PlayerInputAction.Instance.OnSwitchHeroEvent -= SwitchHero;
            PlayerInputAction.Instance.OnSwitchRotateHeroEvent -= SwitchRotateHero;
        }

        #region Event

        private void GameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Prepare:
                    break;
                case GameState.Start:
                    {
                        SpawnControlHero();
                        break;
                    }
                case GameState.Pause:
                    break;
                case GameState.End:
                    {
                        Unregister();
                        m_UIResult.Open();
                        break;
                    }
            }
        }

        private void Move(Vector2 direction)
        {
            if (Heroes.Count == 0 || direction == Vector2.zero)
                return;

            for (int i = 0; i < Heroes.Count; i++)
            {
                HeroesBehavior hero = Heroes[i];

                if (i == 0) // Front Move
                {
                    hero.SetFrontMove(direction);
                }
                else // Move Follow Previous Hero
                {
                    HeroesBehavior previousHero = Heroes[i - 1]; // Previous Hero
                    Vector2Int previousCoordinate = previousHero.PreviousCoordinate;
                    Vector2Int currentCoordinate = previousHero.Coordinate;

                    if (previousCoordinate == currentCoordinate)
                        break;

                    hero.SetFollowerMove(previousCoordinate);
                }
            }
        }

        private void SwitchHero()
        {
            if(Heroes.Count <= 1) return;

            HeroesBehavior lastHero = Heroes[Heroes.Count - 1]; // Last Hero

            for (int i = Heroes.Count - 2; i >= 0; i--) // Switch by start from upper last hero
            {
                HeroesBehavior tmpUpperHero = Heroes[i];

                Vector2Int tmpLastHeroCoordinate = lastHero.Coordinate;
                Vector2Int tmpUpperHeroCoordinate = tmpUpperHero.Coordinate;

                int tmpUpperOrder = tmpUpperHero.Order;
                int tmpLastOrder = lastHero.Order;

                Vector2 tmpUpperPreviousDirection = tmpUpperHero.PreviousDirection;
                Vector2 tmpLastPreviousDirection = lastHero.PreviousDirection;

                Vector2Int tmpUpperPreviousCoordinate = tmpUpperHero.PreviousCoordinate;
                Vector2Int tmpLastPreviousCoordinate = lastHero.PreviousCoordinate;

                Vector3 tmpUpperTargetPosition = tmpUpperHero.TargetPosition;
                Vector3 tmpLastTargetPosition = lastHero.TargetPosition;

                Heroes[i] = lastHero;
                Heroes[i + 1] = tmpUpperHero;

                lastHero.Clone(tmpUpperOrder, tmpUpperPreviousDirection, tmpUpperPreviousCoordinate, tmpUpperTargetPosition);
                tmpUpperHero.Clone(tmpLastOrder, tmpLastPreviousDirection, tmpLastPreviousCoordinate, tmpLastTargetPosition);

                Vector3 newUpperPosition = Generator.Instance.UpdateGrid(tmpUpperHeroCoordinate.x, tmpUpperHeroCoordinate.y, tmpLastHeroCoordinate.x, tmpLastHeroCoordinate.y, tmpUpperHero);
                Vector3 newLastPosition = Generator.Instance.UpdateGrid(tmpLastHeroCoordinate.x, tmpLastHeroCoordinate.y, tmpUpperHeroCoordinate.x, tmpUpperHeroCoordinate.y, lastHero);

                tmpUpperHero.transform.position = newUpperPosition;
                lastHero.transform.position = newLastPosition;
            }
        }

        private void SwitchRotateHero()
        {
            for (int i = 0; i < Heroes.Count-1; i++) // Switch by start from upper last hero
            {
                HeroesBehavior hero = Heroes[i];
                HeroesBehavior nextHero = Heroes[i+1];

                Vector2Int tmpUpperHeroCoordinate = hero.Coordinate;
                Vector2Int tmpLastHeroCoordinate = nextHero.Coordinate;

                int tmpUpperOrder = hero.Order;
                int tmpLastOrder = nextHero.Order;

                Vector2 tmpUpperPreviousDirection = hero.PreviousDirection;
                Vector2 tmpLastPreviousDirection = nextHero.PreviousDirection;

                Vector2Int tmpUpperPreviousCoordinate = hero.PreviousCoordinate;
                Vector2Int tmpLastPreviousCoordinate = nextHero.PreviousCoordinate;

                Vector3 tmpUpperTargetPosition = hero.TargetPosition;
                Vector3 tmpLastTargetPosition = nextHero.TargetPosition;

                Heroes[i] = nextHero;
                Heroes[i+1] = hero;

                nextHero.Clone(tmpUpperOrder, tmpUpperPreviousDirection, tmpUpperPreviousCoordinate, tmpUpperTargetPosition);
                hero.Clone(tmpLastOrder, tmpLastPreviousDirection, tmpLastPreviousCoordinate, tmpLastTargetPosition);

                Vector3 newUpperPosition = Generator.Instance.UpdateGrid(tmpUpperHeroCoordinate.x, tmpUpperHeroCoordinate.y, tmpLastHeroCoordinate.x, tmpLastHeroCoordinate.y, hero);
                Vector3 newLastPosition = Generator.Instance.UpdateGrid(tmpLastHeroCoordinate.x, tmpLastHeroCoordinate.y, tmpUpperHeroCoordinate.x, tmpUpperHeroCoordinate.y, nextHero);

                hero.transform.position = newUpperPosition;
                nextHero.transform.position = newLastPosition;
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

                hero.SetFrontState();
            }
        }

        #endregion

        #endregion

        private void SpawnControlHero()
        {
            IBehavior newHero = Generator.Instance.SpawnControlHero();

            SetHeroData(newHero);
        }

        private void SetHeroData(IBehavior newHero)
        {
            HeroesBehavior hero = newHero as HeroesBehavior;
            hero.OnDeadEvent = Dead;
            hero.OnTriggerWithHeroEvent = TriggerWithHero;
            hero.OnTriggerWithObstacleEvent = TriggerWithObstacle;
            hero.Order = Heroes.Count + 1;
            hero.Collect();

            Heroes.Add(hero);
        }

        #region Callback

        public void Dead(CharacterType type, IBehavior dead)
        {
            if (type == CharacterType.Hero)
            {
                HeroesBehavior heroBehavior = dead as HeroesBehavior;
                Heroes.Remove(heroBehavior);
                Generator.Instance.RemoveMember(dead);
                Destroy(dead.gameObject);

                CheckResult();
            }
            else if(type == CharacterType.Monster)
            {
                GameStatistic.IncreaseMonsterEliminated();
                Generator.Instance.RemoveMember(dead);
                Destroy(dead.gameObject);
                Generator.Instance.SpawnMonster();
            }
        }

        private void TriggerWithHero(IBehavior newHero)
        {
            HeroesBehavior hero = newHero as HeroesBehavior;

            if (hero.IsCollected == true)
            {
                GameManager.instance.SetState(GameState.End);
            }
            else
            {
                HeroesBehavior lastHero = Heroes[Heroes.Count - 1];
                Vector3 positon = Generator.Instance.UpdateGrid(newHero.Coordinate.x, newHero.Coordinate.y, lastHero.PreviousCoordinate.x, lastHero.PreviousCoordinate.y, newHero);
                hero.TargetPosition = positon;
                hero.transform.position = positon;
                SetHeroData(newHero);
                Generator.Instance.SpawnCollectHero();
            }
        }

        private void TriggerWithObstacle(IBehavior hero, IBehavior obstacle)
        {
            HeroesBehavior heroBehavior = hero as HeroesBehavior;
            Heroes.Remove(heroBehavior);

            Generator.Instance.RemoveMember(heroBehavior);

            Destroy(hero.gameObject);

            CheckResult();
        }

        #endregion

        private void CheckResult()
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
    }
}