using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VernamCipher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IKeyGenerator keyGenerator = new BBSRandomGenerator();
            IEncrypter encrypter = new VernamEncrypter();

            var filePath = GetFilePath();
            var message = await File.ReadAllTextAsync(filePath);

            var messageRaw = Encoding.UTF8.GetBytes(message);
            var key = keyGenerator.GenerateKey(messageRaw.Length);
            var encryptedMessage = encrypter.Encrypt(messageRaw, key);
            var decryptedMessageRaw = encrypter.Decrypt(encryptedMessage, key);
            var decryptedMessage = Encoding.UTF8.GetString(decryptedMessageRaw);

            Console.WriteLine($"Message: {BitConverter.ToString(messageRaw)} \n\n");
            Console.WriteLine($"Key: {BitConverter.ToString(key)} \n\n");
            Console.WriteLine($"Encrypted message: {BitConverter.ToString(encryptedMessage)} \n\n");
            Console.WriteLine($"Decrypted message: {BitConverter.ToString(decryptedMessageRaw)} \n\n");

            Console.WriteLine($"Is decrypted text same as original? {string.Equals(message, decryptedMessage)}");

            Console.ReadKey();
        }

        private static string GetFilePath()
        {
            string filePath;

            do
            {
                Console.WriteLine("Please give file path");
                filePath = Console.ReadLine();
            } while (!File.Exists(filePath));

            return filePath;
        }
    }
}
