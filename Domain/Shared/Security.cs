using System.Security.Cryptography;
using System.Text;

namespace Domain.Shared
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

        //private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        //{
        //    byte[] encryptedBytes = null;

        //    var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (RijndaelManaged AES = new RijndaelManaged())
        //        {
        //            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

        //            AES.KeySize = 256;
        //            AES.BlockSize = 128;
        //            AES.Key = key.GetBytes(AES.KeySize / 8);
        //            AES.IV = key.GetBytes(AES.BlockSize / 8);

        //            AES.Mode = CipherMode.CBC;

        //            using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        //                cs.Close();
        //            }

        //            encryptedBytes = ms.ToArray();
        //        }
        //    }

        //    return encryptedBytes;
        //}

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            ArgumentNullException.ThrowIfNull(bytesToBeEncrypted);
            ArgumentNullException.ThrowIfNull(passwordBytes);

            byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8 };

            const int keySize = 32; // 256 bits
            const int ivSize = 16;  // 128 bits

            byte[] derivedBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: passwordBytes,
                salt: saltBytes,
                iterations: 1000,
                hashAlgorithm: HashAlgorithmName.SHA1,
                outputLength: keySize + ivSize);

            byte[] key = derivedBytes[..keySize];
            byte[] iv = derivedBytes[keySize..];

            using MemoryStream ms = new();

            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(bytesToBeEncrypted);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        //private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        //{
        //    byte[] decryptedBytes = null;
        //    var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (RijndaelManaged AES = new RijndaelManaged())
        //        {
        //            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

        //            AES.KeySize = 256;
        //            AES.BlockSize = 128;
        //            AES.Key = key.GetBytes(AES.KeySize / 8);
        //            AES.IV = key.GetBytes(AES.BlockSize / 8);
        //            AES.Mode = CipherMode.CBC;

        //            using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
        //                cs.Close();
        //            }

        //            decryptedBytes = ms.ToArray();
        //        }
        //    }

        //    return decryptedBytes;
        //}

        private static byte[] Decrypt(
            byte[] bytesToBeDecrypted,
            byte[] passwordBytes)
        {
            ArgumentNullException.ThrowIfNull(bytesToBeDecrypted);
            ArgumentNullException.ThrowIfNull(passwordBytes);

            byte[] saltBytes = { 1, 2, 3, 4, 5, 6, 7, 8 };

            const int keySizeInBytes = 32; // 256 bits
            const int ivSizeInBytes = 16;  // 128 bits

            byte[] derivedBytes = Rfc2898DeriveBytes.Pbkdf2(
                password: passwordBytes,
                salt: saltBytes,
                iterations: 1000,
                hashAlgorithm: HashAlgorithmName.SHA1,
                outputLength: keySizeInBytes + ivSizeInBytes);

            byte[] key = derivedBytes[..keySizeInBytes];
            byte[] iv = derivedBytes[keySizeInBytes..];

            try
            {
                using Aes aes = Aes.Create();

                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using MemoryStream ms = new();

                using (CryptoStream cs = new(
                    ms,
                    aes.CreateDecryptor(),
                    CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeDecrypted);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
            finally
            {
                CryptographicOperations.ZeroMemory(derivedBytes);
                CryptographicOperations.ZeroMemory(key);
                CryptographicOperations.ZeroMemory(iv);
            }
        }
    }
}