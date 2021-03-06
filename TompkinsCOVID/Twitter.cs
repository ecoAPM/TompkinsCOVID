using Tweetinvi;
using Tweetinvi.Parameters;

namespace TompkinsCOVID;

public class Twitter : ITwitter
{
	private readonly ITwitterClient _client;

	public Twitter(ITwitterClient client)
		=> _client = client;

	public async Task<DateTime?> GetLatestPostedDate(string username)
	{
		var options = new GetUserTimelineParameters(username)
		{
			PageSize = 5
		};
		var tweets = await _client.Timelines.GetUserTimelineAsync(options);
		var dates = new List<DateTime>();
		foreach (var tweet in tweets)
		{
			if (DateTime.TryParse(tweet.Text.Split("\n")[0], out var date))
				dates.Add(date);
		}

		return dates.Any() ? dates.Max() : null;
	}

	public async Task Tweet(Record record)
		=> await _client.Tweets.PublishTweetAsync(record.ToString());
}