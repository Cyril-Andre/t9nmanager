using System;
using System.Linq;
using System.Security.Cryptography;
namespace Security
{
    /// <summary>
    /// Salted password hashing with PBKDF2-SHA1.
    /// Compatibility: .NET 3.0 and later.
    /// </summary>
    /// <remarks>See http://crackstation.net/hashing-security.htm for much more on password hashing.</remarks>
    public static class PasswordHashProvider
    {
        /// <summary>
        /// The salt byte size, 64 length ensures safety but could be increased / decreased
        /// </summary>
        private const int SaltByteSize = 64;
        /// <summary>
        /// The hash byte size, 
        /// </summary>
        private const int HashByteSize = 64;
        /// <summary>
        ///  High iteration count is less likely to be cracked
        /// </summary>
        private const int Pbkdf2Iterations = 10000;

        /// <summary>
        /// Creates a salted PBKDF2 hash of the password.
        /// </summary>
        /// <remarks>
        /// The salt and the hash have to be persisted side by side for the password. They could be persisted as bytes or as a string using the convenience methods in the next class to convert from byte[] to string and later back again when executing password validation.
        /// </remarks>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hash of the password.</returns>
        public static PasswordHashContainer CreateHash(string password)
        {
            // Generate a random salt
            using (var csprng = new RNGCryptoServiceProvider())
            {
                // create a unique salt for every password hash to prevent rainbow and dictionary based attacks
                var salt = new byte[SaltByteSize];
                csprng.GetBytes(salt);

                // Hash the password and encode the parameters
                var hash = Pbkdf2(password, salt, Pbkdf2Iterations, HashByteSize);

                return new PasswordHashContainer(hash, salt);
            }
        }
        /// <summary>
        /// Recreates a password hash based on the incoming password string and the stored salt
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="salt">The salt existing.</param>
        /// <returns>the generated hash based on the password and salt</returns>
        public static byte[] CreateHash(string password, byte[] salt)
        {
            // Extract the parameters from the hash
            return Pbkdf2(password, salt, Pbkdf2Iterations, HashByteSize);
        }

        /// <summary>
        /// Validates a password given a hash of the correct one.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="salt">The existing stored salt.</param>
        /// <param name="correctHash">The hash of the existing password.</param>
        /// <returns><c>true</c> if the password is correct. <c>false</c> otherwise. </returns>
        public static bool ValidatePassword(string password, byte[] salt, byte[] correctHash)
        {
            // Extract the parameters from the hash
            byte[] testHash = Pbkdf2(password, salt, Pbkdf2Iterations, HashByteSize);
            return CompareHashes(correctHash, testHash);
        }
        /// <summary>
        /// Compares two byte arrays (hashes)
        /// </summary>
        /// <param name="array1">The array1.</param>
        /// <param name="array2">The array2.</param>
        /// <returns><c>true</c> if they are the same, otherwise <c>false</c></returns>
        public static bool CompareHashes(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;
            return !array1.Where((t, i) => t != array2[i]).Any();
        }

        /// <summary>
        /// Computes the PBKDF2-SHA1 hash of a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="iterations">The PBKDF2 iteration count.</param>
        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
        /// <returns>A hash of the password.</returns>
        private static byte[] Pbkdf2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }

    /// <summary>
    /// Container for password hash and salt and iterations.
    /// </summary>
    public sealed class PasswordHashContainer
    {
        /// <summary>
        /// Gets the hashed password.
        /// </summary>
        public byte[] HashedPassword { get; private set; }
        /// <summary>
        /// Gets the salt.
        /// </summary>
        public byte[] Salt { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHashContainer" /> class.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="salt">The salt.</param>
        public PasswordHashContainer(byte[] hashedPassword, byte[] salt)
        {
            this.HashedPassword = hashedPassword;
            this.Salt = salt;
        }
    }

    /// <summary>
    /// Convenience methods for converting between hex strings and byte array.
    /// </summary>
    public static class ByteConverter
    {
        /// <summary>
        /// Converts the hex representation string to an array of bytes
        /// </summary>
        /// <param name="hexedString">The hexed string.</param>
        /// <returns></returns>
        public static byte[] GetHexBytes(string hexedString)
        {
            var bytes = new byte[hexedString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                var strPos = i * 2;
                var chars = hexedString.Substring(strPos, 2);
                bytes[i] = Convert.ToByte(chars, 16);
            }
            return bytes;
        }
        /// <summary>
        /// Gets a hex string representation of the byte array passed in.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        public static string GetHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
        }
    }
}
/*
*Password Hashing With PBKDF2 (http://crackstation.net/hashing-security.htm).
*Copyright(c) 2013, Taylor Hornby
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without 
* modification, are permitted provided that the following conditions are met:
*
*1.Redistributions of source code must retain the above copyright notice,
* this list of conditions and the following disclaimer.
*
* 2. Redistributions in binary form must reproduce the above copyright notice,
* this list of conditions and the following disclaimer in the documentation 
* and/or other materials provided with the distribution.
*
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
* AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
* IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
* ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
* LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
* CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
* SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
* INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
* CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
*ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
* POSSIBILITY OF SUCH DAMAGE.
*/