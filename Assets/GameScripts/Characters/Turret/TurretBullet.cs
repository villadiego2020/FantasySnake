using FS.Characters.Heroes;
using FS.Cores;
using System;
using UnityEngine;

namespace FS.Characters.Turret
{
    public class TurretBullet : MonoBehaviour
    {
        [SerializeField] private LayerMask m_LayerMask;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_Radius = 2f;

        public float Radius => m_Radius;

        public Vector3 Target { get; set; }

        public Action<IBehavior> OnTargetHitEvent;

        private void Update()
        {
            if (Target == null)
                return;

            transform.position = Vector3.MoveTowards(transform.position, Target, m_Speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, Target) < 0.01f)
            {
                Boom();
            }
        }

        private void Boom()
        {
            Collider[] HitColliders = Physics.OverlapSphere(transform.position, m_Radius, m_LayerMask);

            if (HitColliders == null)
                return;

            foreach (var hit in HitColliders)
            {
                HeroesBehavior hero = hit.GetComponent<HeroesBehavior>();

                OnTargetHitEvent?.Invoke(hero);
            }

            Destroy(gameObject);
        }
    }
}