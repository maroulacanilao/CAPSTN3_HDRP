using System;
using System.Collections;
using System.Collections.Generic;
using Dungeon;
using TMPro;
using UnityEngine;

public class DungeonUI : MonoBehaviour
{
    [SerializeField]
    private DungeonManager dungeonManager;
    
    [SerializeField] 
    private TextMeshProUGUI floorText;

    private void FixedUpdate()
    {
        floorText.text = $"Floor {dungeonManager.currentLevel}";
    }
}
