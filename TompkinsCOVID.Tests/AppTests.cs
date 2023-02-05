using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class AppTests
{
	[Fact]
	public async Task PostsNewRecords()
	{
		//arrange
		var twitter = Substitute.For<ISocialMediaManager>();
		twitter.GetLatestPostedDate(Arg.Any<string>()).Returns(DateOnly.Parse("6/30/2021"));

		var hd = Substitute.For<IHealthDepartment>();
		var yesterday = new Record { Date = DateOnly.Parse("06/30/2021"), PositiveToday = 1 };
		var today = new Record { Date = DateOnly.Parse("07/01/2021"), PositiveToday = 256 };
		hd.GetRecordsSince(Arg.Any<DateOnly>()).Returns(new Dictionary<DateOnly, Record>
		{
			{ DateOnly.Parse("06/30/2021"), yesterday },
			{ DateOnly.Parse("07/01/2021"), today }
		});

		var settings = new Dictionary<string, string?>
		{
			{ "url", "http://localhost" },
			{ "username", "test" },
			{ "wait", "0" }
		};
		var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

		var app = new App(twitter, hd, _ => { }, config);

		//act
		await app.Run();

		//assert
		await twitter.Received(1).Post(Arg.Any<Record>());
	}

	[Fact]
	public async Task DoesNotPostIncomplete()
	{
		//arrange
		var twitter = Substitute.For<ISocialMediaManager>();
		twitter.GetLatestPostedDate(Arg.Any<string>()).Returns(DateOnly.Parse("6/30/2021"));

		var hd = Substitute.For<IHealthDepartment>();
		var today = new Record { Date = DateOnly.Parse("07/01/2021") };
		hd.GetRecordsSince(Arg.Any<DateOnly>()).Returns(new Dictionary<DateOnly, Record>
		{
			{ DateOnly.Parse("07/01/2021"), today }
		});

		var settings = new Dictionary<string, string?>
		{
			{ "url", "http://localhost" },
			{ "username", "test" },
			{ "wait", "0" }
		};
		var config = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

		var app = new App(twitter, hd, _ => { }, config);

		//act
		await app.Run();

		//assert
		await twitter.DidNotReceive().Post(Arg.Any<Record>());
	}
}