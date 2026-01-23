using DarkOrbot.Util;
using DarkOrbot.Util.Extensions;
using DarksideApi.DarkOrbit;
using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects;
using DarksideApi.DarkOrbit.Objects.Boosters;
using DarksideApi.DarkOrbit.Objects.Boxes;
using DarksideApi.DarkOrbit.Objects.Client;
using DarksideApi.DarkOrbit.Objects.Map;
using DarksideApi.DarkOrbit.Objects.Pets;
using DarksideApi.DarkOrbit.Objects.Ship;
using DarksideApi.Logic;
using DarksideApi.Util.Extensions;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DarkOrbot.Render
{
    public class MapRenderer : FrameworkElement
    {
        private readonly Api api;
        private readonly VisualCollection visuals;
        private readonly DispatcherTimer timer;
        private BitmapImage mapImage;
        private double zoom;
        private int imgMapId;

        private double canvasWidth;
        private double canvasHeight;
        private double pixelsPerDip;

        private Point currentMousePosition;
        private readonly List<Point> lastPositions;

        public MapRenderer(Api api, double canvasWidth, double canvasHeight)
        {
            this.visuals = new VisualCollection(this);

            this.api = api;
            this.mapImage = null!;
            this.canvasWidth = canvasWidth;
            this.canvasHeight = canvasHeight;
            this.pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            this.lastPositions = [];

            this.timer = new()
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            this.zoom = 0.4;
            this.timer.Tick += this.Timer_Tick;

            this.Start();
        }

        public void UpdateCanvasSize(double width, double height)
        {
            this.canvasWidth = width;
            this.canvasHeight = height;
            this.pixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
            RenderConstants.UpdateScalingFactor(this.canvasWidth, this.canvasHeight, this.api.Map.Width, this.api.Map.Height);
        }

        public void Stop()
        {
            this.timer?.Stop();
        }

        public void Start()
        {
            this.timer?.Start();
        }

        public void UpdateInterval(double miliseconds = 100)
        {
            if (this.timer is null)
            {
                return;
            }
            this.timer.Interval = TimeSpan.FromMilliseconds(miliseconds);
        }

        public void AddLastPosition(Vector2 vector)
        {
            if (this.lastPositions?.Count > 42)
            {
                this.lastPositions?.RemoveAt(0);
            }
            this.lastPositions?.Add(vector.ToDrawingPoint());
        }

        public void ClearLastPositions()
        {
            this.lastPositions?.Clear();
        }

        public void SetCurrentMousePosition(Point pos)
        {
            this.currentMousePosition = pos;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            this.visuals.Clear();
            this.visuals.Add(this.Draw());
        }

        private DrawingVisual Draw()
        {
            try
            {
                if (!this.api.Settings.IsMinimapShown) { return new(); }
                if (this.api.Map == null) { return new(); }

                if (this.api.Map != null && this.imgMapId != this.api.Map.Id)
                {
                    this.imgMapId = this.api.Map.Id;
                    this.ClearLastPositions();
                    this.mapImage = null!;
                }
                if (this.api.Settings.MinimapShowBackground && this.mapImage == null)
                {
                    if (this.api.Map == null)
                        return new();

                    this.mapImage = HelpTools.GetMinimapImage(this.api.Map.Id);
                }

                RenderConstants.UpdateScalingFactor(this.canvasWidth, this.canvasHeight, this.api.Map.Width, this.api.Map.Height);

                List<Box> boxes = this.api.Settings.MinimapShowBoxes ? [.. this.api.Collectibles.Values.OfType<Box>()] : [];
                List<Ore> ores = this.api.Settings.MinimapShowOres ? [.. this.api.Collectibles.Values.OfType<Ore>()] : [];
                List<Npc> npcs = this.api.Settings.MinimapShowNpcs ? [.. this.api.Ships.Values.OfType<Npc>()] : [];
                List<Player> players = this.api.Settings.MinimapShowPlayers ? [.. this.api.Ships.Values.OfType<Player>()] : [];
                List<Ship> ships = [.. this.api.Ships.Values];
                List<Gate> gates = this.api.Settings.MinimapShowGates ? [.. this.api.Gates.Values] : [];
                List<MapAsset> buildings = this.api.Settings.MinimapShowBuildings ? [.. this.api.MapAssets.Values] : [];
                List<Pet> pets = this.api.Settings.MinimapShowPet ? [.. this.api.Pets.Values] : [];
                List<Mine> mines = this.api.Settings.MinimapShowMines ? [.. this.api.Mines.Values] : [];
                List<PoiZone> zones = this.api.Settings.MinimapShowZones ? [.. this.api.POIZones.Values] : [];
                Vector2 poi = this.api.PointOfInterest;
                DateTime dateTime = DateTime.UtcNow;

                DrawingVisual drawingVisual = new();
                RenderOptions.SetEdgeMode(drawingVisual, EdgeMode.Aliased);
                RenderOptions.SetBitmapScalingMode(drawingVisual, BitmapScalingMode.HighQuality);

                // Retrieve the DrawingContext in order to create new drawing content.
                using DrawingContext drawingContext = drawingVisual.RenderOpen();

                this.DrawBackground(drawingContext);

                /*if (App.LicenseManager.LicenseState == Licensing.LicenseState.Invalid)
                {
                    timer.Interval = TimeSpan.FromMilliseconds(120000);
                    DrawStringCenter(drawingContext, $"Expired.");
                    goto IL_DRAW;
                }*/
                if (this.api.Logic is null
                    || this.api.GameClient is null)
                {
                    this.UpdateInterval(1000);
                    //DrawStringCenter(drawingContext, "License invalid!");
                    goto IL_DRAW;
                }

                if (this.api.Logic.State == LogicState.Pausing)
                {
                    this.UpdateInterval(10000);
                    var pauseTimeLeft = Math.Round(this.api.Settings.GeneralPausePauseTime * 60 - (dateTime - this.api.Logic.PauseStartTime).TotalMinutes, 0);
                    this.DrawStringCenter(drawingContext, pauseTimeLeft < 0 ? "Logged out." : $"Pausing {pauseTimeLeft}Min.");
                    goto IL_DRAW;
                }

                if (this.api.Logic.State == LogicState.PausingDeaths)
                {
                    this.UpdateInterval(1000);
                    var pauseTimeLeft = Math.Round(10 - (dateTime - this.api.Logic.PauseStartTime).TotalMinutes, 0);
                    this.DrawStringCenter(drawingContext, $"Too many rapid Deaths, pausing {pauseTimeLeft}Min.");
                    goto IL_DRAW;
                }

                if (this.api.GameClient.GameState != GameState.Ready)
                {
                    this.UpdateInterval(1000);
                    var state = RenderConstants.StateToString(this.api.GameClient.GameState);
                    this.DrawStringCenter(drawingContext, state);
                    goto IL_DRAW;
                }

                if (this.api.Logic.State == LogicState.Revive)
                {
                    this.UpdateInterval(1000);
                    var state = RenderConstants.LogicToString(this.api);
                    this.DrawStringCenter(drawingContext, state);
                    goto IL_DRAW;
                }

                if (this.timer.Interval.TotalMilliseconds >= 1000)
                {
                    this.UpdateInterval(100);
                }

                this.DrawBorders(drawingContext);

                Point pos;
                Point heroPos = this.api.Settings.IsMinimapPlayerView ? new(this.canvasWidth / 2, this.canvasHeight / 2) : this.Scale(this.api.Hero.X, this.api.Hero.Y);

                if (this.api.Settings.WorkAreaSettings.TryGetValue(this.api.Map.Id, out DarksideApi.DarkOrbit.Settings.WorkAreaSetting? workAreaSetting))
                {
                    pos = workAreaSetting.CornerBottomRight == default ? this.Scale(this.currentMousePosition) : this.Scale(workAreaSetting.CornerBottomRight);
                    drawingContext.DrawRectangle(null, RenderConstants.WorkAreaPen, new(this.Scale(workAreaSetting.CornerTopLeft), pos));
                }

                foreach (PoiZone zone in zones)
                {
                    pos = this.Scale(zone.X, zone.Y);
                    Pen? pen = RenderConstants.ZoneBorderPen;
                    Brush? zoneBrush = zone.IsNoAccess ? 
                        zone.IsNoAccessCustom ? null 
                        : RenderConstants.TileZoneBrush 
                        : RenderConstants.ZoneBrush;

                    if (this.api.Movement.PATH_ZONES.Contains(zone) && !zone.IsNoAccessCustom)
                    {
                        pen = RenderConstants.MovementPathPen;
                    }
                    if (zone.Shape == CommandConstants.POI_SHAPE_CIRCLE)
                    {
                        var circleWidth = this.api.Settings.IsMinimapPlayerView ? zone.Radius * this.zoom : zone.Radius * RenderConstants.ScalingFactorX;
                        var circleHeight = this.api.Settings.IsMinimapPlayerView ? zone.Radius * this.zoom : zone.Radius * RenderConstants.ScalingFactorY;
                        drawingContext.DrawEllipse(zoneBrush, pen, pos, circleWidth, circleHeight);
                        continue;
                    }
                    if (zone.Shape == CommandConstants.POI_SHAPE_POLYGON)
                    {
                        var geometry = new StreamGeometry();
                        using (StreamGeometryContext ctx = geometry.Open())
                        {
                            var vector = zone.CornerPositions.First();
                            ctx.BeginFigure(new Point(vector.X, vector.Y), true, true);
                            for (int i = 1; i < zone.CornerPositions.Count; i++)
                            {
                                vector = zone.CornerPositions.ElementAt(i);
                                ctx.LineTo(new Point(vector.X, vector.Y), true , true);
                            }
                        }
                        geometry.Freeze();
                        drawingContext.DrawGeometry(zoneBrush, pen, geometry);
                        continue;
                    }
                    if (zone.Shape == CommandConstants.POI_SHAPE_RECTANGLE)
                    {
                        pos = this.Scale(zone.TopLeft);
                        var zoneWidth = this.api.Settings.IsMinimapPlayerView ? zone.Width * this.zoom : zone.Width * RenderConstants.ScalingFactorX;
                        var zoneHeight = this.api.Settings.IsMinimapPlayerView ? zone.Height * this.zoom : zone.Height * RenderConstants.ScalingFactorY;
                        drawingContext.DrawRectangle(zoneBrush, pen, new(pos.X, pos.Y, zoneWidth, zoneHeight));
                        continue;
                    }
                }

                foreach (MapAsset building in buildings)
                {
                    if (building.Name.Equals("burningTrail"))
                    {
                        continue;
                    }
                    pos = this.Scale(building.X, building.Y);
                    if (building.IsStationTurrent)
                    {
                        drawingContext.DrawEllipse(RenderConstants.BuildingBrush, null, pos, 3, 3);
                        continue;
                    }
                    if (building.IsHpBeacon || building.IsGOPGeneratorPositiv)
                    {
                        this.DrawStringScaled(drawingContext, RenderConstants.GopPositiv, building.X, building.Y, 12, RenderConstants.InformationFont, RenderConstants.HitpointBrush);
                        drawingContext.DrawEllipse(null, RenderConstants.HitpointPen, pos, building.Radius, building.Radius);
                        building.Radius = (building.Radius + 2) % 24;
                        continue;
                    }
                    if (building.IsQuestGiver)
                    {
                        this.DrawStringScaled(drawingContext, RenderConstants.QuestGiverText, building.X, building.Y, 10, RenderConstants.InformationFont, RenderConstants.NanoshieldBrush);
                        continue;
                    }
                    if (building.IsWreck)
                    {
                        this.DrawStringScaled(drawingContext, RenderConstants.WreckText, building.X, building.Y, 4, RenderConstants.MineFont, RenderConstants.MineBrush);
                        continue;
                    }
                    if (building.IsGOPGeneratorNegativ)
                    {
                        this.DrawStringScaled(drawingContext, RenderConstants.GopNegativ, building.X, building.Y, 12, RenderConstants.NeutralFont, RenderConstants.MineBrush);
                        continue;
                    }
                    drawingContext.DrawEllipse(building.ClanTag.Length > 0 ? building.FactionId == this.api.Hero.FactionId
                        || building.ClanRelation == CommandConstants.CLAN_RELATION_ALLIED
                        || building.ClanRelation == CommandConstants.CLAN_RELATION_NON_AGGRESSION_PACT ? RenderConstants.ClanRelationAllyBrush
                        : building.ClanTag == this.api.Hero.ClanTag ? RenderConstants.ClanRelationOwnClanBrush
                        : RenderConstants.ClanRelationWarBrush
                        : RenderConstants.BuildingBrush, null, pos, 7, 7);
                }

                foreach (Gate gate in gates)
                {
                    pos = this.Scale(gate.X, gate.Y);
                    drawingContext.DrawEllipse(null, !gate.IsVisible ? RenderConstants.NeutralOpaquePen : gate.FactionId == 1 ?
                        RenderConstants.MMOPen : gate.FactionId == 2 ?
                        RenderConstants.EICPen : gate.FactionId == 3 ?
                        RenderConstants.VRUPen : RenderConstants.NeutralPen, pos, 5, 5);
                    if (this.api.LockedGateIds.Contains(gate.GateId))
                    {
                        var lockedText = new FormattedText("X", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 7, !gate.IsVisible ? RenderConstants.NeutralOpaqueBrush : RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(lockedText, new(pos.X - lockedText.Width / 2, pos.Y - lockedText.Height / 2));
                        continue;
                    }
                    drawingContext.DrawEllipse(null, !gate.IsVisible ? RenderConstants.NeutralOpaquePen : gate.FactionId == 1 ?
                        RenderConstants.MMOPen : gate.FactionId == 2 ?
                        RenderConstants.EICPen : gate.FactionId == 3 ?
                        RenderConstants.VRUPen : RenderConstants.NeutralPen, pos, 0.3, 0.3);
                }

                foreach (Npc npc in npcs)
                {
                    pos = this.Scale(npc.GetMotherShip.Position);
                    drawingContext.DrawEllipse(RenderConstants.NpcBrush,
                        npc == this.api.Logic.Target ? npc.IsSelected ? RenderConstants.TargetSelectedPen : RenderConstants.TargetUnselectedPen : null,
                        pos,
                        4, 4);
                    if (this.api.Settings.MinimapShowNpcNames)
                    {
                        FormattedText nameText = new(npc.Username, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(nameText, new(pos.X - nameText.Width / 2, pos.Y + nameText.Height - 4));
                    }
                    if (!string.IsNullOrEmpty(npc.Title))
                    {
                        FormattedText titleText = new("T", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(titleText, new(pos.X - titleText.Width / 2, pos.Y - 4));
                    }
                }

                foreach (Player player in players)
                {
                    pos = this.Scale(player.X, player.Y);
                    Pen clanRelationPen = player.FactionId == this.api.Hero.FactionId
                        || player.ClanRelation == CommandConstants.CLAN_RELATION_ALLIED
                        || player.ClanRelation == CommandConstants.CLAN_RELATION_NON_AGGRESSION_PACT ?
                        RenderConstants.ClanRelationAllyPen : player.ClanTag == this.api.Hero.ClanTag ?
                        RenderConstants.ClanRelationOwnClanPen : player.ClanRelation == CommandConstants.CLAN_RELATION_AT_WAR ?
                        RenderConstants.ClanRelationWarPen : RenderConstants.PlayerPen;
                    FormattedText factionText = new(player.FactionId == 1 ? "M" : player.FactionId == 2 ? "E" : player.FactionId == 3 ? "V" : "S",
                        CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        RenderConstants.InformationFont, 8,
                        RenderConstants.NeutralBrush, this.pixelsPerDip);

                    SolidColorBrush? groupColor = (this.api.Group != null && this.api.Group.Members.TryGetValue(player.UserId, out DarksideApi.DarkOrbit.Objects.Group.GroupMemberObj? groupMember)) ? new SolidColorBrush(Color.FromArgb((byte)(groupMember.Player.Cloaked ? 96 : 255), groupMember.R, groupMember.G, groupMember.B)) : null;
                    var activeBotIndex = App.IsActiveBot(player.UserId);
                    if (activeBotIndex > -1)
                    {
                        FormattedText activeBotIndexText = new((activeBotIndex + 1).ToString(),
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            RenderConstants.InformationFont, 6,
                            RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(activeBotIndexText, new(pos.X + 5.5, pos.Y - 6));
                    }
                    drawingContext.DrawEllipse(groupColor, null, new(pos.X + 4, pos.Y - 6), 3, 3);

                    drawingContext.DrawEllipse(player.Cloaked ? RenderConstants.PlayerCloakedBrush : RenderConstants.PlayerBrush,
                        clanRelationPen, pos, 5, 5);
                    drawingContext.DrawText(factionText, new(pos.X - factionText.Width / 2, pos.Y - 5));

                    if (this.api.Settings.MinimapShowPlayerNames)
                    {
                        FormattedText nameText = new(player.Username, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(nameText, new(pos.X - nameText.Width / 2, pos.Y + nameText.Height - 4));
                    }
                }

                foreach (Ship ship in ships)
                {
                    pos = this.Scale(ship.X, ship.Y);
                    var stateBuffer = 0.0;

                    if (ship.IsSelected)
                    {
                        TimeSpan actionTime = dateTime - ship.ActionStartTime;
                        var actionTimeStr = $"{actionTime.Minutes:00}:{actionTime.Seconds:00}";
                        var actionTimeText = new FormattedText(actionTimeStr, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(actionTimeText, new(pos.X - actionTimeText.Width / 2, pos.Y - (actionTimeText.Height + 4)));
                        stateBuffer += actionTimeText.Height + 2;
                    }

                    if (ship == this.api.Logic.Target)
                    {
                        this.DrawEnemyHitpointBar(drawingContext, ship);
                        this.DrawEnemyShieldBar(drawingContext, ship);

                        if (!this.api.Settings.IsMinimapPlayerView)
                        {
                            Brush targetTextColor = RenderConstants.NeutralBrush;
                            var targetLabel = ship.Username;

                            if (ship is Player player)
                            {
                                targetTextColor = player.FactionId == 1 ? RenderConstants.MMOBrush : player.FactionId == 2 ? RenderConstants.EICBrush : player.FactionId == 3 ? RenderConstants.VRUBrush : RenderConstants.NeutralBrush;
                                targetLabel = $"[{player.ClanTag}] {ship.Username}";
                            }

                            DrawTargetName(drawingContext, targetLabel);
                        }

                        drawingContext.DrawLine(RenderConstants.TargetDashPen, heroPos, new(pos.X + 2, pos.Y + 2));

                        if (ship.DamageReceived > 0)
                        {
                            var damageLabel = "-" + ship.DamageReceived;
                            var damageText = new FormattedText(damageLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.PlayerBrush, this.pixelsPerDip);
                            drawingContext.DrawText(damageText, new Point(pos.X - damageText.Width / 2, pos.Y - (damageText.Height) - stateBuffer));
                        }
                    }
                }

                foreach (Box box in boxes)
                {
                    pos = this.Scale(box.X, box.Y);
                    drawingContext.DrawRectangle(RenderConstants.BoxBrush,
                        box == this.api.Logic.Target ? RenderConstants.TargetSelectedPen : null,
                        new(pos.X, pos.Y, 3, 3));

                    if (this.api.InvalidCollectibles.Contains(box.Hash))
                        drawingContext.DrawLine(RenderConstants.TargetSelectedPen, pos, new(pos.X + 3, pos.Y + 3));
               
                    if (api.Logic.Target == box)
                    {
                        DrawTargetName(drawingContext, box.DisplayType);
                    }
                }

                foreach (Ore ore in ores)
                {
                    pos = this.Scale(ore.X, ore.Y);
                    drawingContext.DrawRectangle(ore.Type == Ore.OreType.PALLADIUM ? RenderConstants.PalladiumBrush : RenderConstants.OreBrush,
                        ore == this.api.Logic.Target ? RenderConstants.TargetSelectedPen : null,
                        new(pos.X, pos.Y, 2, 2));

                    if (this.api.InvalidCollectibles.Contains(ore.Hash))
                        drawingContext.DrawLine(RenderConstants.TargetSelectedPen, pos, new(pos.X + 2, pos.Y + 2));

                    if (api.Logic.Target == ore)
                    {
                        DrawTargetName(drawingContext, ore.DisplayType);
                    }
                }

                foreach (Pet pet in pets)
                {
                    pos = this.Scale(pet.X, pet.Y);
                    drawingContext.DrawEllipse(pet.IsVisible ? RenderConstants.PetBrush : RenderConstants.PetCloakedBrush, null, pos, 3, 3);
                    if (pet.OwnerId == this.api.Hero.UserId)
                    {
                        var petName = $"{pet.Name}";
                        var petNameText = new FormattedText(petName, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, Brushes.White, this.pixelsPerDip);
                        drawingContext.DrawText(petNameText, new(pos.X - petNameText.Width / 2 + 2, pos.Y - petNameText.Height));
                    }
                }

                foreach (Mine mine in mines)
                {
                    pos = this.Scale(mine.X, mine.Y);
                    this.DrawStringScaled(drawingContext, RenderConstants.MineText, mine.X, mine.Y, 4, RenderConstants.MineFont, RenderConstants.MineBrush);
                }

                if (poi.X > 0 && poi.Y > 0)
                {
                    pos = this.Scale(poi.X, poi.Y);
                    drawingContext.DrawEllipse(RenderConstants.HitpointTranslucentBrush, RenderConstants.HitpointDarkPen, pos, 4, 4);
                }

                if (this.api.Captcha != null)
                {
                    foreach (Box captchaBox in this.api.Captcha.CaptchaBoxes.Values)
                    {
                        pos = this.Scale(captchaBox.X, captchaBox.Y);
                        drawingContext.DrawRectangle(captchaBox.Type == CommandConstants.BOX_CAPTACHA_RED ? RenderConstants.CaptchaBoxRedBrush : RenderConstants.CaptchaBoxBlackBrush, null, new(pos.X, pos.Y, 3, 3));
                    }
                    var captchaLabel = this.api.Captcha.Amount > 0 ? $"Captcha Boxes Left:\n{this.api.Captcha.Amount}" : $"Waiting: {(this.api.Captcha.WaitTime) / 1000}Sec.";
                    DrawTargetName(drawingContext, captchaLabel);
                }

                Vector2 lastPathElement = this.api.Hero.Path.Count > 1 ? this.api.Hero.Path.Last() : default;
                Point targetPos = lastPathElement != default ? this.Scale(lastPathElement.X, lastPathElement.Y) : this.Scale(this.api.Hero.TargetX, this.api.Hero.TargetY);
                var canvasHeightFifth = (this.canvasHeight - 5) / 5;

                if (this.lastPositions.Count > 0)
                {
                    Point lastPos = this.lastPositions.First();
                    for (var i = 1; i < this.lastPositions.Count; i++)
                    {
                        if (lastPos.X < 0 && lastPos.Y < 0)
                        {
                            if (i + 1 >= this.lastPositions.Count)
                            {
                                break;
                            }
                            lastPos = this.lastPositions[i + 1];
                            continue;
                        }
                        if (this.lastPositions[i].X < 0 && this.lastPositions[i].Y < 0)
                        {
                            continue;
                        }
                        drawingContext.DrawLine(RenderConstants.PathDashPen, this.Scale(lastPos), this.Scale(this.lastPositions[i]));
                        lastPos = this.lastPositions[i];
                    }
                    drawingContext.DrawLine(RenderConstants.PathDashPen, this.Scale(this.lastPositions.Last()), heroPos);
                }

                if (this.api.Hero.IsFlying)
                {
                    drawingContext.DrawLine(RenderConstants.MovementPen, heroPos, targetPos);
                    if (this.api.Logic.Target == null || this.api.Logic.Target is Collectible)
                    {
                        var timespan = TimeSpan.FromMilliseconds(this.api.Hero.FlyingTimeMs);
                        //var v = (targetPos.ToVector2() - heroPos.ToVector2()) / 2 + heroPos.ToVector2();
                        var timeText = new FormattedText($"{timespan.Seconds}:{Math.Round(timespan.Milliseconds / 10d, 0)}",
                            CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                            RenderConstants.InformationFont, 8,
                            RenderConstants.NeutralBrush, this.pixelsPerDip);
                        drawingContext.DrawText(timeText, new(targetPos.X + 2, targetPos.Y));
                    }
                    if (api.Hero.Path.Count > 1)
                    {
                        lastPathElement = api.Hero.Position;
                        foreach (var pathPos in api.Hero.Path)
                        {
                            var pathPoint = this.Scale(pathPos);
                            drawingContext.DrawLine(RenderConstants.MovementPathPen, this.Scale(lastPathElement), pathPoint);
                            lastPathElement = pathPos;
                        }
                    }
                }

                drawingContext.DrawEllipse(this.api.Hero.IsCloaked ? RenderConstants.HeroCloakedBrush : RenderConstants.HeroBrush, null, heroPos, 5, 5);

                this.DrawHeroStats(drawingContext, heroPos);
                this.DrawUsername(drawingContext);
                this.DrawHitpointBar(drawingContext);
                this.DrawShieldBar(drawingContext);
                this.DrawNanoShieldBar(drawingContext);

                this.DrawEnemyInfo(drawingContext, this.api.Logic.Target);

                this.DrawPrimaryInfo(drawingContext);
                this.DrawSecondaryInfo(drawingContext);

                if (this.api.Settings.IsRunning)
                {
                    TimeSpan totalTimeSpan = dateTime - this.api.Logic.TotalRunTime;
                    TimeSpan currentTimeSpan = dateTime - this.api.Logic.StartTime;
                    var runningText = $"{Math.Floor(totalTimeSpan.TotalHours):00}:{totalTimeSpan.Minutes:00}:{totalTimeSpan.Seconds:00}";
                    var pauseLabel = !this.api.Settings.GeneralPauseUsePause ? "No ‖" : $"‖ {Math.Round(this.api.Settings.GeneralPauseAfterPlayTime * 60 - currentTimeSpan.TotalMinutes, 0)}Min";

                    var mapRunningText = new FormattedText(runningText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralOpaqueBrush, this.pixelsPerDip);
                    var mapBotStateText = new FormattedText(RenderConstants.LogicToString(this.api), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralOpaqueBrush, this.pixelsPerDip);

                    Point mapRunningLabelPosition = !this.api.Settings.IsMinimapPlayerView ?
                        new Point(this.canvasWidth / 2 - mapRunningText.Width / 2, this.canvasHeight - (RenderConstants.MapBufferZone))
                        : this.Scale(this.api.Map.Width / 2, (this.api.Map.Height / 2) - 42);
                    Point mapBotStateLabelPosition = !this.api.Settings.IsMinimapPlayerView ?
                        new Point(15, this.canvasHeight - (RenderConstants.MapBufferZone))
                        : this.Scale(this.api.Map.Width / 2, (this.api.Map.Height / 2) - (42 + mapRunningText.Height));

                    drawingContext.DrawText(mapRunningText, new(mapRunningLabelPosition.X, mapRunningLabelPosition.Y - 5));
                    drawingContext.DrawText(mapBotStateText, new(mapBotStateLabelPosition.X, mapBotStateLabelPosition.Y - 5));

                    //this.DrawString(drawingContext, runningText, canvasWidth / 2 - runningText., 1, RenderConstants.NeutralBrush);
                    this.DrawString(drawingContext, pauseLabel, 5, 1, RenderConstants.NeutralBrush, 10);
                }


                if (this.api.Map.EnemysCount > 0)
                {
                    drawingContext.DrawLine(new Pen(Brushes.LightYellow, 2),
                        new Point(1, this.canvasHeight - 1),
                        new(1, this.canvasHeight - canvasHeightFifth * this.api.Map.EnemysCount + (this.api.Map.EnemysCount > 1 ? 1 : 0)));
                }

            IL_DRAW:
                this.DrawSessionValid(drawingContext);
                this.DrawLicense(drawingContext);

                this.DrawForeground(drawingContext);

                // Persist the drawing content.
                drawingContext.Close();

                return drawingVisual;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return new();
        }

        private void DrawForeground(DrawingContext drawingContext)
        {
            if (DateTime.UtcNow.Month != 12)
            {
                return;
            }
            //ICE BERGS
        }

        private void DrawBackground(DrawingContext drawingContext)
        {
            // Create a rectangle and draw it in the DrawingContext.
            Rect rect = new(new Point(0, 0), new Size(this.canvasWidth, this.canvasHeight));
            drawingContext.DrawRectangle(Brushes.Black, null, rect);
            if (!this.api.Settings.MinimapShowBackground)
            {
                return;
            }
            drawingContext.DrawImage(this.mapImage, new(rect.X + 8, rect.Y + 8, rect.Width - 8, rect.Height - (RenderConstants.MapBufferZone + 8)));
        }

        private void DrawStringScaled(DrawingContext drawingContext, string text, double x, double y, int size = 12, Typeface typeface = null!, Brush color = null!)
        {
            typeface ??= RenderConstants.InformationFont;
            color ??= Brushes.White;
            FormattedText formattedText = new(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, typeface, size, color, this.pixelsPerDip);
            drawingContext.DrawText(formattedText, this.Scale(x, y));
        }

        private void DrawStringCenter(DrawingContext drawingContext, string text)
        {
            FormattedText formattedText = new(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(formattedText, new(this.canvasWidth / 2 - formattedText.Width / 2, this.canvasHeight / 2 - formattedText.Height / 2));
        }

        private void DrawString(DrawingContext drawingContext, string text, double x, double y, Brush color, double fontSize = 12)
        {
            FormattedText formattedText = new(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, fontSize, color, this.pixelsPerDip);
            drawingContext.DrawText(formattedText, new(x, y));
        }

        private void DrawLicense(DrawingContext drawingContext)
        {
            if (App.LicenseManager == null)
                return;

            //drawingContext.DrawLine(RenderConstants.LicensePen , new(1, canvasHeight - 1), new((canvasWidth * DarkOrbot.App.LicenseManager.PercentageNotNull) - 1, canvasHeight - 1));
        }

        private void DrawSessionValid(DrawingContext drawingContext)
        {
            if (this.api.GameObservers is null
                || this.api.GameClient is null)
                return;

            //var validLabel = this.api.GameObservers.SessionObserver.IsSessionValid ? RenderConstants.SessionValidText : RenderConstants.SessionInvalidText;
            //Brush validBrush = this.api.GameObservers.SessionObserver.IsSessionValid ? RenderConstants.HeroBrush : RenderConstants.PlayerBrush;
            //var validLabelText = new FormattedText(validLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, validBrush, this.pixelsPerDip);
            //drawingContext.DrawText(validLabelText, new Point(this.canvasWidth - validLabelText.Width - 5, 5 + validLabelText.Height));

            if (this.api.Settings.IsUnity && this.api.GameClient.PingTime <= 0 && this.api.GameClient.GameState == GameState.Ready)
            {
                var unityText = new FormattedText($"Unity", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralBrush, this.pixelsPerDip);
                drawingContext.DrawText(unityText, new Point(this.canvasWidth - unityText.Width - 5, 1));
                return;
            }
            if (this.api.GameClient.PingTime <= 0 || this.api.GameClient.GameState != GameState.Ready)
            {
                return;
            }
            var pingText = new FormattedText($"{Math.Round(this.api.GameClient.PingTime, 0)}Ms", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralBrush, this.pixelsPerDip);
            drawingContext.DrawText(pingText, new Point(this.canvasWidth - pingText.Width - 5, 1));
        }

        private void DrawBorders(DrawingContext drawingContext)
        {
            //drawingContext.DrawRectangle(DrawingConstants.NeutralPen, 1, 1, canvasWidth - 2, canvasHeight - 2);
            var mapLabel = CommandConstants.MAP_NAMES.TryGetValue(this.api.Map.Id, out var mapName) ? mapName : this.api.Map.Name;
            var posLabel = this.api.Hero.X / 100 + "/" + this.api.Hero.Y / 100;
            var npcInCorner = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsInCorner || n.IsFleeingToCorner).ToArray().Length;
            var npcLabel = "Npc Left: " + (this.api.Ships.Count - npcInCorner) + (npcInCorner > 0 ? " (" + npcInCorner + ")" : "");

            var mapLabelText = new FormattedText(mapLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 32, RenderConstants.NeutralOpaqueBrush, this.pixelsPerDip);
            var mapPositionText = new FormattedText(this.api.Map.IsGalaxyGate ? npcLabel : posLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralOpaqueBrush, this.pixelsPerDip);
            var mapDiminishingText = new FormattedText("DIMINISHING QUEST ACTIVE", CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NanoshieldBrush, this.pixelsPerDip);

            Point mapBorder = this.Scale(this.api.Map.Width, this.api.Map.Height);
            Point mapLabelPosition = !this.api.Settings.IsMinimapPlayerView ?
                new Point(this.canvasWidth / 2 - mapLabelText.Width / 2, this.canvasHeight / 2 - RenderConstants.MapBufferZone)
                : this.Scale(this.api.Map.Width / 2, this.api.Map.Height / 2);
            Point posLabelPosition = !this.api.Settings.IsMinimapPlayerView ?
                new Point(this.canvasWidth / 2 - mapPositionText.Width / 2, this.canvasHeight / 2 - RenderConstants.MapBufferZone)
                : this.Scale(this.api.Map.Width / 2, this.api.Map.Height / 2);
            Point diminishingLabelPosition = new((posLabelPosition.X - mapDiminishingText.Width / 2) + 4, posLabelPosition.Y + mapLabelText.Height + mapPositionText.Height + 4);

            drawingContext.DrawText(mapLabelText, mapLabelPosition);
            drawingContext.DrawText(mapPositionText, new(posLabelPosition.X, posLabelPosition.Y + mapLabelText.Height));

            if (this.api.DiminishingQuest != null)
                drawingContext.DrawText(mapDiminishingText, diminishingLabelPosition);

            if (!this.api.Settings.IsMinimapPlayerView)
                drawingContext.DrawRectangle(null, RenderConstants.NeutralOpaquePen, new Rect(10, 10, this.canvasWidth - 20, this.canvasHeight - (20 + RenderConstants.MapBufferZone)));
        }

        private void DrawHeroStats(DrawingContext drawingContext, Point heroPos)
        {
            double statBuffer = 0;
            if (this.api.Hero.IsCloaked)
            {
                var cloakedLabel = "Cloaked";
                var cloakedText = new FormattedText(cloakedLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.MineBrush, this.pixelsPerDip);
                drawingContext.DrawText(cloakedText, new(heroPos.X - cloakedText.Width / 2, heroPos.Y - (cloakedText.Height + 4) - statBuffer));
                statBuffer += cloakedText.Height + 2;
            }
            if (this.api.Hero.IsRepairing && this.api.Hero.RepairAmount > 0)
            {
                var repairLabel = "+" + this.api.Hero.RepairAmount;
                var repairText = new FormattedText(repairLabel, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.HeroBrush, this.pixelsPerDip);
                drawingContext.DrawText(repairText, new Point(heroPos.X - repairText.Width / 2, heroPos.Y - (repairText.Height + 4) - statBuffer));
                statBuffer += repairText.Height + 2;
            }
            if (this.api.Hero.IsFrozen)
            {
                drawingContext.DrawRectangle(null, RenderConstants.IcePen, new(heroPos.X - 2, heroPos.Y - 2, 9, 9));
            }
            if (this.api.Hero.IsSaboteurDebuffed)
            {
                var sabouteurText = new FormattedText(RenderConstants.SaboteurDebuff, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 8, RenderConstants.MineBrush, this.pixelsPerDip);
                drawingContext.DrawText(sabouteurText, new Point(heroPos.X - sabouteurText.Width / 2, heroPos.Y - (sabouteurText.Height + 4) - statBuffer));
                statBuffer += sabouteurText.Height + 2;
            }
        }

        public void DrawUsername(DrawingContext drawingContext)
        {
            /*var nameText = new FormattedText(this.api.Hero.Username, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 11, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(nameText, new Point(4, this.canvasHeight - (42 + nameText.Height)));*/
        }

        public void DrawHitpointBar(DrawingContext drawingContext)
        {
            var barHeight = ((this.canvasHeight - 10) * this.api.Hero.HitpointsPercentage / 100);
            drawingContext.DrawRoundedRectangle(RenderConstants.HitpointDarkBrush, RenderConstants.HitpointDarkPen, new Rect(2, 5, 2, this.canvasHeight - 10), 2, 2);
            drawingContext.DrawRoundedRectangle(RenderConstants.HitpointBrush, RenderConstants.HitpointPen, new Rect(2, 5, 2, barHeight < 0 ? 0 : barHeight), 2, 2);
            /*drawingContext.DrawRectangle(RenderConstants.HitpointDarkBrush, RenderConstants.HitpointDarkPen, new Rect(4, this.canvasHeight - 40, 128, 16));
            drawingContext.DrawRectangle(RenderConstants.HitpointBrush, RenderConstants.HitpointPen, new Rect(4, this.canvasHeight - 40, 128 * (this.api.Hero.HitpointsPercentage / 100), 16));
            var hitpoints = this.api.Hero.Hitpoints + "/" + this.api.Hero.MaxHitpoints;
            var hitpointsText = new FormattedText(hitpoints, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(hitpointsText, new Point(64 - hitpointsText.Width / 2, this.canvasHeight - 39));*/
        }

        public void DrawShieldBar(DrawingContext drawingContext)
        {
            if (this.api.Hero.MaxShield <= 0)
            {
                return;
            }
            var barHeight = ((this.canvasHeight - 10) * this.api.Hero.ShieldPercentage / 100);
            drawingContext.DrawRoundedRectangle(RenderConstants.ShieldDarkBrush, RenderConstants.ShieldDarkPen, new Rect(5, 5, 2, this.canvasHeight - 10), 2, 2);
            drawingContext.DrawRoundedRectangle(RenderConstants.ShieldBrush, RenderConstants.ShieldPen, new Rect(5, 5, 2, barHeight < 0 ? 0 : barHeight), 2, 2);
            /*drawingContext.DrawRectangle(RenderConstants.ShieldDarkBrush, RenderConstants.ShieldDarkPen, new Rect(4, this.canvasHeight - 20, 128, 16));
            drawingContext.DrawRectangle(RenderConstants.ShieldBrush, RenderConstants.ShieldPen, new Rect(4, this.canvasHeight - 20, 128 * (this.api.Hero.ShieldPercentage / 100), 16));
            var shield = this.api.Hero.Shield + "/" + this.api.Hero.MaxShield;
            var shieldText = new FormattedText(shield, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(shieldText, new Point(64 - shieldText.Width / 2, this.canvasHeight - 19));*/
        }

        public void DrawNanoShieldBar(DrawingContext drawingContext)
        {
            if (this.api.Hero.NanoShield <= 0)
            {
                return;
            }
            var barHeight = ((this.canvasHeight - 10) * this.api.Hero.NanoShieldPercentage / 100);
            drawingContext.DrawRoundedRectangle(RenderConstants.NanoshieldDarkBrush, RenderConstants.NanoshieldDarkPen, new Rect(8, 5, 2, this.canvasHeight - 10), 2, 2);
            drawingContext.DrawRoundedRectangle(RenderConstants.NanoshieldBrush, RenderConstants.NanoshieldPen, new Rect(8, 5, 2, barHeight < 0 ? 0 : barHeight), 2, 2);
            /*drawingContext.DrawRectangle(RenderConstants.NanoshieldDarkBrush, RenderConstants.NanoshieldDarkPen, new Rect(4, this.canvasHeight - 20, 128, 16));
            drawingContext.DrawRectangle(RenderConstants.NanoshieldBrush, RenderConstants.NanoshieldPen, new Rect(4, this.canvasHeight - 20, 128 * (this.api.Hero.NanoShieldPercentage / 100), 16));
            var shield = this.api.Hero.NanoShield + "/" + this.api.Hero.MaxNanoShield;
            var shieldText = new FormattedText(shield, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(shieldText, new Point(64 - shieldText.Width / 2, this.canvasHeight - 19));*/
        }

        public void DrawEnemyHitpointBar(DrawingContext drawingContext, Ship ship)
        {
            var barHeight = ((this.canvasHeight - 10) * ship.HitpointsPercentage / 100);
            drawingContext.DrawRoundedRectangle(RenderConstants.HitpointDarkBrush, RenderConstants.HitpointDarkPen, new Rect(this.canvasWidth - 2, 5, 2, this.canvasHeight - 10), 2, 2);
            drawingContext.DrawRoundedRectangle(RenderConstants.HitpointBrush, RenderConstants.HitpointPen, new Rect(this.canvasWidth - 2, 5, 2, barHeight < 0 ? 0 : barHeight), 2, 2);
            /*drawingContext.DrawRectangle(RenderConstants.HitpointDarkBrush, RenderConstants.HitpointDarkPen, new Rect(this.canvasWidth - 128 - 5, this.canvasHeight - 40, 128, 16));
            drawingContext.DrawRectangle(RenderConstants.HitpointBrush, RenderConstants.HitpointPen, new Rect(this.canvasWidth - 128 - 5, this.canvasHeight - 40, 128 * (ship.HitpointsPercentage / 100), 16));
            var hitpoints = ship.Hitpoints + "/" + ship.MaxHitpoints;
            var hitpointsText = new FormattedText(hitpoints, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(hitpointsText, new Point(this.canvasWidth - 64 - 5 - hitpointsText.Width / 2, this.canvasHeight - 39));*/
        }

        public void DrawEnemyShieldBar(DrawingContext drawingContext, Ship ship)
        {
            var barHeight = ((this.canvasHeight - 10) * ship.ShieldPercentage / 100);
            drawingContext.DrawRoundedRectangle(RenderConstants.ShieldDarkBrush, RenderConstants.ShieldDarkPen, new Rect(this.canvasWidth - 5, 5, 2, (this.canvasHeight - 10)), 2, 2);
            drawingContext.DrawRoundedRectangle(RenderConstants.ShieldBrush, RenderConstants.ShieldPen, new Rect(this.canvasWidth - 5, 5, 2, barHeight < 0 ? 0 : barHeight), 2, 2);
            /*drawingContext.DrawRectangle(RenderConstants.ShieldDarkBrush, RenderConstants.ShieldDarkPen, new Rect(this.canvasWidth - 128 - 5, this.canvasHeight - 20, 128, 16));
            drawingContext.DrawRectangle(RenderConstants.ShieldBrush, RenderConstants.ShieldPen, new Rect(this.canvasWidth - 128 - 5, this.canvasHeight - 20, 128 * (ship.ShieldPercentage / 100), 16));
            var shield = ship.Shield + "/" + ship.MaxShield;
            var shieldText = new FormattedText(shield, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 12, Brushes.White, this.pixelsPerDip);
            drawingContext.DrawText(shieldText, new Point(this.canvasWidth - 64 - 5 - shieldText.Width / 2, this.canvasHeight - 19));*/
        }

        public void DrawTargetName(DrawingContext drawingContext, string name)
        {
            var targetLabelText = new FormattedText(name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, RenderConstants.InformationFont, 10, RenderConstants.NeutralOpaqueBrush, this.pixelsPerDip);

            Point targetLabelPosition = new Point(this.canvasWidth - 15 - targetLabelText.Width, this.canvasHeight - (RenderConstants.MapBufferZone));

            drawingContext.DrawText(targetLabelText, new(targetLabelPosition.X, targetLabelPosition.Y - 5));
        }

        public void DrawEnemyInfo(DrawingContext drawingContext, IGameObject? gameObject)
        {
            return;
            /*var useLauncher = false;
            var useRocket = false;
            var rsb = string.Empty;
            var sab = string.Empty;
            var minions = string.Empty;
            var rocketCount = this.api.Inventory.Get(this.api.Hero.CurrentRocket.Id);
            var rocketLauncherCount = this.api.Inventory.Get(this.api.Hero.CurrentRocketLauncher.Id);

            if (!CommandConstants.ROCKET_TYPES.TryGetKey(this.api.Hero.CurrentRocket.Id, out var rocketStr))
            {
                rocketStr = this.api.Hero.CurrentRocket.Id;
            }

            if (!CommandConstants.ROCKETLAUNCHER_TYPES.TryGetKey(this.api.Hero.CurrentRocketLauncher.Id, out var rocketLauncherStr))
            {
                rocketLauncherStr = this.api.Hero.CurrentRocketLauncher.Id;
            }

            if (this.api.Map.IsGalaxyGate)
            {
                DarksideApi.DarkOrbit.Settings.GalaxyGateSetting setting = this.api.Logic.GetGalaxyGateSetting(this.api.Map.Name);
                useLauncher = setting.UseRocketLauncher;
                useRocket = setting.UseRockets;
                sab = $"Wave: {this.api.Map.CurrentGGWave}";
                rsb = $"Lifes: {(this.api.GalaxyGateGenerators.TryGetValue(setting.GalaxyGateId, out GalaxyGateGenerator? generatorGalaxy) ? generatorGalaxy.LifesLeft : this.api.Map.CurrentGGLifeLeft)}";
                Npc? gru = this.api.Ships.Values.OfType<Npc>().Where(n => n.IsProtected).FirstOrDefault();
                minions = $"Minions alive: {(gru != null ? this.api.Ships.Values.OfType<Npc>().ToArray().Length - 1 : "-")}";
            }
            else if (gameObject is Npc npc)
            {
                DarksideApi.DarkOrbit.Settings.NpcSetting setting = this.api.Logic.GetNpcSetting(npc);
                var nextRsb = setting.UseRSB ? Math.Round((DateTime.UtcNow - npc.tempRsbLastUsed).TotalSeconds, 2) : -1000;
                nextRsb = nextRsb > 3.2 ? -2000 : nextRsb;
                rsb = $"RSB: {(nextRsb == 0 ? "Active" : nextRsb == -1000 ? "-" : nextRsb == -2000 ? "Ready" : nextRsb)}";
                sab = $"SAB: {(setting.UseSAB ? this.api.Hero.CurrentLaser.Id == CommandConstants.AMMO_SAB_50 ? "Active" : "False" : "-")}";
                useLauncher = setting.UseRocketLauncher;
                useRocket = setting.UseRockets;
                minions = $"{(npc.IsGru ? "Minions alive" : "Priority")}: {(npc.IsGru ? npc.MinionsAlive.ToString() : setting.Priority.ToString())}";
            }
            else if (gameObject is Player player)
            {
                var nextRsb = this.api.Settings.OnAttackedShootbackUseRSB ? Math.Round((DateTime.UtcNow - player.tempRsbLastUsed).TotalSeconds, 2) : -1000;
                nextRsb = nextRsb > 3.2 ? -2000 : nextRsb;
                rsb = $"RSB: {(nextRsb == 0 ? "Active" : nextRsb == -1000 ? "-" : nextRsb == -2000 ? "Ready" : nextRsb)}";
                sab = $"SAB: {(this.api.Settings.OnAttackedShootbackUseSAB ? this.api.Hero.CurrentLaser.Id == CommandConstants.AMMO_SAB_50 ? "Active" : "False" : "-")}";
                useLauncher = this.api.Settings.OnAttackedShootbackUseRocketLauncher;
                useRocket = this.api.Settings.OnAttackedShootbackUseRocket;
                minions = $"Pet alive: {this.api.Pets.Values.Where(p => p.OwnerId == player.UserId).ToArray().Length > 0}";
            }
            else if (gameObject is Box box && box.IsBootyBox || gameObject is Ore ore)
            {
                var collectable = (Collectible)gameObject;
                var collectingTime = (DateTime.UtcNow - collectable.tempStartCollectTime).TotalSeconds;
                var collecting = collectingTime >= 0 && collectingTime < 60 ? Math.Round(collectingTime, 1).ToString() : "False";
                minions = $"Collecting: {collecting}";
            }
            else if (string.IsNullOrEmpty(minions))
            {
                var originalSpeed = Math.Round(this.api.Hero.Speed / 97f * 100f, 0);
                minions = $"Speed: {originalSpeed}";
            }

            var launcherState = useLauncher ? (rocketLauncherCount > 0 ? $"{rocketLauncherStr} ({HelpTools.FormatNumber(rocketLauncherCount)})" : rocketLauncherStr) : "-";
            var rocketState = useRocket ? (rocketCount > 0 ? $"{rocketStr} ({HelpTools.FormatNumber(rocketCount)})" : rocketStr) : "-";

            var rocket = $"Rocket: {rocketState}";
            var launcher = $"Launcher: {launcherState}";
            var distance = $"Distance: {(gameObject != null && gameObject.CurrentDistance != double.MaxValue ? Math.Round(gameObject.CurrentDistance, 2) : "-")}";

            if (!string.IsNullOrEmpty(sab))
                this.DrawString(drawingContext, sab, this.canvasWidth - 128 - 96, this.canvasHeight - 39, RenderConstants.NeutralBrush, 11);
            if (!string.IsNullOrEmpty(rsb))
                this.DrawString(drawingContext, rsb, this.canvasWidth - 128 - 96, this.canvasHeight - 19, RenderConstants.NeutralBrush, 11);
            if (!string.IsNullOrEmpty(minions))
                this.DrawString(drawingContext, minions, 128 + 24, this.canvasHeight - 19, RenderConstants.NeutralBrush, 11);

            this.DrawString(drawingContext, rocket, this.canvasWidth / 2 - 42, this.canvasHeight - 39, RenderConstants.NeutralBrush, 11);
            this.DrawString(drawingContext, launcher, this.canvasWidth / 2 - 42, this.canvasHeight - 19, RenderConstants.NeutralBrush, 11);
            this.DrawString(drawingContext, distance, 128 + 24, this.canvasHeight - 39, RenderConstants.NeutralBrush, 11);*/
        }

        private void DrawPrimaryInfo(DrawingContext drawingContext)
        {
            if (!this.api.Settings.MinimapShowShipInfo)
            {
                return;
            }
            /*if (!CommandConstants.LASER_TYPES.TryGetKey(this.api.Hero.CurrentLaser.Id, out var ammoStr))
            {
                ammoStr = this.api.Hero.CurrentLaser.Id;
            }
            if (!CommandConstants.DRONE_FORMATIONS.TryGetKey(this.api.Hero.DroneFormation, out var formationStr))
            {
                formationStr = this.api.Hero.DroneFormation;
            }
            var shipLabel = "Ship: " + this.api.Hero.Ship;
            var cargoLabel = "Cargo: " + this.api.Hero.Cargo.UsedCargoSpace + "/" + this.api.Hero.Cargo.MaxCargoSpace;
            var cfgLabel = "Config: " + this.api.Hero.Config;
            var droneLabel = "Formation: " + formationStr;
            var ammoLabel = "Ammo: " + ammoStr + (this.api.Inventory.Get(this.api.Hero.CurrentLaser.Id) > 0 ? ($" ({HelpTools.FormatNumber(this.api.Inventory.Get(this.api.Hero.CurrentLaser.Id))})") : "");
            var stateLabel = "State: " + RenderConstants.LogicToString(this.api);
            var taskLabel = this.api.Settings.UseTaskSystem ? $"Task Duration: {Math.Round(this.api.GameObservers.TaskManager.CurrentTask != null ? this.api.GameObservers.TaskManager.CurrentTask.Duration - (DateTime.UtcNow - this.api.GameObservers.TaskManager.CurrentTask.StartTime).TotalMinutes : 0, 0)}Min" : string.Empty;
            var novaLabel = "NOVA: " + Math.Round(this.api.Hero.CaptainEnergyPercent, 0) + "%";
            var lastRow = 68;

            this.DrawString(drawingContext, shipLabel, 4, this.canvasHeight / 2 - 52, RenderConstants.NeutralBrush);
            this.DrawString(drawingContext, ammoLabel, 4, this.canvasHeight / 2 - 32, RenderConstants.NeutralBrush);
            this.DrawString(drawingContext, cargoLabel, 4, this.canvasHeight / 2 - 12, RenderConstants.NeutralBrush);
            this.DrawString(drawingContext, cfgLabel, 4, this.canvasHeight / 2 + 8, RenderConstants.NeutralBrush);
            this.DrawString(drawingContext, droneLabel, 4, this.canvasHeight / 2 + 28, RenderConstants.NeutralBrush);
            this.DrawString(drawingContext, stateLabel, 4, this.canvasHeight / 2 + 48, RenderConstants.NeutralBrush);
            if (this.api.Hero.MaxCaptainEnergy > 0)
            {
                lastRow = 88;
                this.DrawString(drawingContext, novaLabel, 4, this.canvasHeight / 2 + 68, RenderConstants.NeutralBrush);
            }
            if (string.IsNullOrEmpty(taskLabel))
                return;
            this.DrawString(drawingContext, taskLabel, 4, this.canvasHeight / 2 + lastRow, RenderConstants.NeutralBrush);*/
        }

        private void DrawSecondaryInfo(DrawingContext drawingContext)
        {
            if (!this.api.Settings.MinimapShowCurrentInfo)
            {
                return;
            }
            /*if (api.DiminishingQuest != null)
            {
                var quest = api.DiminishingQuest;// api.Quests.FirstOrDefault();
                string boosterTitle = "Diminishing Quest";
                int notificationY = 52;
                DrawString(drawingContext, boosterTitle, canvasWidth - 94, canvasHeight / 2 - notificationY, RenderConstants.NanoshieldBrush);
                foreach (var questTarget in quest.Targets)
                {
                    notificationY -= 20;
                    string questInfo = questTarget.TargetDisplayType + " " + questTarget.AmountDisplay;
                    DrawString(drawingContext, questInfo, canvasWidth - 94, canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                }
            }*/
            /*
            if (this.api.Settings.MinimapShowBoosters && !this.api.Hero.ActiveBoosters.IsEmpty)
            {
                var boosterTitle = "Boosters:";
                var notificationY = this.api.Hero.ActiveBoosters.Count < 5 ? 52 : 52 - ((5 - this.api.Hero.ActiveBoosters.Count) * 20);
                this.DrawString(drawingContext, boosterTitle, this.canvasWidth - 94, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                foreach (KeyValuePair<string, Booster> booster in this.api.Hero.ActiveBoosters)
                {
                    if (booster.Value is PlayerBooster playerBooster)
                    {
                        notificationY -= 20;
                        var boosterInfo = $"{playerBooster.DisplayName()}: {playerBooster.DisplayDuration()}";
                        this.DrawString(drawingContext, boosterInfo, this.canvasWidth - 94, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                        continue;
                    }
                    if (booster.Value is SharedBooster sharedBooster)
                    {
                        notificationY -= 20;
                        var boosterInfo = $"{sharedBooster.DisplayName()}: {sharedBooster.DisplayAmount()}";
                        this.DrawString(drawingContext, boosterInfo, this.canvasWidth - 94, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                        continue;
                    }
                }
            }
            else if (this.api.Settings.MinimapShowBoosters && this.api.Map.IsGalaxyGate && !this.api.Hero.EBGActiveBoosters.IsEmpty)
            {
                var boosterTitle = "EBG-Boosters:";
                var notificationY = this.api.Hero.EBGActiveBoosters.Count < 5 ? 52 : 52 - ((5 - this.api.Hero.EBGActiveBoosters.Count) * 20);
                this.DrawString(drawingContext, boosterTitle, this.canvasWidth - 94, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                foreach (KeyValuePair<DarksideApi.DarkOrbit.Commands.EternalBlackLight.EternalBlacklightBoosterCommand.BoosterType, int> booster in this.api.Hero.EBGActiveBoosters)
                {
                    notificationY -= 20;
                    var boosterInfo = $"{booster.Value}% {booster.Key}";
                    this.DrawString(drawingContext, boosterInfo, this.canvasWidth - 94, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                }
            }
            else if (this.api.Settings.MinimapShowGroup && this.api.Group != null && !this.api.Group.Members.IsEmpty)
            {
                var groupTitle = "Group:";
                var notificationY = this.api.Group.Members.Count < 3 ? 52 : 52 - ((5 - (this.api.Group.Members.Count * 2)) * 20);
                this.DrawString(drawingContext, groupTitle, this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                foreach (DarksideApi.DarkOrbit.Objects.Group.GroupMemberObj groupMember in this.api.Group.Members.Values)
                {
                    notificationY -= 20;
                    this.DrawString(drawingContext, groupMember.Player.Username, this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, new SolidColorBrush(Color.FromArgb((byte)(groupMember.Player.Cloaked ? 96 : 255), groupMember.R, groupMember.G, groupMember.B)));
                    if (groupMember.Player.Username == this.api.Hero.Username)
                    {
                        continue;
                    }
                    notificationY -= 20;
                    this.DrawString(drawingContext, groupMember.GetStatusAsString(), this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, new SolidColorBrush(Color.FromArgb((byte)(groupMember.Player.Cloaked ? 96 : 255), groupMember.R, groupMember.G, groupMember.B)));
                }
            }
            else if (this.api.Settings.MinimapShowQuest
                && (this.api.DiminishingQuest != null || this.api.Quests.Any(q => q.Value.IsActive)))
            {
                DarksideApi.DarkOrbit.Objects.Quest.Quest quest = this.api.DiminishingQuest ?? this.api.Quests.First(q => q.Value.IsActive).Value;
                var questTitle = this.api.DiminishingQuest != null ? "Diminishing-Quest:" : "Quest: ";
                var notificationY = quest.Targets.Count < 3 ? 52 : 52 - ((5 - (quest.Targets.Count * 2)) * 20);
                this.DrawString(drawingContext, questTitle, this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                foreach (DarksideApi.DarkOrbit.Objects.Quest.QuestTargetObj questTarget in quest.Targets)
                {
                    notificationY -= 20;
                    var targetInfo = $"{questTarget.TargetDisplayType}";
                    this.DrawString(drawingContext, targetInfo, this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, questTarget.IsCompleted ? RenderConstants.VRUBrush : RenderConstants.NeutralBrush);
                    if (!questTarget.NeedAmountDisplay)
                    {
                        continue;
                    }
                    notificationY -= 20;
                    var targetAmount = $"> {questTarget.AmountDisplay}";
                    this.DrawString(drawingContext, targetAmount, this.canvasWidth - 128, this.canvasHeight / 2 - notificationY, questTarget.IsCompleted ? RenderConstants.VRUBrush : RenderConstants.NeutralBrush);
                }
            }
            else if (this.api.Hero.IsPetActive)
            {
                var petLabel = "Pet: " + this.api.Hero.Pet.UserId;
                var petFuelLabel = "Fuel: " + Math.Round(this.api.Hero.Pet.FuelPercentage, 1) + "%";
                var petModeLabel = "Gear: " + (CommandConstants.PET_MODES_SHORT.TryGetValue(this.api.Hero.Pet.Gear, out var mode) ? mode : "Unknown");
                var petLevelLabel = "Level: " + this.api.Hero.Pet.Level;
                var petDistanceLabel = "Distance: " + Math.Round(this.api.Hero.Pet.CurrentDistance, 0);
                var petTargetLabel = "Target: " + this.api.Hero.Pet.TargetX / 100 + "|" + this.api.Hero.Pet.TargetY / 100;
                this.DrawString(drawingContext, petLabel, this.canvasWidth - 104, this.canvasHeight / 2 - 52, RenderConstants.NeutralBrush);
                this.DrawString(drawingContext, petFuelLabel, this.canvasWidth - 104, this.canvasHeight / 2 - 32, RenderConstants.NeutralBrush);
                this.DrawString(drawingContext, petModeLabel, this.canvasWidth - 104, this.canvasHeight / 2 - 12, RenderConstants.NeutralBrush);
                this.DrawString(drawingContext, petLevelLabel, this.canvasWidth - 104, this.canvasHeight / 2 + 8, RenderConstants.NeutralBrush);
                this.DrawString(drawingContext, petDistanceLabel, this.canvasWidth - 104, this.canvasHeight / 2 + 28, RenderConstants.NeutralBrush);
                this.DrawString(drawingContext, petTargetLabel, this.canvasWidth - 104, this.canvasHeight / 2 + 48, RenderConstants.NeutralBrush);
            }
            else if (!this.api.Hero.ActiveDispatchers.IsEmpty)
            {
                List<DispatcherObj> dispatchers = [.. this.api.Hero.ActiveDispatchers.Values];
                var dispatcherTitle = "Dispatchers:";
                var notificationY = 52;
                this.DrawString(drawingContext, dispatcherTitle, this.canvasWidth - 86, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                foreach (DispatcherObj dispatcher in dispatchers)
                {
                    notificationY -= 20;
                    var dispatcherInfo = dispatcher.Name + ", " + (100 - Math.Round(dispatcher.DurationPercentage, 0)) + "%";
                    this.DrawString(drawingContext, dispatcherInfo, this.canvasWidth - 86, this.canvasHeight / 2 - notificationY, RenderConstants.NeutralBrush);
                }
            }*/
        }

        public Point Scale(double x, double y)
        {
            if (!this.api.Settings.IsMinimapPlayerView)
            {
                return new(x * RenderConstants.ScalingFactorX, y * RenderConstants.ScalingFactorY);
            }
            Vector2 heroPos = new(this.api.Hero.X, this.api.Hero.Y);
            var outX = x - heroPos.X;
            var outY = y - heroPos.Y;
            outX *= this.zoom;
            outY *= this.zoom;
            return new Point(this.canvasWidth / 2 + outX, this.canvasHeight / 2 + outY);
        }

        public Point Scale(Point point)
        {
            return this.Scale(point.X, point.Y);
        }

        public Point Scale(Vector2 vector)
        {
            return this.Scale(vector.X, vector.Y);
        }

        public Point ReverseScale(int x, int y)
        {
            if (!this.api.Settings.IsMinimapPlayerView)
            {
                return new(x / RenderConstants.ScalingFactorX, y / RenderConstants.ScalingFactorY);
            }
            Vector2 heroPos = new(this.api.Hero.X, this.api.Hero.Y);
            double outX = x + heroPos.X;
            double outY = y + heroPos.Y;
            outX /= this.zoom;
            outY /= this.zoom;
            return new Point(outX, outY + RenderConstants.MapBufferZone + 20);
        }

        public void IncreaseZoom()
        {
            if (this.zoom >= 1)
            {
                this.CheckZoom();
                return;
            }
            this.zoom += 0.1;
            this.CheckZoom();
        }

        public void DecreaseZoom()
        {
            if (this.zoom <= 0)
            {
                this.CheckZoom();
                return;
            }
            this.zoom -= 0.1;
            this.CheckZoom();
        }

        private void CheckZoom()
        {
            if (this.zoom <= 0)
            {
                this.zoom = 0.1;
            }
            if (this.zoom >= 1)
            {
                this.zoom = 1;
            }
        }

        protected override int VisualChildrenCount => this.visuals.Count;

        // Provide a required override for the GetVisualChild method.
        protected override Visual GetVisualChild(int index)
        {
            return index < 0 || index >= this.visuals.Count ? throw new() : this.visuals[index];
        }
    }
}
