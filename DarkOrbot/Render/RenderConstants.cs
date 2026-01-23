using DarkOrbot.Properties;
using DarkOrbot.Util;
using DarksideApi.DarkOrbit;
using DarksideApi.Logic;
using System.Windows;
using System.Windows.Media;
namespace DarkOrbot.Render
{
    public class RenderConstants
    {
        public static double ScalingFactorX = 0.0125f;
        public static double ScalingFactorY = 0.0125f;
        public const double MapBufferZone = 12;
        public static readonly Pen NeutralPen = new(Brushes.White, 1);
        public static readonly Pen NeutralOpaquePen = new(new SolidColorBrush(Color.FromArgb(12, 255, 255, 255)), 1);
        public static readonly Pen LicensePen = new(Brushes.Violet, 1);
        public static readonly Pen MovementPen = new(Brushes.WhiteSmoke, 1);
        public static readonly Pen MovementPathPen = new(Brushes.LightSeaGreen, 1) { DashStyle = DashStyles.Solid };
        public static readonly Pen TargetSelectedPen = new(Brushes.OrangeRed, 1);
        public static readonly Pen TargetUnselectedPen = new(Brushes.Gray, 1);
        public static readonly Pen TargetDashPen = new(Brushes.LightGray, 1) { DashStyle = DashStyles.Dot };
        public static readonly Pen PathDashPen = new(new SolidColorBrush(Color.FromArgb(125, 169, 169, 169)), 1) { DashStyle = DashStyles.Dash };
        public static readonly Pen EnemyOnMapPen = new(Brushes.LightYellow, 2);
        public static readonly Pen BurningTrailPen = new(Brushes.IndianRed, 1);
        public static readonly Pen IcePen = new(Brushes.LightSkyBlue, 1);
        public static readonly Pen MMOPen = new(Brushes.OrangeRed, 1);
        public static readonly Pen EICPen = new(Brushes.RoyalBlue, 1);
        public static readonly Pen VRUPen = new(Brushes.LawnGreen, 1);
        public static readonly Pen PlayerPen = new(Brushes.Red, 1);
        public static readonly Pen ClanRelationOwnClanPen = new(ClanRelationOwnClanBrush, 0.8);
        public static readonly Pen ClanRelationWarPen = new(ClanRelationWarBrush, 0.8);
        public static readonly Pen ClanRelationAllyPen = new(ClanRelationAllyBrush, 0.8);
        public static readonly Pen HitpointPen = new(HitpointBrush, 1);
        public static readonly Pen ShieldPen = new(ShieldBrush, 1);
        public static readonly Pen HitpointDarkPen = new(HitpointDarkBrush, 1);
        public static readonly Pen ShieldDarkPen = new(ShieldDarkBrush, 1);
        public static readonly Pen NanoshieldPen = new(NanoshieldBrush, 1);
        public static readonly Pen NanoshieldDarkPen = new(NanoshieldDarkBrush, 1);
        public static readonly Pen DebugPen = new(Brushes.LightGreen, 1);
        public static readonly Pen WorkAreaPen = new(Brushes.LightSalmon, 1);
        public static readonly Pen ZoneBorderPen = new(ZoneBrush, 1);
        public static readonly Brush BlackBrush = Brushes.Black;
        public static readonly Brush NeutralBrush = Brushes.White;
        public static readonly Brush NeutralOpaqueBrush = new SolidColorBrush(Color.FromArgb(96, 255, 255, 255));
        public static readonly Brush HitpointBrush = HelpTools.GetBrushFromHTML("#0F7A44");
        public static readonly Brush HitpointTranslucentBrush = new SolidColorBrush(Color.FromArgb(72, 15, 122, 68));
        public static readonly Brush HitpointDarkBrush = HelpTools.GetBrushFromHTML("#094928");
        public static readonly Brush ShieldBrush = HelpTools.GetBrushFromHTML("#87D4F2");
        public static readonly Brush ShieldDarkBrush = HelpTools.GetBrushFromHTML("#517f91");
        public static readonly Brush NanoshieldBrush = HelpTools.GetBrushFromHTML("#F2E108");
        public static readonly Brush NanoshieldDarkBrush = HelpTools.GetBrushFromHTML("#918704");
        public static readonly Brush NpcBrush = Brushes.Orange;
        public static readonly Brush PlayerBrush = Brushes.Red;
        public static readonly Brush PlayerCloakedBrush = new SolidColorBrush(Color.FromArgb(96, 255, 0, 0));
        public static readonly Brush BoxBrush = Brushes.Yellow;
        public static readonly Brush OreBrush = Brushes.Blue;
        public static readonly Brush PalladiumBrush = Brushes.AliceBlue;
        public static readonly Brush BuildingBrush = Brushes.White;
        public static readonly Brush ClanRelationOwnClanBrush = new SolidColorBrush(Color.FromArgb(255, 51, 255, 51));
        public static readonly Brush ClanRelationWarBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static readonly Brush ClanRelationAllyBrush = new SolidColorBrush(Color.FromArgb(255, 254, 210, 135));
        public static readonly Brush PetBrush = Brushes.Cyan;
        public static readonly Brush PetCloakedBrush = new SolidColorBrush(Color.FromArgb(96, 0, 255, 255));
        public static readonly Brush HeroBrush = Brushes.Green;
        public static readonly Brush HeroCloakedBrush = new SolidColorBrush(Color.FromArgb(96, 0, 128, 0));
        public static readonly Brush ZoneBrush = new SolidColorBrush(Color.FromArgb(125, 27, 27, 27));
        public static readonly Brush ZoneBrushLight = new SolidColorBrush(Color.FromArgb(75, 27, 27, 27));
        public static readonly Brush MineBrush = HelpTools.GetBrushFromHTML("#7A1D1D");//HelpTools.GetBrushFromHTML("#7A1D1D1D");
        public static readonly Brush CaptchaBoxRedBrush = Brushes.MediumVioletRed;
        public static readonly Brush CaptchaBoxBlackBrush = Brushes.DarkSlateGray;
        public static readonly Brush MMOBrush = Brushes.OrangeRed;
        public static readonly Brush EICBrush = Brushes.RoyalBlue;
        public static readonly Brush VRUBrush = Brushes.LawnGreen;
        public static readonly Brush ValidBrush = Brushes.Green;
        public static readonly Brush InvalidBrush = Brushes.Red;
        public static readonly Brush DebugBrush = Brushes.LightGreen;
        public static readonly TileBrush TileZoneBrush = new ImageBrush(HelpTools.LoadImageFromByteArray(Resources.POI_NOA))
        {
            TileMode = TileMode.Tile,
            Stretch = Stretch.Fill,
        };
        public static readonly Typeface NeutralFont = new("Verdana");
        public static readonly Typeface ShipStatusFont = new("GenericSansSerif");// new(FontFamily.GenericSansSerif, 10);
        public static readonly Typeface InformationFont = new("Verdana");// new(FontFamily.GenericSansSerif, 8);
        public static readonly Typeface PlayerFont = new("GenericSansSerif");// new(FontFamily.GenericSansSerif, 6);
        public static readonly Typeface MineFont = new("GenericSansSerif");
        public static readonly Typeface MapFont = new(new FontFamily("Verdana"), FontStyles.Oblique, FontWeights.Bold, FontStretches.Normal);
        public static readonly Typeface MapFontLarge = new(new FontFamily("Verdana"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
        public static readonly Typeface DebugFont = new(new FontFamily("Calibri"), FontStyles.Normal, FontWeights.Normal, FontStretches.Expanded);

        public const string MineText = "X";
        public const string QuestGiverText = "!";
        public const string WreckText = "@";
        public const string SessionInvalidText = "X";
        public const string SessionValidText = "✓";
        public const string SaboteurDebuff = "~";
        public const string GopPositiv = "+";
        public const string GopNegativ = "-";
        public const string BoxInvalid = "/";

        public static void UpdateScalingFactor(double canvasWidth, double canvasHeight, double mapWidth, double mapHeight)
        {
            ScalingFactorX = (canvasWidth - 10) / mapWidth;
            ScalingFactorY = (canvasHeight - (MapBufferZone + 10)) / mapHeight;
        }

        public static string LogicToString(Api api)
        {
            return api.Logic.State switch
            {
                LogicState.None => "Roaming",
                //LogicState.GotoBox => "Box",
                LogicState.GotoNpc => "Npc",
                LogicState.GotoRepair => "Repair",
                LogicState.RepairJump => "Jumping for Repair",
                LogicState.GotoGateJump => "Gate",
                LogicState.GotoFlee => "Fleeing",
                LogicState.GateJump or LogicState.FleeJump => "Jumping",
                LogicState.GotoFleeSpotted => "Avoiding Enemy",
                LogicState.Shootback or LogicState.GotoShootback => "Shooting back",
                LogicState.FleeJumpback => "Jumping back",
                LogicState.AvoidMine => "Avoiding Mine",
                LogicState.AvoidClanBase => "Avoiding Clan Base",
                LogicState.Revive => "Reviving",
                LogicState.EnemyOnMap => $"{api.Map.EnemysCount} Enemies on Map",
                LogicState.CaptchaSolve => "Solving Captcha",
                //LogicState.Ore or LogicState.GotoOre => "Ore",
                LogicState.GotoCollectible or LogicState.Collectible => "Collecting",
                LogicState.GalaxyGate => "GG",
                LogicState.GalaxyGateRepair => "GG Repair",
                LogicState.GotoSellOre or LogicState.SellOre => "Selling Ore",
                LogicState.SellOreDone => "Changing Hangar",
                LogicState.AvoidTrader => "Moving away",
                LogicState.GotoGateJumpToAccessGG => "Moving to GG",
                _ => api.Logic.State.ToString(),
            };
        }

        public static string StateToString(GameState state)
        {
            return state switch
            {
                GameState.Loading => "Loading...",
                GameState.LoggingIn => "Logging in...",
                GameState.Disconnected => "Disconnected!",
                GameState.Incompatible => "Game Version Incompatible!",
                GameState.CollectingLoginBonus => "Collecting Login Bonus...",
                GameState.LoginFailed => "Login Failed!",
                GameState.Reconnect => string.Empty,
                _ => state.ToString(),
            };
        }
    }
}
