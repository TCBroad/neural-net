namespace NeuralNet.Core
{
    using System;
    using System.Security.Cryptography;

    public static class CryptoRandom
    {
        public static double Next()
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var bytes = new byte[8];
                random.GetBytes(bytes);

                var unsigned = BitConverter.ToUInt64(bytes, 0) / (1 << 11);

                return unsigned / (double)(1ul << 53);
            }
        }
    }
}