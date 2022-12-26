namespace TompkinsCOVID;

public record Record
{
	public DateOnly Date;
	public ushort? ActiveCases { get; set; }

	public ushort? PositiveToday { get; init; }
	public ushort? SelfPositiveToday { get; init; }
	public ushort? TestedToday { get; init; }

	public ushort? PositiveTotal { get; init; }
	public ushort? SelfPositiveTotal { get; init; }

	public ushort? Hospitalized { get; init; }
	public ushort? Deceased { get; init; }

	public uint? PartiallyVaccinated { get; init; }
	public uint? FullyVaccinated { get; init; }

	public override string ToString()
		=> Date.ToShortDateString() + Environment.NewLine
			+ $"{ActiveCases?.ToString("N0") ?? "[unknown]"} active cases" + Environment.NewLine
			+ Environment.NewLine

			+ $"{PositiveToday?.ToString("N0") ?? "[unknown]"} new positive PCR tests" + Environment.NewLine
			+ $"{SelfPositiveToday?.ToString("N0") ?? "[unknown]"} new positive home tests" + Environment.NewLine
			+ $"{TestedToday?.ToString("N0") ?? "[unknown]"} new PCR test results" + Environment.NewLine
			+ Environment.NewLine

			+ $"{PositiveTotal?.ToString("N0") ?? "[unknown]"} total positive PCR tests" + Environment.NewLine
			+ $"{SelfPositiveTotal?.ToString("N0") ?? "[unknown]"} total positive home tests" + Environment.NewLine
			+ Environment.NewLine

			+ $"{Hospitalized?.ToString("N0") ?? "[unknown]"} currently hospitalized" + Environment.NewLine
			+ $"{Deceased?.ToString("N0") ?? "[unknown]"} deceased" + Environment.NewLine
			+ Environment.NewLine

			+ $"{PartiallyVaccinated?.ToString("N0") ?? "[unknown]"} partially vaccinated" + Environment.NewLine
			+ $"{FullyVaccinated?.ToString("N0") ?? "[unknown]"} fully vaccinated" + Environment.NewLine
			+ Environment.NewLine

			+ "#twithaca";
}