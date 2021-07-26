using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TompkinsCOVID
{
    public class Twitter
    {
        private readonly HttpClient _http;
        private readonly string _consumerKey;
        private readonly string _consumerSecret;
        private readonly string _consumerToken;
        private readonly string _accessKey;
        private readonly string _accessSecret;

        private const string GetURL = "https://api.twitter.com/2/users/1419376352080314369/tweets?max_results=5";
        private const string PostURL = "https://api.twitter.com/1.1/statuses/update.json?include_entities=true";

        public Twitter(HttpClient http)
        {
            _http = http;
            _consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
            _consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
            _consumerToken = Environment.GetEnvironmentVariable("ConsumerToken");
            _accessKey = Environment.GetEnvironmentVariable("AccessKey");
            _accessSecret = Environment.GetEnvironmentVariable("AccessSecret");
        }

        public async Task<DateTime?> GetLatestPostedDate()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, GetURL);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _consumerToken);
            var response = await _http.SendAsync(request);
            var content = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(content);
            var tweets = json.RootElement.GetProperty("data").EnumerateArray();
            var dates = new List<DateTime>();
            foreach (var tweet in tweets)
            {
                var text = tweet.GetProperty("text").GetString();
                if (DateTime.TryParse(text.Split("\n")[0], out var date))
                    dates.Add(date);
            }
            return dates.Any() ? dates.Max() : null;
        }

        public async Task Tweet(Record record)
        {
            var text = record.ToString();
            var data = new Dictionary<string, string>
            {
                {"include_entities", "true"},
                {"oauth_consumer_key", _consumerKey},
                {"oauth_nonce", Convert.ToBase64String(Encoding.UTF8.GetBytes(new Random().Next().ToString())) },
                {"oauth_signature_method", "HMAC-SHA1" },
                {"oauth_timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()},
                {"oauth_token", _accessKey},
                {"oauth_version", "1.0"},
                {"status", text}
            };
            var signature = await GetSignature(data);
            data.Add("oauth_signature", signature);
            var request = new HttpRequestMessage(HttpMethod.Post, PostURL);
            var auth = string.Join(", ", data.Where(d => d.Key.StartsWith("oauth")).Select(d => $@"{d.Key}=""{d.Value}"""));
            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", auth);
            request.Content = new FormUrlEncodedContent(data);
            var response = await _http.SendAsync(request);
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private async Task<string> GetSignature(Dictionary<string, string> data)
        {
            var regex = new Regex(@"%[a-f0-9]{2}");
            var baseContent = new FormUrlEncodedContent(data);
            var encodedContent = regex.Replace(HttpUtility.UrlEncode(await baseContent.ReadAsStringAsync()), m => m.Value.ToUpperInvariant());
            var encodedURL = regex.Replace(HttpUtility.UrlEncode(PostURL), m => m.Value.ToUpperInvariant());
            var signatureBase = $@"POST&{encodedURL}&{encodedContent}";
            var signingKey = $"{_consumerSecret}&{_accessSecret}";
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(signingKey));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(signatureBase));
            var signature = Convert.ToBase64String(hash);
            return signature;
        }
    }
}