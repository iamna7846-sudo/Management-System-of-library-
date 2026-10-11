using System;
using System.IO;

namespace Libraray_manangement_system
{
    public class AuthService
    {
        private readonly string authFile =
        Path.Combine(
        Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData),
        "Libraray_manangement_system",
        "admin.dat");


    public bool HasAdmin()
        {
            return File.Exists(authFile);
        }

        public void CreateAdmin(string username, string password)
        {
            if (HasAdmin())
                throw new InvalidOperationException(
                    "Admin account already exists.");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(
                    "Username is required.");

            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < 12)
                throw new ArgumentException(
                    "Password must be at least 12 characters.");

            string folder = Path.GetDirectoryName(authFile)!;
            Directory.CreateDirectory(folder);

            string hash = PasswordHelper.HashPassword(password);

            File.WriteAllLines(authFile, new string[]
            {
            username,
            hash
            });
        }

        public bool Login(string username, string password)
        {
            if (!File.Exists(authFile))
                return false;

            string[] data = File.ReadAllLines(authFile);

            if (data.Length != 2)
                return false;

            if (!string.Equals(
                username,
                data[0],
                StringComparison.OrdinalIgnoreCase))
                return false;

            return PasswordHelper.VerifyPassword(
                password, data[1]);
        }
    }


}
