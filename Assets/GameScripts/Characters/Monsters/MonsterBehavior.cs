using FS.Cores;
using FS.Cores.Formulas;
using System.Collections.Generic;
using UnityEngine;

namespace FS.Characters.Monsters
{
    public class MonsterBehavior : IBehavior
    {
        public override CharacterType CharacterType => CharacterType.Monster;

        [Header("Other")]
        [SerializeField] protected LayerMask m_LayerMask;

        [SerializeField] private List<IBehavior> m_Targets;
        private float m_AttackTime;
        private bool m_IsAttacking;

        private void Update()
        {
            if (IsDead) return;

            m_IsAttacking = m_Targets.Count > 0;

            Attack();
        }

        public override void Spawned(object[] objects)
        {
            Register();

            CharacterStat stat = objects[0] as CharacterStat;

            this.ApplyStat(stat, true);

            m_Speed = (float)objects[1];
            m_Targets = new List<IBehavior>();
            m_AttackTime = Stat.AttackRate;
            m_IsAttacking = false;

            OnStatChangeEvent?.Invoke(Stat.MaxHP, Stat.HP, Stat.Attack);
        }

        private void Attack()
        {
            if (m_Targets.Count == 0 || m_IsAttacking == false) return;

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

        public void AddTarget(IBehavior target)
        {
            m_Targets.Add(target);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((m_LayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                IBehavior behavior = other.transform.gameObject.GetComponent<IBehavior>();

                switch (behavior.CharacterType)
                {
                    case CharacterType.Hero:
                        {
                            m_Targets.Remove(behavior);

                            if (m_Targets.Count == 0)
                            {
                                m_AttackTime = Stat.AttackRate;
                            }

                            break;
                        }
                }
            }
        }
    }
}