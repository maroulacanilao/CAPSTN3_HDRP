using System.Collections;
using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using UnityEngine;

public interface IHealable
{
    public Evt<HealInfo, bool> OnHeal { get; set; }
}