using System;
using System.Security.Cryptography;
using System.Text;

namespace Protecta.CrossCuting.Utilities.Security
{
    public class Cryptography
    {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Cryptography.FromBytesToStringHexadecimal(data);
        }
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            return Cryptography.CompareStringHexadecimal(hashOfInput, hash);
        }
        public static bool VerifyHmacSha1(string key, string input, string hash)
        {
            string hashOfInput = GetHmacSha1(key, input);
            return Cryptography.CompareStringHexadecimal(hashOfInput, hash);
        }
        public static string GetHmacSha1(string key, string input)
        {
            HMACMD5 hmacmd5 = new HMACMD5(Encoding.UTF8.GetBytes(key));
            byte[] data = hmacmd5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Cryptography.FromBytesToStringHexadecimal(data);
        }
        public static string FromBytesToStringHexadecimal(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public static bool CompareStringHexadecimal(string inputOne, string inputTwo)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(inputOne, inputTwo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string decodeBasic(string value)
        {
            return value;
        }
        public static string encodeBasic(string value)
        {
            return value;
        }
    }
}
