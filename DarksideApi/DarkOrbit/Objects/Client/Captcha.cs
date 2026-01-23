using DarksideApi.DarkOrbit.Commands;
using DarksideApi.DarkOrbit.Objects.Boxes;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Client
{
    public class Captcha : IGameObject
    {
        private const string BoxHash = "0123456789";
        private const double Circle = Math.PI / 180;

        private readonly Api api;

        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);

        public double CurrentDistance => 0;
        public double TargetDistance => 0;
        public double PathDistance => 0;

        public bool HasApi => this.api != null;

        public int Amount { get; set; }
        public int WaitTime { get; private set; }
        public string BoxToCollect { get; private set; }
        public double Radius { get; private set; }
        public Dictionary<int, Box> CaptchaBoxes { get; private set; }

        public Captcha(Api api, int amount, int waitTime, int type, int x, int y, double radius, int blackBoxesCount, int redBoxesCount)
        {
            this.api = api;
            this.CaptchaBoxes = [];

            this.Amount = amount;
            this.X = x;
            this.Y = y;
            this.Radius = radius;
            this.WaitTime = waitTime * 1000;

            this.BoxToCollect = type switch
            {
                0 or 1 => CommandConstants.BOX_CAPTACHA_BLACK,
                2 or 3 => CommandConstants.BOX_CAPTACHA_RED,
                _ => string.Empty,
            };

            this.GenerateCaptcha(blackBoxesCount, redBoxesCount);
            api.Logging.Log($"Captcha Generated will collect {this.Amount}x {this.BoxToCollect}");
        }

        //param1.var_261,param1.var_4812
        private void GenerateCaptcha(int blackBoxesCount, int redBoxesCount)
        {
            double totalBoxesCount = blackBoxesCount + redBoxesCount;
            var circlePosition = Random.Shared.NextDouble() * 360 * Circle;
            var circlePositionParts = 360 / totalBoxesCount * Circle;
            var angleAdd = (Random.Shared.NextDouble() / 3 + 0.6) * this.Radius;
            var boxIndexerBlack = 0;
            var boxIndexerRed = 0;
            Box captchaBox;
            var i = 0;
            while (i < totalBoxesCount)
            {
                var boxTypeGen = Math.Floor(Random.Shared.NextDouble() * (blackBoxesCount + redBoxesCount));
                var boxX = this.X + Math.Sin(circlePosition) * angleAdd;
                var boxY = this.Y + Math.Cos(circlePosition) * angleAdd;
                string boxType;
                int captchaBoxIndex;
                if ((boxType = boxTypeGen < blackBoxesCount ? CommandConstants.BOX_CAPTACHA_BLACK : CommandConstants.BOX_CAPTACHA_RED) == CommandConstants.BOX_CAPTACHA_BLACK)
                {
                    blackBoxesCount--;
                    captchaBoxIndex = ++boxIndexerBlack - 1;
                }
                else
                {
                    redBoxesCount--;
                    boxIndexerRed++;
                    captchaBoxIndex = (int)totalBoxesCount - boxIndexerRed;
                }
                //this.CaptchaBoxes.Add(new Box("0123456789", boxX, boxY, true, _loc10_, _loc15_));
                captchaBox = new(this.api, BoxHash, (int)boxX, (int)boxY, boxType, captchaBoxIndex);
                this.api.Logging.Log($"Generated Box: {captchaBox.Hash}, {captchaBox.X}|{captchaBox.Y}, {captchaBox.Type}, {captchaBox.CaptchaIndex}");
                this.CaptchaBoxes.Add(captchaBoxIndex, captchaBox);
                circlePosition += circlePositionParts;
                i++;
            }
        }

        public void CollectBox(int index)
        {
            this.Amount -= 1;
            this.CaptchaBoxes.Remove(index);
        }
    }
}
