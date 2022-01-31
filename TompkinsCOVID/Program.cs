namespace TompkinsCOVID;

public static class Program
{
	public static async Task Main(string[] args)
		=> await Factory.Runner().Run(args.Any() ? args[0] : null);
}