namespace DarksideApi.DarkOrbit.Commands.Quests
{
    public class QuestConditionModule : Command //693
    {
        public new const short Id = 2002;
        public int questId;
        public List<string> matches;
        public ConditionType type;
        public ConditionType displayType;
        public double targetValue;
        public bool mandatory;
        public QuestConditionStateModule state;
        public List<QuestConditionModule> subConditions;

        public QuestConditionModule()
        {
            this.matches = [];
            this.state = new();
            this.subConditions = [];
        }

        public override void Read(ByteArray reader)
        {
            this.questId = reader.ReadInt();
            this.questId = this.questId << 5 | this.questId >>> 27;
            var i = 0;
            var max = reader.ReadByte();
            while (i < max)
            {
                this.matches.Add(reader.ReadUTF());
                i++;
            }
            this.type = (ConditionType)reader.ReadShort();
            this.displayType = (ConditionType)reader.ReadShort();
            this.targetValue = reader.ReadDouble();
            this.mandatory = reader.ReadBool();
            if (reader.ReadShort() != 0)
            {
                this.state.Read(reader);
            }
            i = 0;
            max = reader.ReadByte();
            while (i < max)
            {
                reader.ReadShort();
                QuestConditionModule questCondition = new();
                questCondition.Read(reader);
                this.subConditions.Add(questCondition);
                i++;
            }
        }

        public override byte[] Write()
        {
            throw new NotImplementedException();
        }

        public enum ConditionType
        {
            QUESTCASE,
            TIMER,
            HASTE,
            ENDURANCE,
            COUNTDOWN,
            COLLECT,
            KILL_NPC,
            DAMAGE,
            AVOID_DAMAGE,
            TAKE_DAMAGE,
            AVOID_DEATH,
            COORDINATES,
            DISTANCE,
            TRAVEL,
            FUEL_SHORTAGE,
            PROXIMITY,
            MAP,
            MAP_DIVERSE,
            EMPTY,
            MISCELLANEOUS,
            AMMUNITION,
            SAVE_AMMUNITION,
            SPEND_AMMUNITION,
            SALVAGE,
            STEAL,
            KILL,
            DEAL_DAMAGE,
            KILL_NPCS,
            KILL_PLAYERS,
            DAMAGE_NPCS,
            DAMAGE_PLAYERS,
            DAMAGE_ENEMY_PLAYERS,
            KILL_MAPASSETS,
            DAMAGE_MAPASSETS,
            VISIT_MAP,
            DIE,
            AVOID_KILL_NPC,
            AVOID_KILL_NPCS,
            AVOID_KILL_PLAYERS,
            AVOID_DAMAGE_NPCS,
            AVOID_DAMAGE_PLAYERS,
            PREVENT,
            JUMP,
            AVOID_JUMP,
            STEADINESS,
            MULTIPLIER,
            STAY_AWAY,
            IN_GROUP,
            KILL_ANY,
            WEB,
            CLIENT,
            CARGO,
            SELL_ORE,
            LEVEL,
            USER_DEFINED,
            COLLECT_BONUS_BOX,
            FINISH_QUEST,
            QUICK_BUY,
            ENTER_GROUP,
            RESTRICT_AMMUNITION_KILL_NPC,
            RESTRICT_AMMUNITION_KILL_PLAYER,
            KILL_PLAYER,
            VISIT_QUEST_GIVER,
            REAL_TIME_HASTE,
            BELOW_HONOUR,
            SPECIAL_HONOUR,
            IN_CLAN,
            COLLECT_LOOT,
            FINISH_STARTER_GATE,
            REFINE_ORE,
            PUT_ITEM_IN_SLOT_BAR,
            VISIT_MAP_TYPE,
            USE_ORE_UPDATE,
            VISIT_JUMP_GATE_TO_MAP_TYPE,
            ACTIVATE_MAP_ASSET_TYPE,
            VISIT_MAP_ASSET,
            UPDATE_SKYLAB_TO_LEVEL,
            ON_MAP,
            BEACON_TAKEOVER,
            FINISH_GALAXY_GATE,
            FINISH_GALAXY_GATES,
            GAIN_INFLUENCE,
            GENERIC_TEXT,
            DISRUPTED,
            REAL_TIME_DATE_HASTE,
            COLLECT_BUILT_RECIPE,
            COLLECT_BONUS_BOX_TYPE,
            COMPLETED_OBJECTIVES,
            ACTIVATE_CPU,
            SPEND_ITEM,
            CLEAR_GALAXY_GATE_WAVE,
            SCORE_ROGUE_LITE,
            COLLECT_BOOTY_BOX_TYPE,
            SPEND_RECIPE_INGREDIENTS,
            HAS_GOLD_PASS,
            COMPLETE_SEASON_PASS_QUESTS,
            LOGIN,
            DURING_EVENT
        }

        public override string ToString()
        {
            return $"QuestConditionModule: {this.questId}, Type: {this.type}, TargetValue: {this.targetValue}, Mandatory: {this.mandatory},\nMatches:[{string.Join(";", this.matches)}]\n, State: {this.state},\nSubConditions: [{string.Join(";", this.subConditions)}]\n";
        }
    }
}
