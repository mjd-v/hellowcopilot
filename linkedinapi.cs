using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

class LinkedInApi
{
    private static readonly string ClientId = "your-client-id";
    private static readonly string ClientSecret = "your-client-secret";
    private static readonly string RedirectUri = "your-redirect-uri";
    private static readonly string AccessToken = "your-access-token";

    static async Task Main(string[] args)
    {
        var messages = await FetchMessages();
        foreach (var message in messages)
        {
            await ReplyToMessage(message);
        }
    }

    private static async Task<string> GetAccessToken()
    {
        using (var client = new HttpClient())
        {
            var requestBody = new
            {
                grant_type = "authorization_code",
                code = "your-authorization-code",
                redirect_uri = RedirectUri,
                client_id = ClientId,
                client_secret = ClientSecret
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);
            return result.access_token.ToString();
        }
    }

    private static async Task<dynamic[]> FetchMessages()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await client.GetAsync("https://api.linkedin.com/v2/messages");
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseBody);
            return result.elements.ToObject<dynamic[]>();
        }
    }

    private static async Task ReplyToMessage(dynamic message)
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var requestBody = new
            {
                body = new
                {
                    text = "Thank you for your message!"
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"https://api.linkedin.com/v2/messages/{message.id}/reply", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
