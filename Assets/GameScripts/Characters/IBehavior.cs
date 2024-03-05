using FS.Characters;
using FS.Cores.Formulas;
using FS.Cores.MapGenerators;
using System;
using UnityEngine;

namespace FS.Cores
{
    public abstract class IBehavior : MonoBehaviour, IInitalize
    {
        public CharacterStat Stat;
        public Vector2Int Coordinate;

        [SerializeField] protected float m_Speed = 5f;

        public virtual CharacterType CharacterType => CharacterType.None;
        public bool IsDead => Stat.HP == 0;
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

        }

        public virtual void Unregister()
        {

        }

        #endregion

        public abstract void Spawned(params object[] objects);
        public virtual void GetHit(IBehavior enemy)
        {
            DamageMessage damageMessage = this.TakeDamage(enemy);

            OnTakeDamageEvevnt?.Invoke(damageMessage);

            if (damageMessage.IsDead)
            {
                this.ResetStat();
                MapGenerator.Instance.RemoveMember(this);
                OnDeadEvent?.Invoke(CharacterType);
            }
        }
    }
}