using System.Security.Cryptography;

namespace Diglett.Core.Utilities
{
    public static class CommonHelper
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #region Randomizer

        public static string GenerateRandomDigitCode(int length)
        {
            var buffer = new byte[length];
            for (int i = 0; i < length; ++i)
            {
                buffer[i] = (byte)Random.Shared.Next(10);
            }

            return string.Join(string.Empty, buffer);
        }

        public static string GenerateRandomString(int length = 16)
        {
            var result = new char[length];
            var data = new byte[length];

            RandomNumberGenerator.Fill(data);

            for (int i = 0; i < length; i++)
            {
                result[i] = Chars[data[i] % Chars.Length];
            }

            return new string(result);
        }

        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            return Random.Shared.Next(min, max);
        }

        #endregion
    }
}
