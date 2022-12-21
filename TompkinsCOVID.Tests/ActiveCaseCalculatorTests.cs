using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class ActiveCaseCalculatorTests
{
	[Fact]
	public void CanCalculateActiveCases()
	{
		//arrange
		var records = new Dictionary<DateTime, Record>
		{
			{ DateTime.Parse("06/30/2021"), new Record(Stub.Row(new[] { "06/30/2021", "", "", "999", "", "", "", "", "", "", "", "999", "" })) },
			{ DateTime.Parse("07/01/2021"), new Record(Stub.Row(new[] { "07/01/2021", "", "", "1", "", "", "", "", "", "", "", "2", "" })) },
			{ DateTime.Parse("07/02/2021"), new Record(Stub.Row(new[] { "07/02/2021", "", "", "3", "", "", "", "", "", "", "", "4", "" })) },
			{ DateTime.Parse("07/03/2021"), new Record(Stub.Row(new[] { "07/03/2021", "", "", "5", "", "", "", "", "", "", "", "6", "" })) },
			{ DateTime.Parse("07/04/2021"), new Record(Stub.Row(new[] { "07/04/2021", "", "", "7", "", "", "", "", "", "", "", "8", "" })) },
			{ DateTime.Parse("07/05/2021"), new Record(Stub.Row(new[] { "07/05/2021", "", "", "9", "", "", "", "", "", "", "", "10", "" })) },
			{ DateTime.Parse("07/06/2021"), new Record(Stub.Row(new[] { "07/06/2021", "", "", "11", "", "", "", "", "", "", "", "12", "" })) },
			{ DateTime.Parse("07/07/2021"), new Record(Stub.Row(new[] { "07/07/2021", "", "", "13", "", "", "", "", "", "", "", "14", "" })) },
			{ DateTime.Parse("07/08/2021"), new Record(Stub.Row(new[] { "07/08/2021", "", "", "999", "", "", "", "", "", "", "", "999", "" })) }
		};

		//act
		var active = records.CalculateActiveCases(DateTime.Parse("07/07/2021"));

		//assert
		Assert.Equal(105, active);
	}
}