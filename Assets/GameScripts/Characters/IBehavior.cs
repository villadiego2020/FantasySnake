using FS.Characters;
using FS.Cores.Formulas;
using FS.Cores.MapGenerators;
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
        public bool IsDead => Stat.HP == 0;

        public Action<int, int, int> OnStatChangeEvent;
        public Action<int, int> OnHPChangeEvent;
        public Action<CharacterType> OnDeadEvent;
        public Action<DamageMessage> OnTakeDamageEvevnt;

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
            CharacterStat stat = new CharacterStat()
            {
                MaxHP = Mathf.FloorToInt(data.MinMaxHPStat.Value),
                HP = Mathf.FloorToInt(data.MinMaxHPStat.Value),
                Attack = Mathf.FloorToInt(data.MinMaxAttackStat.Value),
            };

            this.ApplyStat(stat);
            OnStatChangeEvent?.Invoke(Stat.MaxHP, Stat.HP, Stat.Attack);
        }

        #endregion

        public abstract void Spawned(params object[] objects);
        public virtual void GetHit(IBehavior enemy)
        {
            DamageMessage damageMessage = this.TakeDamage(enemy);

            OnHPChangeEvent?.Invoke(Stat.MaxHP, Stat.HP);
            OnTakeDamageEvevnt?.Invoke(damageMessage);

            if (damageMessage.IsDead)
            {
                Unregister();
                this.ResetStat();
                MapGenerator.Instance.RemoveMember(this);
                OnDeadEvent?.Invoke(CharacterType);
            }
        }
    }
}