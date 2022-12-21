using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class RecordTests
{
	[Fact]
	public async Task CanOutputAsTweet()
	{
		//arrange
		var data = new[] { "1/30/22", "1,777,942", "1,933", "43", "16,697", "16,302", "342", "18", "53", "83059", "75596", "10", "1497" };
		var cells = Stub.Row(data);
		var record = new Record(cells);

		//act
		var tweet = record.ToString();

		//assert
		var expected = await File.ReadAllTextAsync("tweet.txt");
		Assert.Equal(expected, tweet);
	}

	[Fact]
	public void NoDateThrows()
	{
		//arrange
		var data = Array.Empty<string>();

		//act
		var cells = Stub.Row(data);

		//assert
		Assert.ThrowsAny<Exception>(() => new Record(cells));
	}
}