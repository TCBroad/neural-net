namespace NeuralNet
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

                return BitConverter.ToDouble(bytes);
            }
        }
    }
}