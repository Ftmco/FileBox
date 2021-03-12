using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public static class Hash
{
    public static async Task<string> CreateSHA256Async(this string str)
    {
        return await Task.Run(() =>
        {
            SHA256 sha256 = SHA256.Create();
            UTF8Encoding encoder = new();
            byte[] combined = encoder.GetBytes(str);
            sha256.ComputeHash(combined);
            string hash = BitConverter.ToString(sha256.Hash);
            string returnHash = hash.Replace("-", string.Empty);
            return returnHash;
        });
    }

    public static string CreateSHA256(this string str)
    {
        SHA256 sha256 = SHA256.Create();
        UTF8Encoding encoder = new();
        byte[] combined = encoder.GetBytes(str);
        sha256.ComputeHash(combined);
        string hash = BitConverter.ToString(sha256.Hash);
        string returnHash = hash.Replace("-", string.Empty);
        return returnHash;
    }
}