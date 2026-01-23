namespace DarksideApi.Licensing
{
    public class InjectedKeyReviewer
    {
        const int FIRST_SEGMENT_SUM = 24;

        const int SECOND_SEGMENT_DIVIDER = 4;
        const int SECOND_SEGMENT_SUM_DIVIDE = 5;

        const int THIRD_SEGMENT_SUM_SQRT = 6;

        const int FOURTH_SEGMENT_MODULO = 10;
        const int FOURTH_SEGMENT_SUM_MOD = 6;

        const int CHECK_SUM = 41 % 42;

        public static bool Review(string injectedKey)
        {
            try
            {
                var segments = injectedKey.Split('-');

                return (CheckSum() && CheckSegmentOne(segments[0]) && CheckSegmentFour(segments[3]) && CheckSegmentTwo(segments[1]) && CheckSegmentThree(segments[2]));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static int[] GetSegmentAsArray(string segment)
        {
            List<int> keys = [];
            foreach (var key in segment)
            {
                var value = int.Parse(key.ToString());
                keys.Add(value);
            }
            return [.. keys];
        }

        private static bool CheckSum()
        {
            return (FIRST_SEGMENT_SUM + SECOND_SEGMENT_SUM_DIVIDE + THIRD_SEGMENT_SUM_SQRT + FOURTH_SEGMENT_SUM_MOD) == CHECK_SUM;
        }

        private static bool CheckSegmentOne(string segment)
        {
            return GetSegmentAsArray(segment).Sum() == FIRST_SEGMENT_SUM;
        }

        private static bool CheckSegmentTwo(string segment)
        {
            return (GetSegmentAsArray(segment).Sum() / SECOND_SEGMENT_DIVIDER) == SECOND_SEGMENT_SUM_DIVIDE;
        }

        private static bool CheckSegmentThree(string segment)
        {
            return ((int)Math.Sqrt(GetSegmentAsArray(segment).Sum())) == THIRD_SEGMENT_SUM_SQRT;
        }

        private static bool CheckSegmentFour(string segment)
        {
            return (GetSegmentAsArray(segment).Sum() % FOURTH_SEGMENT_MODULO) == FOURTH_SEGMENT_SUM_MOD;
        }
    }
}
