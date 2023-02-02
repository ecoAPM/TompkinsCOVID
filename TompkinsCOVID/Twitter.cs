using Tweetinvi;
using Tweetinvi.Parameters;

namespace TompkinsCOVID;

public sealed class Twitter : ISocialMediaManager
{
	private readonly ITwitterClient _client;

	public Twitter(ITwitterClient client)
		=> _client = client;

	public async Task<DateOnly?> GetLatestPostedDate(string username)
	{
		var options = new GetUserTimelineParameters(username)
		{
			PageSize = 5
		};
		var tweets = await _client.Timelines.GetUserTimelineAsync(options);
		var dates = new List<DateOnly>();
		foreach (var tweet in tweets)
		{
			if (DateOnly.TryParse(tweet.Text.Split("\n")[0], out var date))
				dates.Add(date);
		}

		return dates.Any() ? dates.Max() : null;
	}

	public async Task Post(Record record)
		=> await _client.Tweets.PublishTweetAsync(record.ToString());
}