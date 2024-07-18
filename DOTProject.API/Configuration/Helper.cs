using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DOTProject.API.Configuration
{
    public static class Helper
    {
        public static string Hash(string text)
        {
            string result = string.Empty;
            var salt = Encoding.ASCII.GetBytes("SuperRdmTextJobSheet");
            result = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: text!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return result;
        }
    }
}