using System;
using System.Collections;
using System.Linq;

namespace VernamCipher
{
    public class VernamEncrypter : IEncrypter
    {
        public byte[] Encrypt(byte[] message, byte[] key)
        {
            return GetBitwiseXor(message, key);
        }

        public byte[] Decrypt(byte[] cipher, byte[] key)
        {
            return GetBitwiseXor(cipher, key);
        }

        private static byte[] GetBitwiseXor(byte[] message, byte[] key)
        {
            if (message == null || !message.Any())
            {
                throw new ArgumentException("Invalid message");
            }

            if (key == null || !key.Any())
            {
                throw new ArgumentException("Invalid key");
            }

            if (message.Length != key.Length)
            {
                throw new ArgumentException("Key has to have same length as message");
            }

            var messageBits = new BitArray(message);
            var keyBits = new BitArray(key);

            var xorResult = messageBits.Xor(keyBits);

            return xorResult.ToBytes();
        }
    }
}