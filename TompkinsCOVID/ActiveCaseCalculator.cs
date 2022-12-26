namespace TompkinsCOVID;

public static class ActiveCaseCalculator
{
	public const int ActiveDays = 7;

	public static ushort CalculateActiveCases(this IDictionary<DateOnly, Record> records, DateOnly day)
		=> (ushort) records.Where(r => r.Key.AddDays(ActiveDays) > day && r.Key <= day)
			.Sum(r => r.Value.PositiveToday ?? 0);
}