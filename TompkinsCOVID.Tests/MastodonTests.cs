using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class MastodonTests
{
	[Fact]
	public async Task CanGetLatestDate()
	{
		//arrange
		var response = @"[
			{ ""content"": ""<p>1/1/2023"" },
			{ ""content"": ""<p>not an automated post"" },
			{ ""content"": ""<p>1/2/2023"" }
		]";
		var handler = new MockHttpHandler(response);
		var http = new HttpClient(handler);
		var settings = new Dictionary<string, string?>
		{
			{ "mastodon:instance", "localhost"}
		};
		var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build().GetSection("mastodon");
		var client = new Mastodon(http, config, string.Empty);

		//act
		var date = await client.GetLatestPostedDate();

		//assert
		Assert.Equal(new DateOnly(2023, 1, 2), date);
	}

	[Fact]
	public async Task CanPostUpdate()
	{
		//arrange
		var handler = new MockHttpHandler(string.Empty);
		var http = new HttpClient(handler);
		var settings = new Dictionary<string, string?>
		{
			{ "mastodon:instance", "localhost"}
		};
		var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build().GetSection("mastodon");
		var client = new Mastodon(http, config, string.Empty);

		//act
		var record = new Record();
		await client.Post(record);

		//assert
		var status = JsonSerializer.Deserialize<JsonElement>(handler.PostBody!).GetProperty("status").GetString();
		Assert.Equal(record.ToString(), status);
	}

	private class MockHttpHandler : HttpMessageHandler
	{
		private readonly string _response;

		public string? PostBody { get; set; }

		public MockHttpHandler(string response)
			=> _response = response;

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			PostBody = request.Content is not null
				? await request.Content.ReadAsStringAsync(cancellationToken)
				: string.Empty;

			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(_response)
			};
		}
	}
}