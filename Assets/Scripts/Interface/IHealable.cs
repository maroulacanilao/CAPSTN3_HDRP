using System.Collections;
using System.Collections.Generic;
using BaseCore;
using CustomEvent;
using UnityEngine;

public interface IHealable
{
    public void Heal(HealInfo healInfo_, bool isOverHeal_ = false);
}