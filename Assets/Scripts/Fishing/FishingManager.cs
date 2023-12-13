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

    [field: SerializeField] public List<ConsumableData> fishableFish { get; private set; } = new List<ConsumableData>();

    [SerializeField] private GameDataBase gameDataBase;
    private PlayerData playerData => gameDataBase.playerData;
    private PlayerInventory inventory => playerData.inventory;
    private PlayerLevel playerLevel => playerData.LevelData;

    [SerializeField] private int framesToCatch;

    public bool hasFishingStarted = false;
    public bool fishOnHook = false;

    public static readonly Evt<Item, int> OnCatchSuccess = new Evt<Item, int>();
    public static readonly Evt<FishingManager> OnFishingStarted = new Evt<FishingManager>();
    public static readonly Evt<FishingManager> OnHookedFish = new Evt<FishingManager>();
    public static readonly Evt<bool> OnCaughtFish = new Evt<bool>();

    private Coroutine waitingForFishRoutine;
    private Coroutine hookedFishRoutine;

    public void InitiateFishing(int rodLevel)
    {
        hasFishingStarted = true;
        OnFishingStarted.Invoke(this);

        for (int i = 0; i < fishInPool.Count; i++)
        {
            int levelChecker = (int)fishInPool[i].RarityType;

            if (levelChecker <= rodLevel + 1)
            {
                fishableFish.Add(fishInPool[i]);
                Debug.Log($"{fishInPool[i].name}: Level {levelChecker}");
            }
        }

        Debug.Log("Fishing Start");
        int randomNumber = Random.Range(3, 5);
        waitingForFishRoutine = StartCoroutine(WaitingForFish(randomNumber));
    }

    IEnumerator WaitingForFish(int secondsToBite_)
    {
        for (int i = 0; i < secondsToBite_; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        //HookedFish();
        hookedFishRoutine = StartCoroutine(HookedFishy());
        yield return null;
    }

    public void CancelFishing()
    {
        hasFishingStarted = false;
        StopCoroutine(waitingForFishRoutine);
        fishableFish.Clear();

        OnCaughtFish.Invoke(false);
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

        yield return new WaitForSeconds(1.5f);

        FishingFailed();
        yield return null;
    }

    public void FishingSuscess()
    {
        int randomFish = Random.Range(0, fishableFish.Count);
        CollectFish(fishableFish[randomFish]);
        StopCoroutine(hookedFishRoutine);

        OnCaughtFish.Invoke(true);
    }

    private void FishingFailed()
    {
        AudioManager.PlayMissSfx();

        hasFishingStarted = false;
        fishOnHook = false;
        fishableFish.Clear();

        OnHookedFish.Invoke(this);
        OnCaughtFish.Invoke(false);
    }

    private void CollectFish(ConsumableData fish_)
    {
        fishOnHook = false;
        hasFishingStarted = false;

        var _item = fish_.GetConsumableItem(1);
        AudioManager.PlayHarvesting();
        inventory.AddItem(_item);
        gameDataBase.fishDataBase.AddCatch(_item.Data as ConsumableData);
        fishableFish.Clear();

        OnCatchSuccess.Invoke(_item, 1);
        OnHookedFish.Invoke(this);
    }
}
