using System;
using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    public abstract class ItemData : ScriptableObject
    {
        [field: SerializeField] public ItemType ItemType { get; protected set; }

        [field: SerializeField] public string ItemID { get; private set; }

        [field: SerializeField] public string ItemName { get; private set; }
        [field: SerializeField] public RarityType RarityType { get; private set; }

        [field: MinValue(0)] [field: InfoBox("base value when bought on shop")]
        [field: SerializeField] public int BuyValue { get; private set; } = 1;
        [field: SerializeField] public bool IsSellable { get; private set; }

        [field: MinValue(0)] [field: ShowIf("IsSellable")] [field: InfoBox("base value when sold on shop")]
        [field: SerializeField] public int SellValue { get; private set; } = 1;
        
        [field: SerializeField] public bool IsShippable { get; private set; }
        
        [field: MinValue(0)] [field: ShowIf("IsShippable")] [field: InfoBox("base value when shipped")]
        [field: SerializeField] public int ShippingValue { get; private set; } = 1;

        [field: ResizableTextArea]
        [field: SerializeField] public string Description { get; private set; }

        [field: SerializeField] public bool IsStackable { get; protected set; } = false;

        [field: ShowAssetPreview]
        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: ShowAssetPreview]
        [field: SerializeField] public GameObject Prefab { get; private set; }

        protected virtual void OnValidate()
        {
            
        }
    }
}