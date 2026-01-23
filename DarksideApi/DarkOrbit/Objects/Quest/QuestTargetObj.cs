using DarksideApi.DarkOrbit.Commands;
using DarksideApi.Util.Extensions;
using static DarksideApi.DarkOrbit.Commands.Quests.QuestConditionModule;

namespace DarksideApi.DarkOrbit.Objects.Quest
{
    public class QuestTargetObj
    {
        //public int TargetId { get; private set; }
        public List<string> TargetIds { get; private set; }
        public int ConditionId { get; private set; }
        public ConditionType TargetType { get; private set; }
        public QuestTargetObj? SubTarget { get; private set; }
        public long Amount { get; private set; }
        public long CurrentAmount { get; set; }

        public string TargetDisplayType => this.GetTargetDisplayType();
        public string AmountDisplay => this.CurrentAmount + "/" + this.Amount;
        public bool NeedAmountDisplay => this.Amount > 1;
        public bool IsCompleted => this.CurrentAmount >= this.Amount;
        public string DisplayName => this.TargetDisplayType + " " + (NeedAmountDisplay ? this.AmountDisplay : "");

        public QuestTargetObj(List<string> targetIds, int conditionId, ConditionType targetType, long amount, long currentAmount, QuestTargetObj? subTarget = null)
        {
            this.TargetIds = targetIds;
            this.ConditionId = conditionId;
            this.TargetType = targetType;
            this.Amount = amount;
            this.CurrentAmount = currentAmount;
            this.SubTarget = subTarget;
        }

