using System.Net;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class NYSDOH_CDCTests
{
	private static readonly byte[] configData = @"
	{
		""api"": {
			""tests"": ""http://localhost/tests"",
			""hospitalizations"": ""http://localhost/hospitalizations"",
			""fatalities"": ""http://localhost/fatalities"",
			""vaccinations"": ""http://localhost/vaccinations""
		}
	}"u8.ToArray();

	private static readonly IConfigurationSection Config
		= new ConfigurationBuilder()
			.AddJsonStream(new MemoryStream(configData)).Build()
			.GetSection("api");

	[Fact]
	public async Task CanGetAllRecords()
	{
		//arrange
		var handler = new MockHttp();
		var client = new HttpClient(handler);
		var hd = new NYSDOH_CDC(client, Config);

		//act
		var records = await hd.GetRecords();

		//assert
		Assert.Equal(2, records.Count);
		Assert.Equal("07/01/2021", records.First().Key.ToShortDateString());
	}

	[Fact]
	public async Task CanGetRecordsSinceLatest()
	{
		//arrange
		var handler = new MockHttp();
		var client = new HttpClient(handler);
		var hd = new NYSDOH_CDC(client, Config);

		//act
		var records = await hd.GetRecordsSince(DateOnly.Parse("07/01/2021"));

		//assert
		Assert.Equal("07/02/2021", records.Single().Key.ToShortDateString());
	}

	private class MockHttp : HttpMessageHandler
	{
		const string testData = @"[
			{
				""test_date"": ""2021-07-01"",
				""new_positives"": """",
				""total_number_of_tests"": """",
				""cumulative_number_of_positives"": """"
			},
			{
				""test_date"": ""2021-07-02"",
				""new_positives"": """",
				""total_number_of_tests"": """",
				""cumulative_number_of_positives"": """"
			}
		]";

		const string hospitalizationData = @"[
			{
				""as_of_date"": ""2021-07-01"",
				""patients_currently"": """"
			}
		]";

		const string fatalityData = @"[
			{
				""as_of_date"": ""2021-07-01"",
				""total_by_place_of_fatality"": """"
			}
		]";

		const string vaccinationData = @"[
			{
				""date"": ""2021-07-01"",
				""administered_dose1_pop_pct"": """",
				""series_complete_pop_pct"": """",
				""booster_doses_vax_pct"": """",
				""bivalent_booster_5plus_pop_pct"": """"
			}
		]";

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var data = GetData(request.RequestUri!);
			var message = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(data)
			};
			return Task.FromResult(message);
		}

		private static string GetData(Uri url)
		{
			if (url.LocalPath.Contains("tests"))
				return testData;

			if (url.LocalPath.Contains("hospitalizations"))
				return hospitalizationData;

			if (url.LocalPath.Contains("fatalities"))
				return fatalityData;

			if (url.LocalPath.Contains("vaccinations"))
				return vaccinationData;

			return null!;
		}
	}
}