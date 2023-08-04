using System;

namespace GameTutorial
{
    [Serializable]
    public static class TutorialValues
    {
        public static bool HaveHoed = false;
        public static bool HaveWatered = false;
        public static bool HavePlanted = false;
        public static bool HaveGrown = false;
        public static bool HasStartingItems = false;
        public static bool HasEquippedItems = false;
        public static bool HasUnlockDungeon = false;
        public static bool HasOpenedInventory = false;
        public static bool HasHarvested = false;
        public static int HarvestedCount = 0;
        public static bool IsFarmingTutorialDone = false;
        public static bool DoneWithTutorial = false;
        
        // For Combat
        public static bool HasWelcomedCombat = false;
        public static bool HasTaughtBasicAttack = false;
        public static bool HasTaughtSpell = false;
        public static bool HasTaughtItem = false;
        public static bool HasTaughtRun = false;

        public static void ResetValues()
        {
            HaveHoed = false;
            HaveWatered = false;
            HavePlanted = false;
            HaveGrown = false;
            HasStartingItems = false;
            HasEquippedItems = false;
            HasUnlockDungeon = false;
            HasOpenedInventory = false;
            HasHarvested = false;
            HarvestedCount = 0;
            IsFarmingTutorialDone = false;
            DoneWithTutorial = false;
            HasWelcomedCombat = false;
            HasTaughtBasicAttack = false;
            HasTaughtSpell = false;
            HasTaughtItem = false;
            HasTaughtRun = false;
            
            // Fungus.FungusManager.Instance.SaveManager.
        }
    }
}
