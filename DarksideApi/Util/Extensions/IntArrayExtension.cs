using System.Numerics;

namespace DarksideApi.Util.Extensions
{
    public static class IntArrayExtension
    {
        public static Vector2 ToVector2(this int[] array)
        {
            return array.Length > 2
                ? throw new FormatException("Array Length was larger than maximum Vector dimension!")
                : new(array[0], array[1]);
        }
    }
}
