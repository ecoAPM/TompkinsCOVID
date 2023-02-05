using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class FactoryTests
{
	[Fact]
	public void CanCreateApp()
	{
		//arrange/act
		var app = Factory.App();

		//assert
		Assert.IsType<App>(app);
	}
}