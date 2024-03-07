using FS.Characters.Turret;
using FS.Cores;
using UnityEngine;

namespace FS.Datas
{
    [CreateAssetMenu(fileName = "Prefab", menuName = "FS/Configiration/Prefab", order = 1)]
    public class PrefabConfiguration : ScriptableObject
    {
        public DamagePopup DamagePopupPrefab;
        public TurretBullet TurretBulletPrefab;
        public GameObject TurretBulletAreaPrefab;
    }
}