using Items.Inventory;
using ScriptableObjectData;
using Shop;
using TMPro;
using UnityEngine;

namespace UI.ShrineUI
{
    public abstract class ShrineMenu : MonoBehaviour
    {
        [SerializeField] protected GameDataBase gameDataBase;
        [SerializeField] protected TextMeshProUGUI errorTxt;

        protected PlayerInventory inventory => gameDataBase.playerInventory;
        protected ShrineData shrineData => gameDataBase.shrineData;
        
        public virtual void Initialize()
        {
            
        }
    }
}