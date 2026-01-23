using System.Diagnostics;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Settings
{
    public class WorkAreaSetting : Setting
    {
        public int MapId { get; set; }
        public Vector2 CornerTopLeft { get; set; }
        public Vector2 CornerBottomRight { get; set; }

        public bool IsInside(Vector2 position)
        {
            return (position.X >= this.CornerTopLeft.X && position.X <= this.CornerBottomRight.X) &&
                   (position.Y >= this.CornerTopLeft.Y && position.Y <= this.CornerBottomRight.Y);
        }

        public bool IsInside(int x, int y)
        {
            return (x >= this.CornerTopLeft.X && x <= this.CornerBottomRight.X) &&
                   (y >= this.CornerTopLeft.Y && y <= this.CornerBottomRight.Y);
        }

        public bool IsTopLeftSmallerThanBottomRight()
        {
            return (this.CornerTopLeft.X < this.CornerBottomRight.X);
        }

        public bool IsBottomHigherThenTop()
        {
            return (this.CornerBottomRight.Y > this.CornerTopLeft.Y);
        }

        public bool IsReversed()
        {
            return (this.CornerTopLeft.Y < this.CornerBottomRight.Y) && !this.IsTopLeftSmallerThanBottomRight();
        }

        public bool IsOutOfBounds()
        {
            // Out of bounds if either X or Y is out of bounds
            return this.IsXOutOfBound() || this.IsYOutOfBound();
        }

        public bool IsXOutOfBound()
        {
            return (this.CornerTopLeft.X < 0);
        }

        public bool IsYOutOfBound()
        {
            return (this.CornerTopLeft.Y < 0);
        }

        public int GetRandomX()
        {
            int min = (int)Math.Min(this.CornerTopLeft.X, this.CornerBottomRight.X);
            int max = (int)Math.Max(this.CornerTopLeft.X, this.CornerBottomRight.X);
            if (min == max) return min;
            // Random.Shared.Next is exclusive for max, so add 1 to include max
            return Random.Shared.Next(min, max + 1);
        }

        public int GetRandomY()
        {
            int min = (int)Math.Min(this.CornerTopLeft.Y, this.CornerBottomRight.Y);
            int max = (int)Math.Max(this.CornerTopLeft.Y, this.CornerBottomRight.Y);
            if (min == max) return min;
            return Random.Shared.Next(min, max + 1);
        }

        public void SwapTopLeftAndBottomRight()
        {
            Vector2 tempTopLeft = this.CornerTopLeft;

            this.CornerTopLeft = this.CornerBottomRight;
            this.CornerBottomRight = tempTopLeft;

            Debug.WriteLine("Swapped WorkArea Corners! =>");
            Debug.Write("Top Left: " + this.CornerTopLeft + ", ");
            Debug.Write("Bottom Right: " + this.CornerBottomRight + "\n");
        }

        public void Reverse()
        {
            var topLeft = new Vector2(this.CornerTopLeft.X, this.CornerBottomRight.Y);
            var bottomRight = new Vector2(this.CornerBottomRight.X, this.CornerTopLeft.Y);

            this.CornerTopLeft = topLeft;
            this.CornerBottomRight = bottomRight;

            Debug.WriteLine("Reversed WorkArea Corners! =>");
            Debug.Write("Top Left: " + this.CornerTopLeft + ", ");
            Debug.Write("Bottom Right: " + this.CornerBottomRight + "\n");
        }

        public void CheckWorkArea()
        {
            // Fix out-of-bounds for both corners
            if (this.CornerTopLeft.X < 0) this.CornerTopLeft = new(0, this.CornerTopLeft.Y);
            if (this.CornerTopLeft.Y < 0) this.CornerTopLeft = new(this.CornerTopLeft.X, 0);
            if (this.CornerBottomRight.X < 0) this.CornerBottomRight = new(0, this.CornerBottomRight.Y);
            if (this.CornerBottomRight.Y < 0) this.CornerBottomRight = new(this.CornerBottomRight.X, 0);

            // Ensure top left is actually top left and bottom right is bottom right
            if (!this.IsTopLeftSmallerThanBottomRight())
                this.SwapTopLeftAndBottomRight();

            if (this.IsReversed())
                this.Reverse();
        }
    }
}
