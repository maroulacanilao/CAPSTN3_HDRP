using BaseCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Farming;
using CustomEvent;
using CustomHelpers;
using ObjectPool;
using ScriptableObjectData;
using ScriptableObjectData.CharacterData;
using Items;
using Items.Inventory;
using Items.ItemData;
using UnityEngine;
using Managers;

public class FishingManager : Singleton<FishingManager>
{
    [field: SerializeField] public List<ConsumableData> fishInPool { get; private set; } = new List<ConsumableData>();

    [SerializeField] private GameDataBase gameDataBase;
    private PlayerData playerData => gameDataBase.playerData;
    private PlayerInventory inventory => playerData.inventory;
    private PlayerLevel playerLevel => playerData.LevelData;

    [SerializeField] private int framesToCatch;

    public bool hasFishingStarted = false;
    public bool fishOnHook = false;

    public static readonly Evt<Item, int> OnCatchSuccess = new Evt<Item, int>();
    public static readonly Evt<FishingManager> OnHookedFish = new Evt<FishingManager>();

    public void InitiateFishing()
    {
        hasFishingStarted = true;
        Debug.Log("Fishing Start");
        int randomNumber = Random.Range(3, 5);
        StartCoroutine(WaitingForFish(randomNumber));
    }

    IEnumerator WaitingForFish(int secondsToBite_)
    {
        for (int i = 0; i < secondsToBite_; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        //HookedFish();
        StartCoroutine(HookedFishy());
        yield return null;
    }

    private void HookedFish()
    {
        fishOnHook = true;
        OnHookedFish.Invoke(this);
        AudioManager.PlayHit();
    }

    public IEnumerator HookedFishy()
    {
        fishOnHook = true;
        OnHookedFish.Invoke(this);
        AudioManager.PlayHit();

        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1f);
        }

        hasFishingStarted = false;
        fishOnHook = false;
        OnHookedFish.Invoke(this);
        yield return null;
    }

    public void FishingSuscess()
    {
        CollectFish(fishInPool[0]);
        StopCoroutine(HookedFishy());
    }

    private void CollectFish(ConsumableData fish_)
    {
        fishOnHook = false;
        hasFishingStarted = false;

        var _item = fish_.GetConsumableItem(1);
        AudioManager.PlayHarvesting();
        inventory.AddItem(_item);

        OnCatchSuccess.Invoke(_item, 1);
        OnHookedFish.Invoke(this);
    }
}
