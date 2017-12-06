using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CCA.TheraNest.API
{
    public class Account
    {
        public static HttpClient GetClient(IToken token, string baseUrl = "https://api.theranest.com")
        {
            var client = new HttpClient() { BaseAddress = new Uri(baseUrl) };

            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            var bearer = $"Bearer {token}";
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", bearer);

            return client;
        }

        public static async Task<HttpClient> SignIn(string user, string password, string baseUrl = "https://api.theranest.com")
        {
            var client = new HttpClient() {BaseAddress = new Uri(baseUrl)};

            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            var model = new SignInModel { email = user, password = password };

            var response = await client.PostAsync("account/sign-in", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Post account sign-in was not successful, returned code {response.StatusCode}.");

            var token = await response.Content.ReadAsStringAsync();
            token = token.Replace("\"", "");
            return GetClient(new JwtToken {Token = token});
        }

        class SignInModel
        {
            // ReSharper disable InconsistentNaming
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public string email { get; set; }
            public string password { get; set; }
            // ReSharper restore InconsistentNaming
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}
