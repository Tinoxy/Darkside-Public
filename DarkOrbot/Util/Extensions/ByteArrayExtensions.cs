namespace DarkOrbot.Util.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int FindBytes(this byte[] src, byte[] find)
        {
            var index = -1;
            var matchIndex = 0;
            // handle the complete source array
            for (var i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else
                {
                    matchIndex = src[i] == find[0] ? 1 : 0;
                }

            }
            return index;
        }

        public static byte[]? ReplaceBytes(this byte[] src, byte[] search, byte[] repl)
        {
            byte[]? dst = null;
            byte[]? temp = null;
            var index = src.FindBytes(search);
            while (index >= 0)
            {
                temp = temp == null ? src : dst;

                dst = new byte[temp!.Length - search.Length + repl.Length];

                // before found array
                Buffer.BlockCopy(temp, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    temp,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    temp.Length - (index + search.Length));


                index = dst.FindBytes(search);
            }
            return dst;
        }
    }
}
