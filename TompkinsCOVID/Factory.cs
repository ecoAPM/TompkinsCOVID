using Tweetinvi;
using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public static class Factory
{
	public static Runner Runner()
	{
		var consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
		var consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
		var accessKey = Environment.GetEnvironmentVariable("AccessKey");
		var accessSecret = Environment.GetEnvironmentVariable("AccessSecret");
		var client = new TwitterClient(consumerKey, consumerSecret, accessKey, accessSecret);
		var twitter = new Twitter(client);

		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
		var http = new HttpClient();
		var url = config["url"] ?? string.Empty;
		var healthDept = new TCHD(http, url);

		return new Runner(twitter, healthDept, Console.WriteLine, config);
	}
}