        private string GetTargetDisplayType()
        {
            return this.TargetType switch
            {
                ConditionType.COLLECT => this.GetOreDisplayName(),
                ConditionType.KILL_NPC or ConditionType.KILL_NPCS => this.GetNpcDisplayName(),
                ConditionType.COLLECT_BONUS_BOX => "Bonus Box",
                ConditionType.KILL_PLAYER or ConditionType.KILL_PLAYERS => "Kill Players",
                ConditionType.FINISH_GALAXY_GATE => (int.TryParse(this.TargetIds.First(), out var ggId) ? (CommandConstants.GALAXY_GATES.TryGetValue(ggId, out var ggName) ? ggName : "Galaxy Gate") : this.TargetIds.First()),
                ConditionType.REAL_TIME_HASTE or ConditionType.COUNTDOWN or ConditionType.ENDURANCE or ConditionType.TIMER or ConditionType.HASTE or ConditionType.REAL_TIME_DATE_HASTE => "Time",
                ConditionType.DAMAGE_NPCS or ConditionType.DAMAGE => this.GetNpcDisplayName(int.Parse(this.TargetIds.First())),
                ConditionType.MAP or ConditionType.VISIT_MAP_TYPE or ConditionType.MAP_DIVERSE or ConditionType.VISIT_MAP or ConditionType.ON_MAP => "Visit " + (CommandConstants.MAP_NAMES.TryGetValue(int.Parse(this.TargetIds.First()), out var mapName) ? mapName : "Map"),
                ConditionType.AVOID_DAMAGE or ConditionType.AVOID_DAMAGE_NPCS or ConditionType.AVOID_DAMAGE_PLAYERS => "Avoid DMG",
                ConditionType.TAKE_DAMAGE => "Take DMG",
                ConditionType.AVOID_DEATH => "Avoid Death",
                ConditionType.COORDINATES => "Position",
                ConditionType.DISTANCE => "Distance",
                ConditionType.TRAVEL => "Travel",
                ConditionType.FUEL_SHORTAGE => "Fuel",
                ConditionType.PROXIMITY => "Range",
                ConditionType.KILL_MAPASSETS or ConditionType.KILL_ANY => "Kill",
                ConditionType.EMPTY or ConditionType.MISCELLANEOUS => string.Empty,
                ConditionType.AMMUNITION or ConditionType.SPEND_AMMUNITION or ConditionType.SAVE_AMMUNITION => "Ammo",
                ConditionType.SALVAGE or ConditionType.STEAL => "Salvage",
                ConditionType.KILL => "Kill",
                ConditionType.DEAL_DAMAGE or ConditionType.DAMAGE_PLAYERS or ConditionType.DAMAGE_ENEMY_PLAYERS or ConditionType.DAMAGE_MAPASSETS => "Damage",
                ConditionType.DIE => "Die",
                ConditionType.AVOID_KILL_NPC or ConditionType.AVOID_KILL_NPCS => "Avoid NPC",
                ConditionType.AVOID_KILL_PLAYERS => "Avoid Players",
                ConditionType.PREVENT => "Prevent",
                ConditionType.JUMP => "Jump",
                ConditionType.AVOID_JUMP => "Avoid Jump",
                ConditionType.STEADINESS => "",
                ConditionType.MULTIPLIER => "",
                ConditionType.STAY_AWAY => "Avoid",
                ConditionType.IN_GROUP => "Group",
                ConditionType.WEB => "Web",
                ConditionType.CLIENT => "Client",
                ConditionType.CARGO => "Cargo",
                ConditionType.SELL_ORE => "Sell",
                ConditionType.LEVEL => "Level",
                ConditionType.USER_DEFINED => "",
                ConditionType.FINISH_QUEST => "Finish",
                ConditionType.QUICK_BUY => "Buy",
                ConditionType.ENTER_GROUP => "Group",
                ConditionType.RESTRICT_AMMUNITION_KILL_NPC or ConditionType.RESTRICT_AMMUNITION_KILL_PLAYER => "Use Ammo",
                ConditionType.VISIT_QUEST_GIVER => "Visit",
                ConditionType.BELOW_HONOUR => "Below Honor",
                ConditionType.SPECIAL_HONOUR => "Honor",
                ConditionType.IN_CLAN => "Clan",
                ConditionType.COLLECT_LOOT => "Collect",
                ConditionType.FINISH_STARTER_GATE => "Finish Gate",
                ConditionType.REFINE_ORE => "Refine",
                ConditionType.PUT_ITEM_IN_SLOT_BAR => "Equip",
                ConditionType.USE_ORE_UPDATE => "Use Lab",
                ConditionType.VISIT_JUMP_GATE_TO_MAP_TYPE => "Jump",
                ConditionType.ACTIVATE_MAP_ASSET_TYPE or ConditionType.VISIT_MAP_ASSET => "Use",
                ConditionType.UPDATE_SKYLAB_TO_LEVEL => "Upgrade Skylab",
                ConditionType.BEACON_TAKEOVER => "Beacon",
                ConditionType.FINISH_GALAXY_GATES => "Finish Gate",
                ConditionType.GAIN_INFLUENCE => "Influence",
                ConditionType.GENERIC_TEXT => "Text",
                ConditionType.DISRUPTED => "Disrupted",
                ConditionType.COLLECT_BUILT_RECIPE => "Assemble",
                ConditionType.COLLECT_BONUS_BOX_TYPE => "Collect",
                ConditionType.COMPLETED_OBJECTIVES => "Complete",
                ConditionType.ACTIVATE_CPU => "Use CPU",
                ConditionType.SPEND_ITEM => "Spend",
                ConditionType.CLEAR_GALAXY_GATE_WAVE => "Clear Wave",
                ConditionType.SCORE_ROGUE_LITE => "Astral Gate",
                ConditionType.COLLECT_BOOTY_BOX_TYPE => "Collect",
                ConditionType.SPEND_RECIPE_INGREDIENTS => "Recipe",
                ConditionType.HAS_GOLD_PASS => "Goldpass",
                ConditionType.COMPLETE_SEASON_PASS_QUESTS => "Season Quest",
                ConditionType.LOGIN => "Login",
                ConditionType.DURING_EVENT => "In Event",
                ConditionType.QUESTCASE or _ => this.TargetType.ToString(),
            };
        }

        private string GetNpcDisplayName(int index = 0)
        {
            foreach (KeyValuePair<int, Dictionary<string, int>> map in Api.NpcIdsByMap)
            {
                if (!map.Value.TryGetKey(int.Parse(this.TargetIds.ElementAt(index)), out var npcName))
                {
                    continue;
                }
                return npcName;
            }
            return "Npc";
        }

        private string GetOreDisplayName(int index = 0)
        {
            foreach (KeyValuePair<int, Dictionary<string, int>> map in Api.OreIdsByMap)
            {
                if (!map.Value.TryGetKey(int.Parse(this.TargetIds.ElementAt(index)), out var npcName))
                {
                    continue;
                }
                return npcName;
            }
            return "Ore";
        }

        public override string ToString()
        {
            return TargetDisplayType;
        }
    }
}
