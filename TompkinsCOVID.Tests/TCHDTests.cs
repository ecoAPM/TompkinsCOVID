using System.Net;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class TCHDTests
{
	[Fact]
	public async Task CanGetAllRecords()
	{
		//arrange
		var handler = new MockHttp();
		var client = new HttpClient(handler);
		var hd = new TCHD(client, "http://localhost");

		//act
		var records = await hd.GetRecords();

		//assert
		Assert.Equal("07/01/2021", records.First().Key.ToShortDateString());
	}

	[Fact]
	public async Task CanGetRecordsSinceLatest()
	{
		//arrange
		var handler = new MockHttp();
		var client = new HttpClient(handler);
		var hd = new TCHD(client, "http://localhost");

		//act
		var records = await hd.GetRecordsSince(DateOnly.Parse("07/01/2021"));

		//assert
		Assert.Equal("07/02/2021", records.Single().Key.ToShortDateString());
	}

	private class MockHttp : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			const string html = @"<html>
                                        <body>
                                            <table>
                                                <tbody>
                                                    <tr><td>Header</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
                                                    <tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
                                                    <tr><td>07/01/2021</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
                                                    <tr><td>07/02/2021</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>
                                                </tbody>
                                            </table>
                                        </body>
                                    </html>";

			var message = new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(html)
			};
			return Task.FromResult(message);
		}
	}
}