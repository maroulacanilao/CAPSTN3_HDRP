using System.Collections;
using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(DamageInfo damageInfo_);
}