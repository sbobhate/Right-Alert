using System;
using System.Text;
using System.Security.Cryptography;

namespace HashLibrary
{
    public static class Hasher
    {
        /// <summary>
        /// This method hashes the given text with 
        /// the SHA1CryptoServiceProvider.
        /// </summary>
        /// <param name="text">Text to hash</param>
        /// <returns>Hashed Value</returns>
        public static string HashString(string text)
        {
            // Create an instance of the SHA1 provider
            SHA1 sha = new SHA1CryptoServiceProvider();

            // Compute the hash 
            byte[] hashedData = sha.ComputeHash(Encoding.Unicode.GetBytes(text));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hashedData)
            {
                // Convert each byte to Hex
                stringBuilder.Append(String.Format("{0,2:X2}", b));
            }

            // Return the hashed value
            return stringBuilder.ToString();
        }
    }
}