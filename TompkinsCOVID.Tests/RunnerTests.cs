using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class RunnerTests
{
	[Fact]
	public async Task TweetsNewRecords()
	{
		//arrange
		var twitter = Substitute.For<ITwitter>();
		twitter.GetLatestPostedDate(Arg.Any<string>()).Returns(DateOnly.Parse("6/30/2021"));

		var hd = Substitute.For<IHealthDepartment>();
		var yesterday = new Record(Stub.Row(new[] { "06/30/2021", "", "", "1", "", "", "2"  }));
		var today = new Record(Stub.Row(new[] { "07/01/2021", "", "", "256", "", "", "1024" }));
		hd.GetRecords().Returns(new Dictionary<DateOnly, Record>
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

		var runner = new Runner(twitter, hd, _ => { }, config);

		//act
		await runner.Run();

		//assert
		await twitter.Received(1).Tweet(Arg.Any<Record>());
	}

	[Fact]
	public async Task DoesNotTweetIncomplete()
	{
		//arrange
		var twitter = Substitute.For<ITwitter>();
		twitter.GetLatestPostedDate(Arg.Any<string>()).Returns(DateOnly.Parse("6/30/2021"));

		var hd = Substitute.For<IHealthDepartment>();
		var today = new Record(Stub.Row(new[] { "07/01/2021" }));
		hd.GetRecords().Returns(new Dictionary<DateOnly, Record>
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

		var runner = new Runner(twitter, hd, _ => { }, config);

		//act
		await runner.Run();

		//assert
		await twitter.DidNotReceive().Tweet(Arg.Any<Record>());
	}
}