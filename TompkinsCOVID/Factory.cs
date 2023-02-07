using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public static class Factory
{
	public static App App()
	{
		var http = new HttpClient();
		var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

		var accessToken = Environment.GetEnvironmentVariable("AccessToken");
		var mastodon = new Mastodon(http, config.GetSection("mastodon"), accessToken);

		var healthDept = new NYSDOH_CDC(http, config.GetSection("api"));

		return new App(mastodon, healthDept, Console.WriteLine, config);
	}
}