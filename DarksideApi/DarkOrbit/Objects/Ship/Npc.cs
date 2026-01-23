using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Settings;
using DarksideApi.Util;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Ship
{
    public class Npc(Api api, int userID, string username, int x, int y, string shipname, bool cloaked, string specialType) : Ship(api, userID, username, x, y, shipname), IGameObject
    {
        //Status
        public bool Cloaked { get; set; } = cloaked;
        public int LastAttackerId { get; set; } = 0;
        public string SpecialType { get; set; } = specialType;

        //Effects
        public List<int> MinioinNpcs = [];

        public bool IsInCorner => this.HasApi && (HelpTools.CalculateDistance(0, 0, this.X, this.Y) < 5 || HelpTools.CalculateDistance(20999, 13499, this.X, this.Y) < 5);
        public bool IsFleeing => this.HasApi && (HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, TargetX, TargetY) > HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, X, Y));// && this.HitpointsPercentage < 25;//HelpTools.IsMovingAwayFrom(api.Hero.X, api.Hero.Y, tempLastPositions);//HasApi && HelpTools.CalculateDistance(api.Hero.X, api.Hero.Y, TargetX, TargetY) > 600;
        public bool IsFleeingToCorner => this.MaxHitpoints > 0 && this.HitpointsPercentage < 23
            && (HelpTools.IsMovingTowards(0, 0, this.tempLastPositions) || HelpTools.IsMovingTowards(20999, 13499, this.tempLastPositions))
            && HelpTools.CalculateDistance(this.TargetX, this.TargetY, this.api.Hero.TargetX, this.api.Hero.TargetY) > 600
            || this.IsInCorner;

        public bool IsGru/*LOL*/ => this.MinioinNpcs.Count > 0;
        public int MinionsAlive => this.api.Ships.Count(n => this.MinioinNpcs.Contains(n.Key));
        public bool IsProtected => this.Username == "-=[ Devourer ]=- ζ25"
                    || this.Username == "-=[ Devourer ]=- ζ27"
                    || this.Username == "-=[ StreuneR ]=- δ4"
                    || this.Username == "-=[ Lordakium ]=- δ9"
                    || this.Username == "-=[ Sibelon ]=- δ14"
                    || this.Username == "-=[ Kristallon ]=- δ19"
                    || this.Username == "..::{ Boss Lordakium }::... δ21"
                    || this.Username == "..::{ Boss Lordakium }::... δ23"
                    || this.Username == "..::{ Boss Lordakium }::... δ25"
                    || this.Username == "-=[ Battleray ]=-"
                    || this.Username == "-=[ Deadly Battleray ]=-"
                    || this.Username == "( Uber Battleray )"
                    || this.Username == "-={ Gygerim Overlord }=-"
                    || this.Username == "-=[ Hitac 2.0 ]=-"
                    || this.Username == "-=[ Hitac 2.5 ]=-"
                    //|| (this.Username.Contains("=^(Plutus)^="))   //Ꝕ
                    || this.Username.Contains("-=[ Sunder Star ]=-")     //ȼ1,c2 //not c3 & c4
                    || this.Username.Contains("-=[ Slow Field ]=-")      //c2
                    || (this.Username.Contains("-=[ Warhead ]=-") && api.Map.IsTreachorusDomainMap)       //c3
                    || this.UserId.Equals("-x->ICETERION<-x-");
        public bool IsPassive => this.Username == "\\ Abide I // EBG51"
                    || this.Username == "\\ Steadfast III // EBG51";
        public bool IsWarhead => this.Username.Contains("Warhead");
        public bool IsTurrent => this.Username.Contains("-=[ Turret ]=-");
        public bool IsEmFreighter => this.Username == ("-={EM Freighter}=-");
        public bool IsHeavyNpc => this.MaxHitpoints > 149999;
        public bool IsDefendable => this.SpecialType == "PLAYER_CAMP_NPC_TYPE";
        public bool IsMinion => this.Username.Contains(CommandConstants.NPC_INTERCEPTOR) || this.Username.Contains(CommandConstants.NPC_HITAC_MINION);
        public bool IsHitacMinion => this.Username.Contains(CommandConstants.NPC_HITAC_MINION);
        public bool IsHitac => this.Username.Contains("Hitac 2.0") || this.Username.Contains("Hitac 2.5");
        public Npc GetMotherShip => !this.IsHitacMinion ? this : (api.Ships.Values.OfType<Npc>().FirstOrDefault(s => s.IsHitac) ?? this);

        public bool IsValid(NpcSetting npcSetting)
        {
            return this != null && npcSetting != null && this.HasApi && this.Username == npcSetting.Name && this.IsSelectable && npcSetting.IsEnabled &&
                   (!npcSetting.OnlyFree || (this.LastAttackerId == 0
                    || this.LastAttackerId == this.api.Hero.UserId
                    || this.Shield + 1 == this.MaxShield
                    || (this.api.Group != null && this.api.Group.IsInGroup(LastAttackerId)))
                   );
        }

        public new double GetRadius(double radius)
        {
            return (this.IsFleeing ? 
                this.Speed >= this.api.Hero.Speed ? 
                radius * 0.75 : (radius * (1 - ((this.api.Hero.Speed - this.Speed) / 750 * 0.75)))
                : radius);
        }

        public static Npc Empty()
        {
            return new Npc(null, 0, string.Empty, 0, 0, string.Empty, false, string.Empty);
        }
    }
}
