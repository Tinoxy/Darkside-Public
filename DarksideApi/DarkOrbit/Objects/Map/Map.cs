using DarksideApi.DarkOrbit.Commands;
using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Map
{
    public class Map : IGameObject
    {
        public int Id { get; private set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);

        public string Name { get; set; }

        public int CurrentGGWave { get; set; }
        public int CurrentGGLifeLeft { get; set; }
        public int EnemysCount { get; set; }
        public Vector2 TargetQuarter { get; set; }

        public int Width { get; private set; } = CommandConstants.MAP_WIDTH_NORMAL;
        public int Height { get; private set; } = CommandConstants.MAP_HEIGHT_NORMAL;
        public int UsableX { get; private set; }
        public int UsableY { get; private set; }
        public int UsableWidth { get; private set; }
        public int UsableHeight { get; private set; }
        public Vector2 CornerTopLeft { get; private set; }
        public Vector2 CornerTopRight { get; private set; }
        public Vector2 CornerBottomLeft { get; private set; }
        public Vector2 CornerBottomRight { get; private set; }
        public List<Vector2> Corners { get; private set; }

        public double CurrentDistance => 0;
        public double TargetDistance => 0;
        public double PathDistance => 0;
        public bool HasApi => true;
        public bool IsEternalBlackLightGatePlaced { get; set; }

        public bool IsHomeMap => this.Id == 1 || this.Id == 5 || this.Id == 9;
        public bool IsPirateMap => this.Id == 91 || this.Id == 92 || this.Id == 93 || this.Id == 94;
        public bool IsPirateTraderMap => this.Id == 92;
        public bool IsPiratePalladiumMap => this.Id == 93;
        public bool IsBlackLightMap => this.Id == 306 || this.Id == 307 || this.Id == 308;
        public bool IsTreachorusDomainMap => this.Id == 471 || this.Id == 472;
        public bool IsFrozenLabyrinth => IsFrozenLabyrinthSeason && (this.Id >= 430 && this.Id <= 445);
        public bool IsMimesisEscort => !IsFrozenLabyrinthSeason && (this.Id >= 430 && this.Id <= 445);
        public bool IsPlutus => (this.Id >= 410 && this.Id <= 415) || (this.Id >= 450 && this.Id <= 455);
        public bool IsGalaxyGate => CommandConstants.GET_GALAXY_GATE_MAP_IDS().Contains(this.Id);
        public bool IsFrozenLabyrinthSeason => DateTime.UtcNow.Month == 12 || DateTime.UtcNow.Month == 1;

        public Map(int mapId, int initX, int initY)
        {
            this.Id = mapId;
            this.Name = CommandConstants.MAP_NAMES.TryGetValue(mapId, out var name) ? name : mapId.ToString();
            this.CornerTopLeft = new();
            this.CornerTopRight = new();
            this.CornerBottomLeft = new();
            this.CornerBottomRight = new();
            this.Corners = [];
            this.TargetQuarter = new();

            this.X = initX;
            this.Y = initY;
            this.UsableX = 0;
            this.UsableY = 0;

            this.SetMapSize(mapId);

            Debug.WriteLineIf(Id > 0, $"Map: {mapId}[{this.Name}] created.");
        }

        public bool IsInMap(Vector2 vector)
        {
            return this.IsInMap((int)vector.X, (int)vector.Y);
        }

        public bool IsInMap(int x, int y)
        {
            return (x > this.UsableX && y > this.UsableY && x < (this.UsableX + this.UsableWidth) && y < (this.UsableY + this.UsableHeight));
        }

        public int GetCornerIndex(Vector2 corner)
        {
            if (corner == this.CornerTopLeft) return 0;
            if (corner == this.CornerTopRight) return 1;
            if (corner == this.CornerBottomLeft) return 2;
            if (corner == this.CornerBottomRight) return 3;

            return -1; // Not found
        }

        private void SetMapSize(int mapId)
        {
            var weight = 1.0f;    //1.0, 0.75, 0.5, 0.25
            switch (mapId)
            {
                case 16:
                case 29:
                case 91:
                case 93:
                case 94:
                case 229:
                    this.Width = CommandConstants.MAP_WIDTH_LARGE;
                    this.Height = CommandConstants.MAP_HEIGHT_LARGE;
                    break;
                case 415:
                case 455:
                case 471:
                case 472:
                    this.Width = CommandConstants.MAP_WIDTH_SMALL;
                    this.Height = CommandConstants.MAP_HEIGHT_SMALL;
                    weight = 0.5f;
                    break;
                default:
                    this.Width = CommandConstants.MAP_WIDTH_NORMAL;
                    this.Height = CommandConstants.MAP_HEIGHT_NORMAL;
                    break;
            }
            var tempH = this.Height;
            var tempW = this.Width;
            if (this.IsFrozenLabyrinth && IsFrozenLabyrinthSeason)
            {
                this.Width = 41500;
                this.Height = 26500;
                tempH = 13100;
                tempW = 21000;
                weight = 1.0f;
                if (this.X > 21000)  //RIGHT SIDE
                {
                    if (this.Y > 13500)  //BOTTOM
                    {
                        this.UsableX = 21500;
                        this.UsableY = 13500;
                    }
                    else  //TOP
                    {
                        this.UsableX = 21500;
                        this.UsableY = 0;
                    }
                }
                else    //LEFT SIDE
                {
                    if (this.Y > 13500)  //BOTTOM
                    {
                        this.UsableX = 0;
                        this.UsableY = 13500;
                    }
                    else  //TOP
                    {
                        this.UsableX = 0;
                        this.UsableY = 0;
                    }
                }
            }

            this.UsableWidth = tempW;
            this.UsableHeight = tempH;

            this.CornerTopLeft = new((int)((this.UsableX + 2000) * weight), (int)((this.UsableY + 2000) * weight));
            this.CornerTopRight = new((this.UsableX + this.UsableWidth) - 2000 * weight, (int)((this.UsableY) + 2000 * weight));
            this.CornerBottomLeft = new((int)((this.UsableX) + 2000 * weight), (this.UsableY + this.UsableHeight) - 2000 * weight);
            this.CornerBottomRight = new((this.UsableX + this.UsableWidth) - 2000 * weight, (this.UsableY + this.UsableHeight) - 2000 * weight);
            this.Corners = [this.CornerTopLeft, this.CornerTopRight, this.CornerBottomLeft, this.CornerBottomRight];
        }

    }
}
