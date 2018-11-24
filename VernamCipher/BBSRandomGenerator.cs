using System.Collections;
using System.Linq;
using System.Security.Cryptography;

namespace VernamCipher
{
    public class BBSRandomGenerator : IKeyGenerator
    {
        private const int Confidence = 3000;
        public const int ByteLength = 8;
        private readonly BigInteger n;

        private readonly RNGCryptoServiceProvider rand;
        private BigInteger seed;


        public BBSRandomGenerator()
        {
            rand = new RNGCryptoServiceProvider();

            var p = GetProbablePrime();
            var q = GetProbablePrime();
            n = p * q;

            SetSeed();
        }

        public byte[] GenerateKey(int bytesLength)
        {
            var result = new byte[bytesLength];
            var x = seed * seed % n;

            for (var i = 0; i < bytesLength; i++)
            {
                var nextByte = GetNextByte(ref x);
                result[i] = nextByte;
            }

            return result;
        }

        private void SetSeed()
        {
            var minValue = new BigInteger(2);

            do
            {
                seed = Extensions.GetRandom(rand, minValue, n);
            } while (seed.gcd(n) != 1);
        }

        private byte GetNextByte(ref BigInteger x)
        {
            var bitsArray = new BitArray(ByteLength);
            for (var i = 0; i < ByteLength; i++)
            {
                x = x * x % n;
                var bit = GetParity(x);
                bitsArray.Set(i, bit);
            }

            return bitsArray.ToByte();
        }

        private static bool GetParity(BigInteger number)
        {
            var bytes = number.getBytes();
            var bits = new BitArray(bytes);
            var count = GetSetBitsCount(bits);

            return count % 2 == 1;
        }

        private static int GetSetBitsCount(IEnumerable bits)
        {
            return bits.Cast<bool>().Aggregate(0, (count, bit) => bit ? count : ++count);
        }

        private BigInteger GetProbablePrime(int bits = 512)
        {
            BigInteger prime;
            do
            {
                prime = BigInteger.genPseudoPrime(bits, Confidence, rand);
            } while (prime % 4 != 3);

            return prime;
        }
    }
}