﻿using FS.Characters;
using System;
using UnityEngine;

namespace FS.Cores.Formulas
{
    /// <summary>
    /// Self condition when get hit or stat change
    /// </summary>
    public static class Formula
    {
        /// <summary>
        /// This must be call when character need to apply stat
        /// </summary>
        /// <param name="self"></param>
        public static void ApplyStat(this IBehavior self, CharacterStat statModifier, bool isInit = false)
        {
            if(isInit == true)
            {
                self.Stat.MaxHP = statModifier.MaxHP;
                self.Stat.HP = statModifier.MaxHP;
                self.Stat.Attack = statModifier.Attack;
                self.Stat.AttackRate = statModifier.AttackRate;
            }
            else
            {
                self.Stat.MaxHP += statModifier.MaxHP;
                self.Stat.HP = Mathf.Clamp(self.Stat.HP + statModifier.HP, 0, self.Stat.MaxHP);
                self.Stat.Attack += statModifier.Attack;
            }
        }

        /// <summary>
        /// This must be call when character dead or need to restat stat
        /// </summary>
        /// <param name="self"></param>
        public static void ResetStat(this IBehavior self)
        {
            self.Stat.HP = 0;
            self.Stat.Attack = 0;
        }

        /// <summary>
        /// Call when Player or Enemy get hit
        /// </summary>
        /// <param name="self"></param>
        /// <param name="enemy"></param>
        public static DamageMessage TakeDamage(this IBehavior self, IBehavior enemy)
        {
            int enemyDamage = enemy.Stat.Attack;

            self.Stat.HP = Math.Max(0, self.Stat.HP - enemyDamage);

            return new DamageMessage
            {
                DamageNet = enemyDamage,
                DamageHitFilter = DamageHitFilter.Normal,
                IsDead = self.IsDead,
                Self = self,
                Enemy = enemy
            };
        }
    }
}