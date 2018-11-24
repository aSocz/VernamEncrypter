using System;
using System.Collections;
using System.Security.Cryptography;

namespace VernamCipher
{
    public static class Extensions
    {
        // taken from https://stackoverflow.com/questions/42805544/generate-a-random-biginteger-between-two-values-c-sharp
        public static BigInteger GetRandom(RNGCryptoServiceProvider rng, BigInteger min, BigInteger max)
        {
            // shift to 0...max-min
            var max2 = max - min;

            var bits = max2.bitCount();

            // 1 bit for sign (that we will ignore, we only want positive numbers!)
            bits++;

            // we round to the next byte
            var bytes = (bits + 7) / 8;

            var uselessBits = bytes * 8 - bits;

            var bytes2 = new byte[bytes];

            while (true)
            {
                rng.GetBytes(bytes2);

                // The maximum number of useless bits is 1 (sign) + 7 (rounding) == 8
                if (uselessBits == 8)
                {
                    // and it is exactly one byte!
                    bytes2[0] = 0;
                }
                else
                {
                    // Remove the sign and the useless bits
                    for (var i = 0; i < uselessBits; i++)
                    {
                        //Equivalent to
                        //byte bit = (byte)(1 << (7 - (i % 8)));
                        var bit = (byte)(1 << (7 & ~i));

                        //Equivalent to
                        //bytes2[i / 8] &= (byte)~bit;
                        bytes2[i >> 3] &= (byte)~bit;
                    }
                }

                var bi = new BigInteger(bytes2);

                // If it is too much big, then retry!
                if (bi >= max2)
                {
                    continue;
                }

                // unshift the number
                bi += min;
                return bi;
            }
        }

        public static byte[] ToBytes(this BitArray bits)
        {
            var result = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(result, 0);

            return result;
        }

        public static byte ToByte(this BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }

            var bytes = new byte[1];
            bits.CopyTo(bytes, 0);

            return bytes[0];
        }
    }
}