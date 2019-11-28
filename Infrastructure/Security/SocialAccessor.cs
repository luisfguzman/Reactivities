using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.User;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Security
{
    public class SocialAccessor : ISocialAccessor
    {
        private class GoogleValidate {
            public string iss { get; set; }
            public string aud { get; set; }
            public long exp { get; set; }
        }
        private readonly HttpClient _httpFacebookClient;
        private readonly HttpClient _httpGoogleClient;
        private readonly IOptions<FacebookAppSettings> _fbconfig;
        private readonly IOptions<GoogleAppSettings> _googleConfig;
        public SocialAccessor(IOptions<FacebookAppSettings> fbConfig, IOptions<GoogleAppSettings> googleConfig)
        {
            _googleConfig = googleConfig;
            _fbconfig = fbConfig;
            _httpFacebookClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://graph.facebook.com/")
            };
            _httpFacebookClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpGoogleClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://oauth2.googleapis.com/")
            };
            _httpGoogleClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<FacebookUserInfo> FacebookLogin(string accessToken)
        {
            // verify token is valid
            var verifyToken = await _httpFacebookClient.GetAsync($"debug_token?input_token={accessToken}&access_token={_fbconfig.Value.AppId}|{_fbconfig.Value.AppSecret}");

            if (!verifyToken.IsSuccessStatusCode)
                return null;

            var result = await GetAsync<FacebookUserInfo>(accessToken, "me", "fields=name,email,picture.width(100).height(100)");

            return result;
        }

        public async Task<GoogleUserInfo> GoogleLogin(string idToken)
        {
            var response = await _httpGoogleClient.GetAsync($"tokeninfo?id_token={idToken}");

            if(!response.IsSuccessStatusCode)
                return null;
            
            var result = await response.Content.ReadAsStringAsync();

            var validationData = JsonConvert.DeserializeObject<GoogleValidate>(result);
            var expiryDate = new DateTime(validationData.exp);
            var compareResult = DateTime.Compare(expiryDate,  DateTime.Now);

            if(validationData.aud != _googleConfig.Value.ClientId || validationData.iss != "accounts.google.com" || compareResult > 0)
                return null;

            return JsonConvert.DeserializeObject<GoogleUserInfo>(result);
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args)
        {
            var response = await _httpFacebookClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");

            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}