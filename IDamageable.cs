using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XC_Framework
{
    /// <summary>
    /// 伤害接口
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float damage,GameObject attacker);
    }
}