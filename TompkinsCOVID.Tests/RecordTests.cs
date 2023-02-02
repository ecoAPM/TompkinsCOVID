using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class RecordTests
{
	[Fact]
	public async Task CanOutputAsPost()
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
		var post = record.ToString();

		//assert
		Assert.True(post.Length < 280);
		var expected = await File.ReadAllTextAsync("post.txt");
		Assert.Equal(expected, post);
	}
}