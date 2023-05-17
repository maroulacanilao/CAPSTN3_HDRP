using System.Collections;
using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using UnityEngine;

public interface IDamagable
{
    public Evt<DamageInfo> OnDamage { get; set; }
}