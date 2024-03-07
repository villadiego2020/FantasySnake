using FS.Characters.Monsters;
using FS.Cores;
using FS.Cores.Formulas;
using FS.Cores.Generators;
using FS.UIs;
using System;
using System.Collections.Generic;
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
        [SerializeField] private Collider m_Collider;

        public bool IsCollected;
        public int Order; // Order = 1 is Front Line
        public Vector2 PreviousDirection;
        public Vector2Int PreviousCoordinate;
        public Vector3 TargetPosition;
        private float m_AttackTime;
        public bool IsFirstMove;

        public Action<IBehavior> OnTriggerWithHeroEvent;
        public Action<IBehavior, IBehavior> OnTriggerWithObstacleEvent;

        private bool m_IsAttacking;
        [SerializeField] private List<IBehavior> m_Targets;

        private void Update()
        {
            if (IsCollected == false || IsDead)
                return;

            m_IsAttacking = m_Targets.Count > 0;

            Move();
            Attack();
        }

        public override void Spawned(params object[] objects)
        {
            Register();

            CharacterStat stat = objects[0] as CharacterStat;

            this.ApplyStat(stat, true);

            m_Speed = (float)objects[1];
            m_Targets = new List<IBehavior>();
            m_AttackTime = 0;
            m_IsAttacking = false;
            IsFirstMove = true;

            PreviousCoordinate = Coordinate;
            TargetPosition = transform.position;
            SetFrontState();

            OnStatChangeEvent?.Invoke(Stat.MaxHP, Stat.HP, Stat.Attack);
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, m_Speed * Time.deltaTime);
        }

        private void Attack()
        {
            if(m_Targets.Count == 0 || m_IsAttacking == false) return;

            m_AttackTime += Time.deltaTime;

            if (m_AttackTime >= Stat.AttackRate)
            {
                m_AttackTime = 0;

                foreach (var target in m_Targets)
                {
                    if (target == null || target.IsDead) continue;

                    target.GetHit(this);
                }
            }
        }

        public void Collect()
        {
            IsCollected = true;
            m_UICollectableHero.Close();
            m_UIStat.Open();

            SetFrontState();
        }

        public void ActiveCollider(bool isActive)
        {
            m_Collider.enabled = isActive;
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
            Vector3 position = Generator.Instance.GetNextGrid(this, Coordinate.x, Coordinate.y, direction, PreviousDirection);

            if (position != TargetPosition)
            {
                AudioManager.Instance.Move();
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
            TargetPosition = Generator.Instance.UpdateGrid(Coordinate.x, Coordinate.y, coordinate.x, coordinate.y, this);
        }

        #region Trigger
        public void OnTriggerEnter(Collider other)
        {
            if ((m_LayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (Order == -1 || IsCollected == false)
                    return;

                IBehavior behavior = other.transform.gameObject.GetComponent<IBehavior>();

                switch (behavior.CharacterType)
                {
                    case CharacterType.Hero:
                        {
                            HeroesBehavior targetBehavior = (HeroesBehavior)behavior;

                            if(Order == 1)
                            {
                                OnTriggerWithHeroEvent?.Invoke(behavior);
                            }

                            break;
                        }
                    case CharacterType.Monster:
                        {
                            MonsterBehavior targetBehavior = (MonsterBehavior)behavior;

                            targetBehavior.AddTarget(this);
                            AddTarget(targetBehavior);

                            break;
                        }
                    case CharacterType.Obstacle:
                        {
                            OnTriggerWithObstacleEvent?.Invoke(this, behavior);
                            break;
                        }
                }
            }
        }

        public void AddTarget(IBehavior target)
        {
            m_Targets.Add(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((m_LayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (Order == -1 || IsCollected == false)
                    return;

                IBehavior behavior = other.transform.gameObject.GetComponent<IBehavior>();

                switch (behavior.CharacterType)
                {
                    case CharacterType.Monster:
                        {
                            m_Targets.Remove(behavior);

                            if(m_Targets.Count == 0)
                            {
                                m_AttackTime = Stat.AttackRate;
                            }

                            break;
                        }
                }
            }
        }

        #endregion
    }
}