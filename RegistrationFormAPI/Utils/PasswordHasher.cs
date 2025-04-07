using System;
using System.Security.Cryptography;
using System.Text;

namespace RegistrationFormAPI.Utils
{
    
    /// Utility class for hashing passwords using SHA256
   
    public static class PasswordHasher
    {
        
        /// Computes the SHA256 hash of a password
       
        /// <param name="password">The plain text password to hash</param>
        /// <returns>A SHA256 hash of the password as a hexadecimal string</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

      
        /// Verifies if a plain text password matches a stored hash
    
        /// <param name="password">The plain text password to verify</param>
        /// <param name="storedHash">The stored hash to compare against</param>
        /// <returns>True if the password matches the hash, false otherwise</returns>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            {
                return false;
            }

            string hashedPassword = HashPassword(password);
            return string.Equals(hashedPassword, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}