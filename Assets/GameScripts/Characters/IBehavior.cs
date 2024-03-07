using FS.Characters;
using FS.Cores.Formulas;
using FS.Cores.Generators;
using FS.Datas;
using FS.UIs;
using System;
using UnityEngine;

namespace FS.Cores
{
    public abstract class IBehavior : MonoBehaviour, IInitalize
    {
        public CharacterStat Stat;
        public Vector2Int Coordinate;
        public CharacterGrownup CharacterGrownup;

        [Header("UI")]
        [SerializeField] protected UIStat m_UIStat;

        protected float m_Speed = 5f;

        public virtual CharacterType CharacterType => CharacterType.None;
        public CharacterState CharacterState { get; protected set; } = CharacterState.Idle;
        public bool IsDead => Stat.HP == 0;

        public Action<int, int, int> OnStatChangeEvent;
        public Action<int, int> OnHPChangeEvent;
        public Action<CharacterType, IBehavior> OnDeadEvent;

        #region IInitalize

        public virtual void Initialize(params object[] objects)
        {
            Spawned(objects);
        }

        public virtual void Deinitialize()
        {

        }

        public virtual void Register()
        {
            OnStatChangeEvent += m_UIStat.AdjustStat;
            OnHPChangeEvent += m_UIStat.AdjustHP;

            CharacterGrownup.OnCharacterGrownEvent += CharacterReadyToGrown;
        }

        public virtual void Unregister()
        {
            OnStatChangeEvent -= m_UIStat.AdjustStat;
            OnHPChangeEvent -= m_UIStat.AdjustHP;

            CharacterGrownup.OnCharacterGrownEvent -= CharacterReadyToGrown;
        }

        protected void CharacterReadyToGrown(GrownUpData data)
        {
            int increaseMaxHP = UnityEngine.Random.Range(data.MinMaxHPStat.MinMaxStat.x, data.MinMaxHPStat.MinMaxStat.y);
            int increaseHP = UnityEngine.Random.Range(data.MinMaxHPStat.MinMaxStat.x, data.MinMaxHPStat.MinMaxStat.y) / 2;
            int increaseAttackHP = UnityEngine.Random.Range(data.MinMaxAttackStat.MinMaxStat.x, data.MinMaxAttackStat.MinMaxStat.y);

            CharacterStat stat = new CharacterStat()
            {
                MaxHP = increaseMaxHP,
                HP = increaseHP,
                Attack = increaseMaxHP,
            };

            this.ApplyStat(stat);
            OnStatChangeEvent?.Invoke(Stat.MaxHP, Stat.HP, Stat.Attack);
        }

        #endregion

        public abstract void Spawned(params object[] objects);
        public virtual void GetHit(IBehavior enemy)
        {
            DamageMessage damageMessage = this.TakeDamage(enemy);

            Spawner.Instance.CreateDamagePopup(damageMessage, gameObject.transform.position);
            AudioManager.Instance.Hit();

            OnHPChangeEvent?.Invoke(Stat.MaxHP, Stat.HP);

            if (damageMessage.IsDead)
            {
                Unregister();
                this.ResetStat();
                Generator.Instance.RemoveMember(this);
                OnDeadEvent?.Invoke(CharacterType, this);
            }
        }
    }
}