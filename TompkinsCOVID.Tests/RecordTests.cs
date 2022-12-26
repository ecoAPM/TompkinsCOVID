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
			Date = DateOnly.Parse("12/26/2022"),
			ActiveCases = 123,
			PositiveToday = 12,
			TestedToday = 1234,
			PositiveTotal = 12345,
			Hospitalized = 23,
			Deceased = 34,
			BivalentBoosted = 12.3m,
			VaxxedAndBoosted = 23.4m,
			FullyVaccinated = 34.5m,
			PartiallyVaccinated = 45.6m
		};

		//act
		var tweet = record.ToString();

		//assert
		Assert.True(tweet.Length < 280);
		var expected = await File.ReadAllTextAsync("tweet.txt");
		Assert.Equal(expected, tweet);
	}
}