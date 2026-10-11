using System;
using System.Security.Cryptography;

namespace Libraray_manangement_system
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 600000;


    public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return Iterations + "." +
                   Convert.ToBase64String(salt) + "." +
                   Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(
            string password,
            string storedValue)
        {
            try
            {
                string[] parts = storedValue.Split('.');

                if (parts.Length != 3)
                    return false;

                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash =
                    Convert.FromBase64String(parts[2]);

                if (iterations < 100000 ||
                    iterations > 1000000 ||
                    salt.Length != SaltSize ||
                    expectedHash.Length != HashSize)
                    return false;

                byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    HashSize);

                return CryptographicOperations.FixedTimeEquals(
                    actualHash,
                    expectedHash);
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }


}

