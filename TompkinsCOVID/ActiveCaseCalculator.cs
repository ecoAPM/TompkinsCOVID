namespace TompkinsCOVID;

public static class ActiveCaseCalculator
{
	private const int ActiveDays = 7;

	public static ushort CalculateActiveCases(this IDictionary<DateTime, Record> records, DateTime day)
		=> (ushort) records.Where(r => r.Key.AddDays(ActiveDays) > day && r.Key <= day)
			.Sum(r => (r.Value.PositiveToday ?? 0) + (r.Value.SelfPositiveToday ?? 0));
}