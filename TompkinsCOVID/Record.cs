namespace TompkinsCOVID;

public record Record
{
	public DateOnly Date  { get; init; }
	public ushort? ActiveCases { get; set; }

	public ushort? PositiveToday { get; init; }
	public ushort? TestedToday { get; init; }
	public ushort? PositiveTotal { get; init; }

	public ushort? Hospitalized { get; init; }
	public ushort? Deceased { get; init; }

	public override string ToString()
		=> Date.ToShortDateString() + Environment.NewLine
            + $"{ActiveCases?.ToString("N0") ?? "[unknown]"} active cases" + Environment.NewLine
            + $"{PositiveToday?.ToString("N0") ?? "[unknown]"} new positive tests" + Environment.NewLine
            + Environment.NewLine
            + $"{TestedToday?.ToString("N0") ?? "[unknown]"} new test results" + Environment.NewLine
            + $"{PositiveTotal?.ToString("N0") ?? "[unknown]"} total positive tests" + Environment.NewLine
            + Environment.NewLine
            + $"{Hospitalized?.ToString("N0") ?? "[unknown]"} currently hospitalized" + Environment.NewLine
            + $"{Deceased?.ToString("N0") ?? "[unknown]"} deceased" + Environment.NewLine
            + Environment.NewLine
            + "#ithaca";
}