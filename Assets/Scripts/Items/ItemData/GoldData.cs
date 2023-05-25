namespace Items.ItemData
{
    [UnityEngine.CreateAssetMenu(menuName = "ScriptableObjects/ItemData/GoldData", fileName = "GoldData")]
    public class GoldData : ItemData
    {
        private void Reset()
        {
            ItemType = ItemType.Gold;
        }

        protected override void OnValidate()
        {
            ItemType = ItemType.Gold;
        }
        
        public ItemGold GetGoldItem(int amount_)
        {
            return new ItemGold(this, amount_);
        }
    }
}