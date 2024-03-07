using FS.Characters.Turret;
using FS.Cores.Formulas;
using FS.Datas;
using UnityEngine;

namespace FS.Cores
{
    [DefaultExecutionOrder(ScriptOrders.SPAWNER_ORDER)]
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private PrefabConfiguration m_Config;

        public static Spawner Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public DamagePopup CreateDamagePopup(DamageMessage message, Vector3 position)
        {
            DamagePopup instance = Instantiate(m_Config.DamagePopupPrefab, position, Quaternion.identity);
            instance.Setup(message);

            return instance;
        }

        public TurretBullet CreateTurretBullet(Vector3 target, Vector3 position)
        {
            TurretBullet instance = Instantiate(m_Config.TurretBulletPrefab, position, Quaternion.identity);
            instance.Target = target;

            return instance;
        }

        public GameObject CreateTurretBulletArea(Vector3 position, Transform parent = null)
        {
            GameObject obj = Instantiate(m_Config.TurretBulletAreaPrefab);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.localScale = Vector3.one;

            return obj;
        }

        public GameObject Create(GameObject prefab, Vector3 position, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.localScale = Vector3.one;

            return obj;
        }
    }
}