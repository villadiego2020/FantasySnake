using FS.Cores;
using FS.Cores.Formulas;
using FS.Cores.MapGenerators;
using FS.UIs;
using System;
using UnityEngine;

namespace FS.Characters.Heroes
{
    public class HeroesBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Hero;

        [Header("Additional - UI")]
        [SerializeField] private UICollectableHero m_UICollectableHero;
        [SerializeField] private GameObject m_FrontBase;

        [Header("Other")]
        [SerializeField] protected LayerMask m_LayerMask;

        public bool IsCollected;
        public int Order; // Order = 1 is Front Line
        public Vector2 PreviousDirection;
        public Vector2Int PreviousCoordinate;
        public Vector3 TargetPosition;

        public Action<IBehavior> OnTriggerWithHeroEvent;
        public Action<IBehavior, IBehavior> OnTriggerWithObstacleEvent;

        private void Update()
        {
            if (IsCollected == false)
                return;

            Move();
        }

        public override void Spawned(params object[] objects)
        {
            Register();

            CharacterStat stat = objects[0] as CharacterStat;

            this.SetMinMaxStat(CharacterGrownup.Data.MinMaxHPStat.MinMaxStat, CharacterGrownup.Data.MinMaxAttackStat.MinMaxStat);
            this.ApplyStat(stat, true);

            m_Speed = (float)objects[1];

            PreviousCoordinate = Coordinate;
            TargetPosition = transform.position;
            SetFrontState();

            OnStatChangeEvent?.Invoke(Stat.MaxHP, Stat.HP, Stat.Attack);
        }

        public override void GetHit(IBehavior enemy)
        {
            base.GetHit(enemy);
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, m_Speed * Time.deltaTime);
        }

        public void Collect()
        {
            IsCollected = true;
            m_UICollectableHero.Close();
            m_UIStat.Open();

            SetFrontState();
        }

        public void Clone(int order, Vector2 previousDirection, Vector2Int previousCoordinate, Vector3 targetPosition)
        {
            Order = order;
            PreviousDirection = previousDirection;
            PreviousCoordinate = previousCoordinate;
            TargetPosition = targetPosition;

            SetFrontState();
        }

        public void SetFrontState()
        {
            m_FrontBase.SetActive(Order == 1);
        }

        public void SetFrontMove(Vector2 direction)
        {
            Vector2Int tmpCoordinate = Coordinate;
            Vector3 position = MapGenerator.Instance.GetNextGrid(Coordinate.x, Coordinate.y, direction, PreviousDirection);

            if (position != TargetPosition)
            {
                PreviousDirection = direction;
                PreviousCoordinate = tmpCoordinate;
            }
            else
            {
                PreviousCoordinate = Coordinate;
            }

            TargetPosition = position;
        }

        public void SetFollowerMove(Vector2Int coordinate)
        {
            PreviousCoordinate = Coordinate;
            TargetPosition = MapGenerator.Instance.UpdateGrid(Coordinate.x, Coordinate.y, coordinate.x, coordinate.y, this);
        }

        #region Trigger
        public void OnTriggerEnter(Collider other)
        {
            if ((m_LayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (Order == -1 || IsCollected == false)
                    return;

                IBehavior targetBehavior = other.transform.gameObject.GetComponent<IBehavior>();

                switch (targetBehavior.CharacterType)
                {
                    case CharacterType.Hero:
                        {
                            HeroesBehavior heroBehavior = (HeroesBehavior)targetBehavior;

                            if(Order == 1)
                            {
                                if (heroBehavior.IsCollected == true)
                                {
                                    OnDeadEvent?.Invoke(CharacterType.Hero);
                                }
                                else
                                {
                                    OnTriggerWithHeroEvent?.Invoke(targetBehavior);
                                }
                            }

                            break;
                        }
                    case CharacterType.Monster:
                        break;
                    case CharacterType.Obstacle:
                        {
                            OnTriggerWithObstacleEvent?.Invoke(this, targetBehavior);
                            break;
                        }
                }
            }
        }
        #endregion
    }
}