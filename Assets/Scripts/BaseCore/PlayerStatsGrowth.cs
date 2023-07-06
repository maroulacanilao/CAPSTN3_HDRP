using Character;
using CustomEvent;
using Items;
using ScriptableObjectData.CharacterData;

namespace BaseCore
{
    public class PlayerStatsGrowth : StatsGrowth
    {
        public struct ChangeStatsInfo
        {
            public CombatStats prevStats;
            public CombatStats newStats;
            public CombatStats addedStats;
        }
        
        public readonly Evt<ChangeStatsInfo> OnStatsChanged = new Evt<ChangeStatsInfo>();
        
        private PlayerData playerData { get; set; }
        private int level => playerData.LevelData.CurrentLevel;
        
        public void SetPlayerData(PlayerData playerData_)
        {
            playerData = playerData_;
        }
        
        public override void SetAdditionalStats(CombatStats stats_)
        {
            var prevStats = GetTotalStats(level);
            
            additionalStats = stats_;
            
            var newStats = GetTotalStats(level);
            var addedStats = newStats - prevStats;
            OnStatsChanged.Invoke(new ChangeStatsInfo
            {
                prevStats = prevStats,
                newStats = newStats,
                addedStats = addedStats
            });
        }

        public override void AddEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!EquipmentStatsSources.Add(item_)) return;

            var prevStats = GetTotalStats(level);
            
            equipmentStats += stats_;
            
            var newStats = GetTotalStats(level);
            var addedStats = newStats - prevStats;
            OnStatsChanged.Invoke(new ChangeStatsInfo
            {
                prevStats = prevStats,
                newStats = newStats,
                addedStats = addedStats
            });
        }

        public override void RemoveEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!EquipmentStatsSources.Remove(item_)) return;

            var prevStats = GetTotalStats(level);
            
            equipmentStats -= stats_;
            
            var newStats = GetTotalStats(level);
            var addedStats = newStats - prevStats;
            OnStatsChanged.Invoke(new ChangeStatsInfo
            {
                prevStats = prevStats,
                newStats = newStats,
                addedStats = addedStats
            });
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public override void AddBonusStats(CombatStats stats_)
        {
            var prevStats = GetTotalStats(level);
            
            bonusStats += stats_;
            
            var newStats = GetTotalStats(level);
            var addedStats = newStats - prevStats;
            OnStatsChanged.Invoke(new ChangeStatsInfo
            {
                prevStats = prevStats,
                newStats = newStats,
                addedStats = addedStats
            });
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public override void RemoveBonusStats(CombatStats stats_)
        {
            var prevStats = GetTotalStats(level);
            
            bonusStats -= stats_;
            
            var newStats = GetTotalStats(level);
            var addedStats = newStats - prevStats;
            OnStatsChanged.Invoke(new ChangeStatsInfo
            {
                prevStats = prevStats,
                newStats = newStats,
                addedStats = addedStats
            });
        }
    }
}
