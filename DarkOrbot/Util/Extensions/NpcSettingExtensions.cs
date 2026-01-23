using DarksideApi.DarkOrbit.Settings;
using System.Windows.Media;

namespace DarkOrbot.Util.Extensions
{
    public static class NpcSettingExtensions
    {
        public static ImageSource GetSettingsImage(this NpcSetting npcSetting)
        {
            /*List<BitmapImage> bitmaps = [];
            if (npcSetting.)
            {
                bitmaps.Add(Properties.Resources.npcListAddPowder);
            }
            if (plates)
            {
                bitmaps.Add(Properties.Resources.npcListAddPlates);
            }
            if (board)
            {
                bitmaps.Add(Properties.Resources.npcListAddBoard);
            }
            if (superArmor)
            {
                bitmaps.Add(Properties.Resources.npcListAddSuperPlates);
            }
            if (lionsBulwark)
            {
                bitmaps.Add(Properties.Resources.npcListAddLionsBulwark);
            }
            if (dragonPowder)
            {
                bitmaps.Add(Properties.Resources.npcListAddPowder); //TODO ADD IMG
            }
            if (hailstorm)
            {
                bitmaps.Add(Properties.Resources.npcListAddHailstorm);
            }
            if (windstorm)
            {
                bitmaps.Add(Properties.Resources.npcListAddWindstorm);
            }
            if (snowman)
            {
                bitmaps.Add(Properties.Resources.npcListAddSnowman);
            }
            if (candle)
            {
                bitmaps.Add(Properties.Resources.npcListAddCandle);
            }
            if (winterLight)
            {
                bitmaps.Add(Properties.Resources.npcListAddWinterLight);
            }
            if (lostLove)
            {
                bitmaps.Add(Properties.Resources.npcListAddLostLove);
            }
            if (skyFire)
            {
                bitmaps.Add(Properties.Resources.npcListAddSkyfire);
            }
            if (elmosFire)
            {
                bitmaps.Add(Properties.Resources.npcListAddElmosfire);
            }
            if (bloodLust)
            {
                bitmaps.Add(Properties.Resources.npcListAddBloodlust);
            }
            if (eyeOfPrecision)
            {
                bitmaps.Add(Properties.Resources.npcListAddWinterLightEyeOfPrec);
            }
            if (homingMine)
            {
                bitmaps.Add(Properties.Resources.npcListAddMine);
            }
            if (dragonFire)
            {
                bitmaps.Add(Properties.Resources.npcListAddSkyfire);    //TODO ADD IMG
            }
            if (bastionSentry)
            {
                bitmaps.Add(Properties.Resources.npcListAddSentry);
            }
            if (speedStone)
            {
                bitmaps.Add(Properties.Resources.npcListAddSpeedstone);
            }
            if (vpItem > 0)
            {
                bitmaps.Add(Properties.Resources.npcListAddVpItem);
            }
            if (hpItem > 0)
            {
                bitmaps.Add(Properties.Resources.npcListAddHpItem);
            }
            int height = 19 + (19 * (bitmaps.Count / 9)) + 4;
            int width = (38 * bitmaps.Count);
            Bitmap result = new Bitmap(width > 0 ? width : 38, height);
            int i = 0;
            int y = 0;
            int x = 0;
        using (Graphics g = Graphics.FromImage(result))
        {
            foreach (var bitmap in bitmaps)
            {
                g.DrawImage(bitmap, x, y, 19, 19);
                x += (19 + 4);
                if (i++ > 8)
                {
                    i = 0;
                    x = 0;
                    y += (19 + 4);
                }
            }
            var backgroundBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            var font = new Font("Arial", 10, FontStyle.Bold);
            if (minHpAmount > 0)
            {
                g.FillRectangle(backgroundBrush, x, y, 19, 19);
                g.DrawString(minHpAmount.ToString(), font, new System.Drawing.SolidBrush(System.Drawing.Color.Red), new PointF(x, y));
                x += (19 + 4);
                if (i++ > 8)
                {
                    i = 0;
                    x = 0;
                    y += (19 + 4);
                }
            }
            if (maxHpAmount > 0)
            {
                g.FillRectangle(backgroundBrush, x, y, 19, 19);
                g.DrawString(maxHpAmount.ToString(), font, new System.Drawing.SolidBrush(System.Drawing.Color.Green), new PointF(x, y));
                x += (19 + 4);
                if (i++ > 8)
                {
                    i = 0;
                    x = 0;
                    y += (19 + 4);
                }
            }
            if (hasCustomRange)
            {
                g.FillRectangle(backgroundBrush, x, y, 19, 19);
                g.DrawString(range.ToString(), font, new System.Drawing.SolidBrush(System.Drawing.Color.White), new PointF(x, y));
                x += (19 + 4);
                if (i++ > 8)
                {
                    i = 0;
                    x = 0;
                    y += (19 + 4);
                }
            }
            return HelpTools.ImageSourceFromBitmap(result);*/
            return null!;
        }
    }
}
