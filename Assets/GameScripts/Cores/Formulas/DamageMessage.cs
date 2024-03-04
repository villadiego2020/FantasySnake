using UnityEngine;

namespace FS.Cores.Formulas
{
    public struct DamageMessage
    {
        public float DamageNet;
        public DamageHitFilter DamageHitFilter;
        public bool IsDead;
        public IBehavior Self;
        public IBehavior Enemy;

        public override string ToString()
        {
            return $"Damage:[{DamageNet}], Damage Filter:[{DamageHitFilter}], Enemy State:[{IsDead}]" +
                $", Self:{Self.name}: Enemy:{Enemy.name}";
        }
    }
}