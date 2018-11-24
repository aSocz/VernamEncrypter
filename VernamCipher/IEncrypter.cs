namespace VernamCipher
{
    public interface IEncrypter
    {
        byte[] Encrypt(byte[] message, byte[] key);
        byte[] Decrypt(byte[] cipher, byte[] key);
    }
}