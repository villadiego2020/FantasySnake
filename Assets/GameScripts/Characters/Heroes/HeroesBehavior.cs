using FS.Cores;
using FS.Cores.Formulas;
using FS.Cores.MapGenerators;
using System;
using UnityEngine;

namespace FS.Characters.Heroes
{
    public class HeroesBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Hero;

        [SerializeField] protected LayerMask m_LayerMask;
        public int Order;
        public bool IsCollected;
        public Vector2 PreviousDirection;
        public Vector2Int PreviousCoordinate;
        private Vector3 m_TargetPosition;

        public Action<IBehavior> OnTriggerWithHeroEvent;
        public Action<IBehavior, IBehavior> OnTriggerWithObstacleEvent;

        private void Update()
        {
            if (IsCollected == false)
                return;

            Move();
        }

        private void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, m_Speed * Time.deltaTime);
        }

        public void SetLeaderMove(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            Vector2Int tmpCoordinate = Coordinate;
            Vector3 position = MapGenerator.Instance.GetNextGrid(Coordinate.x, Coordinate.y, direction, PreviousDirection);

            if (position != m_TargetPosition)
            {
                PreviousDirection = direction;
                PreviousCoordinate = tmpCoordinate;
            }
            else
            {
                PreviousCoordinate = Coordinate;
            }

            m_TargetPosition = position;
        }

        public void SetFollowMove(Vector2 direction, Vector2Int coordinate)
        {
            if (direction == Vector2.zero)
                return;

            PreviousCoordinate = Coordinate;
            m_TargetPosition = MapGenerator.Instance.UpdateGrid(Coordinate.x, Coordinate.y, coordinate.x, coordinate.y, this);
        }

        public override void Spawned(params object[] objects)
        {
            CharacterStat stat = objects[0] as CharacterStat;
            this.ApplyStat(stat);

            PreviousCoordinate = Coordinate;
            m_TargetPosition = transform.position;
        }

        public override void GetHit(IBehavior enemy)
        {
            base.GetHit(enemy);
        }

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
    }
}