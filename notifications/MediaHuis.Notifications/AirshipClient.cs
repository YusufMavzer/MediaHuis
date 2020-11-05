using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MediaHuis.Notifications
{
    public class AirshipClient
    {
        private readonly Uri _baseUrl = new Uri("https://go.airship.eu/");
        private readonly HttpClient _client;
        private readonly string _token;
        public AirshipClient(Config config)
        {
            _token = config.AirshipToken;
            _client = new HttpClient();
        }

        private HttpContent GetHttpContent(string title, string body)
        {
            var payload = new
            {
                audience = "all",
                device_types = new[] { "android", "ios" },
                notification = new
                {
                    ios = new { title, alert = body },
                    android = new { title, alert = body }
                }
            };
            return new StringContent(JsonSerializer.Serialize(payload));
        }
        
        public async Task<HttpResponseMessage> PushNotification(string title, string body)
        {
            _client.DefaultRequestHeaders.Clear();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(_baseUrl, "api/push"));
            httpRequestMessage.Content = GetHttpContent(title, body);
            httpRequestMessage.Headers.Clear();
            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", $"Bearer {_token}");
            httpRequestMessage.Headers.TryAddWithoutValidation("Accept", $"application/vnd.urbanairship+json; version=3");
            httpRequestMessage.Headers.TryAddWithoutValidation("Content-Type", $"application/vnd.urbanairship+json; version=3");
            return await _client.SendAsync(httpRequestMessage);
        }
    }
}
