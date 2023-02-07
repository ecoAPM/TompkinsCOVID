using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public sealed class Mastodon : ISocialMediaManager
{
	private readonly HttpClient _http;
	private readonly IConfigurationSection _config;
	private readonly string? _accessToken;

	public Mastodon(HttpClient http, IConfigurationSection config, string? accessToken)
	{
		_http = http;
		_config = config;
		_accessToken = accessToken;
	}

	public async Task<DateOnly?> GetLatestPostedDate()
	{
		var request = new HttpRequestMessage(HttpMethod.Get, $"https://{_config["instance"]}/api/v1/accounts/{_config["id"]}/statuses")
		{
			Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) }
		};
		var response = await _http.SendAsync(request);
		var json = await JsonSerializer.DeserializeAsync<JsonElement[]>(await response.Content.ReadAsStreamAsync());

		var dates = new List<DateOnly>();
		foreach (var element in json!)
		{
			var text = element.GetProperty("content").GetString();
			if (DateOnly.TryParse(text?.Split("<p>")[1].Split("<br />")[0], out var date))
			{
				dates.Add(date);
			}
		}

		return dates.Any() ? dates.Max() : new DateOnly(2022, 11, 16);
	}

	public async Task Post(Record record)
	{
		var content = JsonSerializer.Serialize(record.ToString());
		var request = new HttpRequestMessage(HttpMethod.Post, $"https://{_config["instance"]}/api/v1/statuses")
		{
			Headers = { Authorization = new AuthenticationHeaderValue("Bearer", _accessToken) },
			Content = new StringContent($"{{ \"status\": {content} }}", new MediaTypeHeaderValue("application/json"))
		};
		await _http.SendAsync(request);
	}
}