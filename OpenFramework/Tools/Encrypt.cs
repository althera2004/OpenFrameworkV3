// --------------------------------
// <copyright file="Encrypt.cs" company="OpenFramework">
//     Copyright (c) 2013 - OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.cat</author>
// --------------------------------
namespace OpenFrameworkV3.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Encrypt
    {
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "openframework219";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;

        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            var symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            byte[] cipherTextBytes = new List<byte>().ToArray();
            using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(cipherTextBytes);
        }

        // Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };

            byte[] plainTextBytes = new List<byte>().ToArray();
            int decryptedByteCount = 0;

            using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
            {
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        plainTextBytes = new byte[cipherTextBytes.Length];
                        decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    }
                }
            }

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}