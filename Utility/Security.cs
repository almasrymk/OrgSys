using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utility
{
    public static class Security
    {
        private readonly static string PASSWORD = "OrgSys";

        public static string Encrypt(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(PASSWORD);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            var bytesEncrypted = Security.Encrypt(bytesToBeEncrypted, passwordBytes);
            return Convert.ToBase64String(bytesEncrypted);
        }

        public static string Decrypt(string encryptedText)
        {
            if (encryptedText == null)
            {
                return null;
            }

            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(PASSWORD);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
            var bytesDecrypted = Security.Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes;

            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (var ms = new MemoryStream())
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // ✅ اشتقاق Key
                byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                    passwordBytes,
                    saltBytes,
                    100000,
                    HashAlgorithmName.SHA256,
                    32
                );

                aes.Key = key;

                // ✅ IV عشوائي (أفضل من اشتقاقه)
                aes.GenerateIV();
                byte[] iv = aes.IV;

                // نكتب IV في بداية الناتج (عشان نستخدمه وقت فك التشفير)
                ms.Write(iv, 0, iv.Length);

                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                }

                encryptedBytes = ms.ToArray();
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // ✅ اشتقاق نفس الـ Key
                byte[] key = Rfc2898DeriveBytes.Pbkdf2(
                    passwordBytes,
                    saltBytes,
                    100000,
                    HashAlgorithmName.SHA256,
                    32
                );

                aes.Key = key;

                // ✅ قراءة IV من أول 16 بايت
                byte[] iv = new byte[16];
                byte[] cipherBytes = new byte[bytesToBeDecrypted.Length - 16];

                Array.Copy(bytesToBeDecrypted, 0, iv, 0, 16);
                Array.Copy(bytesToBeDecrypted, 16, cipherBytes, 0, cipherBytes.Length);

                aes.IV = iv;

                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.FlushFinalBlock();

                    return ms.ToArray();
                }
            }
        }
    }
}