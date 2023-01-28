using Jose;
using PlaylistConverter.Models;
using System.Security.Cryptography;

namespace PlaylistConverter.Services
{
    public static class AppleMusicServices
    {
        public static string Secret = "";
        public static string CreateDevToken()
        {
            // Get the 'issued at' and 'expiration' values 
            var iat = Math.Round((DateTime.UtcNow.AddMinutes(-1) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, 0);
            var exp = Math.Round((DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds, 0);

            // The 'issued' code is the unqiue code from our Apple Devloper account
            var payload = new Dictionary<string, object>()
        {
            { "iat", iat },
            { "exp", exp },
            { "iss", "2LVQ52Y6V4" }
        };

            // We are encrypting this using ES256 per Apple Music Docs
            // The 'KID' value is from the registered app in the dev account  
            var extraHeader = new Dictionary<string, object>()
        {
            { "alg", "ES256" },
            { "typ", "JWT" },
            { "kid", "6C9TU33CXS" }
        };

            var keyString = Secret;

            CngKey privateKey = CngKey.Import(Convert.FromBase64String(keyString), CngKeyBlobFormat.Pkcs8PrivateBlob);

            return JWT.Encode(payload, privateKey, JwsAlgorithm.ES256, extraHeader);
        }
    }
}
