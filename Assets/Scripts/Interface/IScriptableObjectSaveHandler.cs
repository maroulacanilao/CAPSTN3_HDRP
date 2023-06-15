using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScriptableObjectSaveHandler
{
    public void SaveData();
    public bool LoadData();
}
