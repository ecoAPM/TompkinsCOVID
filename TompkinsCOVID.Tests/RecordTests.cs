using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class RecordTests
{
	[Fact]
	public async Task CanOutputAsTweet()
	{
		//arrange
		var record = new Record
		{
			Date = DateOnly.Parse("1/30/22"),
			ActiveCases = 342,
			PositiveToday = 43,
			TestedToday = 1933,
			PositiveTotal = 16697,
			Hospitalized = 18,
			Deceased = 53,
			PartiallyVaccinated = 83059,
			FullyVaccinated = 75596,
			SelfPositiveToday = 10,
			SelfPositiveTotal = 1497
		};

		//act
		var tweet = record.ToString();

		//assert
		var expected = await File.ReadAllTextAsync("tweet.txt");
		Assert.Equal(expected, tweet);
	}
}