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
            DamagePopup damagePopup = Instantiate(m_Config.DamagePopupPrefab, position, Quaternion.identity);
            damagePopup.Setup(message);

            return damagePopup;
        }

        public GameObject Create(GameObject prefab, Transform parent)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.SetParent(parent);
            obj.transform.position = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            return obj;
        }
    }
}