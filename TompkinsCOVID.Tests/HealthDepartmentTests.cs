using System.Net;
using Xunit;

namespace TompkinsCOVID.Tests;

public class HealthDepartmentTests
{
	[Fact]
	public async Task CanReadFromSpreadsheet()
	{
		//arrange
		var handler = new MockHttp();
		var client = new HttpClient(handler);
		var hd = new HealthDepartment(client);

		//act
		var records = await hd.GetRecords("http://localhost");

		//assert
		Assert.Equal("07/01/2021", records.Single().Key.ToShortDateString());
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