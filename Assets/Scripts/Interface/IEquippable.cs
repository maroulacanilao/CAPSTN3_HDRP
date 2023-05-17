using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

public interface IEquippable : IStorable
{
    public void OnEquip(CharacterBase character_);
    public void OnUnEquip(CharacterBase character_);
}