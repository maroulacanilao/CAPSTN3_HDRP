using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable : IStorable
{
    public void OnUse();
}