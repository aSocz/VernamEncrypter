namespace VernamCipher
{
    public interface IKeyGenerator
    {
        byte[] GenerateKey(int bytesLength);
    }
}