using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AES.Models
{
    public class MyAES
    {
        // Default Cipher Block Chaining
        private readonly byte[] _iv = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public string Encrypt(string plainText, string password, byte[]? iv = null)
        {
            iv ??= _iv;

            var key = Encoding.UTF8.GetBytes(password);

            var aes = Aes.Create();
            aes.IV = iv;
            aes.Key = key;

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            var inputBytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();

            var encrypted = memoryStream.ToArray();

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string plaintext, string password, byte[]? iv = null)
        {
            iv ??= _iv;

            var key = Encoding.UTF8.GetBytes(password);

            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

            var inputBytes = Convert.FromBase64String(plaintext);
            cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            cryptoStream.FlushFinalBlock();

            var decrypted = memoryStream.ToArray();
  
            return Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
        }
    }
}
