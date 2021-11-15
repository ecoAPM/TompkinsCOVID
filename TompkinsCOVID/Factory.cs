using System;
using System.Net.Http;
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

		var http = new HttpClient();
		var healthDept = new HealthDepartment(http);

		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		return new Runner(twitter, healthDept, Console.WriteLine, config);
	}
}
