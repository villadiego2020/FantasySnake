using FS.Asset.Players;
using FS.Characters.Heroes;
using FS.Characters.Turret;
using FS.Cores;
using FS.Cores.Formulas;
using UnityEngine;

namespace FS.Characters.Turrets
{
    public class Turret : IBehavior
    {
        [SerializeField] private float m_AttackTime;
        [SerializeField] private float m_AttackRate;

        private GameObject m_BulletArea;

        public override void Spawned(params object[] objects)
        {
            Register();

            CharacterStat stat = objects[0] as CharacterStat;
            m_AttackRate = Random.Range(0.1f, stat.AttackRate);

            this.ApplyStat(stat, true);
        }

        private void Update()
        {
            if (GameManager.instance.GameState != GameState.CrazyTime)
                return;

            transform.LookAt(PlayerController.Instance.Heroes[0].transform);

            Attack();
        }

        private void Attack()
        {
            m_AttackTime += Time.deltaTime;

            if (m_AttackTime >= m_AttackRate)
            {
                m_AttackRate = Random.Range(0.1f, Stat.AttackRate);
                m_AttackTime = 0;

                Vector3 position = PlayerController.Instance.Heroes[0].transform.position;
                TurretBullet bullet = Spawner.Instance.CreateTurretBullet(position, transform.position);
                bullet.OnTargetHitEvent = FoundTarget;

                m_BulletArea = Spawner.Instance.CreateTurretBulletArea(position);
                m_BulletArea.transform.localScale = Vector3.one * bullet.Radius;
                Destroy(m_BulletArea, 2f);
            }
        }

        private void FoundTarget(IBehavior hero)
        {
            if(hero.CharacterType == CharacterType.Hero)
            {
                HeroesBehavior heroBehavior = hero as HeroesBehavior;

                if (heroBehavior.IsCollected == false)
                    return;
            }

            hero.GetHit(this);
        }
    }
}