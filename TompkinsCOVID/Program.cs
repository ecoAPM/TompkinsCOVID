using System.Threading.Tasks;

namespace TompkinsCOVID;

public static class Program
{
	public static async Task Main()
		=> await Factory.Runner().Run();
}
