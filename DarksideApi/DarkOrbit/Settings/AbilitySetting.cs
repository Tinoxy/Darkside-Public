using DarksideApi.DarkOrbit.Objects.Ship;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace DarksideApi.DarkOrbit.Settings
{
    public class AbilitySetting : Setting
    {
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_ALWAYS = "Always";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_EVERY = "Every";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_HP_BELOW = "Hitpoints below";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_HP_ABOVE = "Hitpoints above";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_SHIELD_BELOW = "Shield Below";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_SHIELD_ABOVE = "Shield Above";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_TARGET_HP_BELOW = "Target Hitpoints below";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_TARGET_HP_ABOVE = "Target Hitpoints above";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_TARGET_SHIELD_BELOW = "Target Shield below";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_TARGET_SHIELD_ABOVE = "Target Shield above";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_FLEEING = "When Fleeing";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_SHOOTBACK = "When Attacking Player";
        [XmlIgnore, JsonIgnore] public const string ABILITY_USAGE_GROUP_HP_BELOW = "Group Member Hitpoints below";
        [XmlIgnore, JsonIgnore]
        public static readonly List<string> ABILITY_USAGES = [
            ABILITY_USAGE_ALWAYS,
            ABILITY_USAGE_EVERY,
            ABILITY_USAGE_HP_BELOW,
            ABILITY_USAGE_HP_ABOVE,
            ABILITY_USAGE_SHIELD_BELOW,
            ABILITY_USAGE_SHIELD_ABOVE,
            ABILITY_USAGE_TARGET_HP_BELOW,
            ABILITY_USAGE_TARGET_HP_ABOVE,
            ABILITY_USAGE_TARGET_SHIELD_BELOW,
            ABILITY_USAGE_TARGET_SHIELD_ABOVE,
            ABILITY_USAGE_FLEEING,
            ABILITY_USAGE_SHOOTBACK,
            ABILITY_USAGE_GROUP_HP_BELOW,
        ];

        public string? AbilityType { get; set; }
        public string? AbilityName { get; set; }
        public string? AbilityUsageType { get; set; }
        public int AbilityUsageData { get; set; }

        [XmlIgnore, JsonIgnore]
        public bool NeedsUsageDataDisplayed => !(this.AbilityUsageType == ABILITY_USAGE_ALWAYS || this.AbilityUsageType == ABILITY_USAGE_FLEEING || this.AbilityUsageType == ABILITY_USAGE_SHOOTBACK);
        [XmlIgnore, JsonIgnore]
        public string AbilityUsageDisplay => this.AbilityUsageType + (!this.NeedsUsageDataDisplayed ? "" : (" " + this.AbilityUsageData + (this.AbilityUsageType == ABILITY_USAGE_EVERY ? "Min." : "%")));
        [XmlIgnore, JsonIgnore]
        public DateTime lastUsed = DateTime.MinValue;

        public bool Activatable(Api api)
        {
            if (!this.IsEnabled)
            {
                return false;
            }

            if (api.Logic.State == Logic.LogicState.Pausing || api.Logic.State == Logic.LogicState.PausingDeaths || api.Logic.State == Logic.LogicState.Revive
                || api.Logic.State == Logic.LogicState.AvoidClanBase || api.Logic.State == Logic.LogicState.AvoidMine
                || api.Logic.State == Logic.LogicState.GalaxyGateRepair
                || api.Logic.State == Logic.LogicState.GotoRepair
                || api.Logic.State == Logic.LogicState.GotoSellOre
                || api.Logic.State == Logic.LogicState.Repair || api.Logic.State == Logic.LogicState.RepairJump || api.Logic.State == Logic.LogicState.SellOre)
            {
                return false;
            }

            if (this.AbilityUsageType == ABILITY_USAGE_ALWAYS)
            {
                return true;
            }

            if (this.AbilityUsageType == AbilitySetting.ABILITY_USAGE_EVERY)
            {
                return (DateTime.UtcNow - this.lastUsed).TotalSeconds > (60 * this.AbilityUsageData);
            }

            if (this.AbilityUsageType == ABILITY_USAGE_HP_BELOW)
            {
                return api.Hero.HitpointsPercentage < this.AbilityUsageData;
            }
            if (this.AbilityUsageType == ABILITY_USAGE_HP_ABOVE)
            {
                return api.Hero.HitpointsPercentage > this.AbilityUsageData;
            }

            if (this.AbilityUsageType == ABILITY_USAGE_SHIELD_BELOW)
            {
                return api.Hero.ShieldPercentage < this.AbilityUsageData;
            }
            if (this.AbilityUsageType == ABILITY_USAGE_SHIELD_ABOVE)
            {
                return api.Hero.ShieldPercentage > this.AbilityUsageData;
            }

            if (this.AbilityUsageType == ABILITY_USAGE_FLEEING)
            {
                return api.Logic.State == Logic.LogicState.GotoFlee;
            }

            if (this.AbilityUsageType == ABILITY_USAGE_SHOOTBACK)
            {
                return api.Logic.State == Logic.LogicState.Shootback;
            }

            if (this.AbilityUsageType == ABILITY_USAGE_GROUP_HP_BELOW)
            {
                if (api.Group == null)
                {
                    return false;
                }
                return api.Group.Members.Any(m => m.HitpointsPercentage > 0 && m.HitpointsPercentage < this.AbilityUsageData);
            }

            if (api.Logic.Target is Ship ship)
            {
                if (ship is Npc n && !api.Logic.GetNpcSetting(n).UseAbility)
                {
                    return false;
                }

                if (this.AbilityUsageType == ABILITY_USAGE_TARGET_HP_BELOW)
                {
                    return ship.HitpointsPercentage < this.AbilityUsageData;
                }
                if (this.AbilityUsageType == ABILITY_USAGE_TARGET_HP_ABOVE)
                {
                    return ship.HitpointsPercentage > this.AbilityUsageData;
                }

                if (this.AbilityUsageType == ABILITY_USAGE_TARGET_SHIELD_BELOW)
                {
                    return ship.ShieldPercentage < this.AbilityUsageData;
                }
                if (this.AbilityUsageType == ABILITY_USAGE_TARGET_SHIELD_ABOVE)
                {
                    return ship.ShieldPercentage > this.AbilityUsageData;
                }
            }

            return false;
        }
    }
}
