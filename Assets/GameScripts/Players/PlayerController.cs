using FS.Characters.Heroes;
using FS.Cores;
using FS.Cores.MapGenerators;
using System.Collections.Generic;
using UnityEngine;

namespace FS.Asset.Players
{
    [DefaultExecutionOrder(ScriptOrders.PLAYER_CONTROLLER_ORDER)]
    public class PlayerController : IJobState
    {
        public static PlayerController Instance { get; private set; }

        [Header("Prefab")]
        [SerializeField] private GameObject m_HeroPrefab;

        [Space(5)]
        [Header("Data")]
        public List<IBehavior> Heroes;

        private void Awake()
        {
            Instance = this;
        }

        public void CollectHero(IBehavior newHero)
        {
            HeroesBehavior hero = newHero as HeroesBehavior;
            hero.Order = Heroes.Count + 1;

            Heroes.Add(hero);
        }

        #region IJobState
        public override void Initialize(params object[] objects)
        {
            SpawnHero();

            IsDone = true;
        }

        public override void Deinitialize()
        {

        }

        public override void Register()
        {

        }

        public override void Unregister()
        {

        }

        #endregion

        private void SpawnHero()
        {
            GameObject heroObj = Instantiate(m_HeroPrefab);
            HeroesBehavior hero = heroObj.GetComponent<HeroesBehavior>();

            MapGenerator.Instance.SetMember(0, 0, hero);
            CollectHero(hero);
        }
    }
}