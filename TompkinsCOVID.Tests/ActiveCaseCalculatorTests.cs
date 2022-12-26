using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class ActiveCaseCalculatorTests
{
	[Fact]
	public void CanCalculateActiveCases()
	{
		//arrange

		var records = new Dictionary<DateOnly, Record>
		{
			{ DateOnly.Parse("06/30/2021"), new Record { Date = DateOnly.Parse("06/30/2021"), PositiveToday = 999, SelfPositiveToday = 999 } },
			{ DateOnly.Parse("07/01/2021"), new Record { Date = DateOnly.Parse("07/01/2021"), PositiveToday = 1, SelfPositiveToday = 2 } },
			{ DateOnly.Parse("07/02/2021"), new Record { Date = DateOnly.Parse("07/02/2021"), PositiveToday = 3, SelfPositiveToday = 4 } },
			{ DateOnly.Parse("07/03/2021"), new Record { Date = DateOnly.Parse("07/03/2021"), PositiveToday = 5, SelfPositiveToday = 6 } },
			{ DateOnly.Parse("07/04/2021"), new Record { Date = DateOnly.Parse("07/04/2021"), PositiveToday = 7, SelfPositiveToday = 8 } },
			{ DateOnly.Parse("07/05/2021"), new Record { Date = DateOnly.Parse("07/05/2021"), PositiveToday = 9, SelfPositiveToday = 10 } },
			{ DateOnly.Parse("07/06/2021"), new Record { Date = DateOnly.Parse("07/06/2021"), PositiveToday = 11, SelfPositiveToday = 12 } },
			{ DateOnly.Parse("07/07/2021"), new Record { Date = DateOnly.Parse("07/07/2021"), PositiveToday = 13, SelfPositiveToday = 14 } },
			{ DateOnly.Parse("07/08/2021"), new Record { Date = DateOnly.Parse("07/08/2021"), PositiveToday = 999, SelfPositiveToday = 999 } }
		};
		//act
		var active = records.CalculateActiveCases(DateOnly.Parse("07/07/2021"));

		//assert
		Assert.Equal(105, active);
	}
}