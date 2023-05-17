using BaseCore;
using Character;
using CustomEvent;
using UnityEngine;

public class FarmSceneManager : Singleton<FarmSceneManager>
{
    [field: SerializeField] public GameObject player { get; private set; }
}
