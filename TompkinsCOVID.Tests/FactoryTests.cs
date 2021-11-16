using Xunit;

namespace TompkinsCOVID.Tests;

public class FactoryTests
{
	[Fact]
	public void CanCreateRunner()
	{
		//arrange/act
		var runner = Factory.Runner();

		//assert
		Assert.IsType<Runner>(runner);
	}
}