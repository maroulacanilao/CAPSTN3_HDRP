using Farming;
using Items.ItemData;
using Managers;
using ObjectPool;
using UnityEngine;

namespace GameTutorial
{
    public class FarmingTutorial : MonoBehaviour, IPoolable
    {
        public string wateringKey = "WaterTut", 
            Planting = "PlantingTut", 
            hoeingKey = "HoeTut", 
            Harvesting = "Harvesting",
            harvestMore = "HarvestMore",
            donewithTutorial = "DoneWithTutorial";
        private FarmTile farmTile;
    
        private void Awake()
        {
            farmTile = GetComponent<FarmTile>();
            farmTile.OnChangeState.AddListener(ChangeState);
        }

        private void OnDestroy()
        {
            farmTile.OnChangeState.RemoveListener(ChangeState);
        }

        public void OnSpawn()
        {
            if (TutorialValues.DoneWithTutorial)
            {
                Destroy(this);
                return;
            }
            if (!TutorialValues.HaveHoed) TutorialValues.HaveHoed = true;
            else if (TutorialValues.HaveHoed && !TutorialValues.HavePlanted)
            {
                Fungus.Flowchart.BroadcastFungusMessage(Planting);
            }
        }

        private static bool hasSentWateredMessage = false;
        private void Update()
        {
            if(hasSentWateredMessage) return;
            if (farmTile.isWatered)
            {
                TutorialValues.HaveWatered = true;
            }
        }

        public void OnDeSpawn()
        {
        
        }

        private void ChangeState(FarmTile tile_, TileState tileState_)
        {
            switch (tileState_)
            {
                case TileState.Planted when !TutorialValues.HavePlanted:
                    TutorialValues.HavePlanted = true;
                    Fungus.Flowchart.BroadcastFungusMessage(wateringKey);
                    return;
                case TileState.Growing:
                    Debug.Log("Growing");
                    Fungus.Flowchart.BroadcastFungusMessage(Harvesting);
                    break;
            }
        }
    
        private bool IsDoneWithTutorial()
        {
            return false;
        }

        private void DestroyAll()
        {
            var _tutComp = GameObject.FindObjectsOfType<FarmingTutorial>();

            for (int i = _tutComp.Length - 1; i >= 0; i--)
            {
                Destroy(_tutComp[i]);
            }
        }
    }
}
