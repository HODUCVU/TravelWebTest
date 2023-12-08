using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPal.Api;
namespace WebDuLichMVC.Services
{
    public class PayPalConfiguration
    {
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        static PayPalConfiguration()
        {
            var config = GetConfig();
            ClientId = config["clientId"];
            ClientSecret = config["clientSecret"];
        }
        public static Dictionary<string, string> GetConfig() {
            return new Dictionary<string, string>() {
                {"clientId", "xxx"},
                {"clientSecret", "xxx"}
            };
        }

        //Create accessToken
        private static string GetAccessToken() {
            string accessToken = new OAuthTokenCredential(ClientId, ClientSecret,
                GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext(string accessToken = "") {
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ?
                GetAccessToken() : accessToken);
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}