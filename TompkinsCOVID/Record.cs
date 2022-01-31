using AngleSharp.Dom;

namespace TompkinsCOVID;

public record Record
{
	public readonly DateTime Date;
	public ushort? ActiveCases;

	public readonly ushort? PositiveToday;
	public readonly ushort? SelfPositiveToday;
	public readonly ushort? TestedToday;

	public readonly ushort? PositiveTotal;
	public readonly ushort? SelfPositiveTotal;

	public readonly ushort? Hospitalized;
	public readonly ushort? Deceased;

	public readonly uint? PartiallyVaccinated;
	public readonly uint? FullyVaccinated;

	public Record(IList<IElement> cells)
	{
		if (!DateTime.TryParse(cells[0].TextContent, out Date))
			throw new ArgumentException("Could not read date from cells", nameof(cells));

		if (ushort.TryParse(Cleanup(cells[2]), out var testedToday))
			TestedToday = testedToday;

		if (ushort.TryParse(Cleanup(cells[3]), out var positiveToday))
			PositiveToday = positiveToday;

		if (ushort.TryParse(Cleanup(cells[4]), out var positiveTotal))
			PositiveTotal = positiveTotal;

		if (ushort.TryParse(Cleanup(cells[6]), out var activeCases))
			ActiveCases = activeCases;

		if (ushort.TryParse(Cleanup(cells[7]), out var hospitalized))
			Hospitalized = hospitalized;

		if (ushort.TryParse(Cleanup(cells[8]), out var deceased))
			Deceased = deceased;

		if (uint.TryParse(Cleanup(cells[9]), out var partiallyVaccinated))
			PartiallyVaccinated = partiallyVaccinated;

		if (uint.TryParse(Cleanup(cells[10]), out var fullyVaccinated))
			FullyVaccinated = fullyVaccinated;

		if (ushort.TryParse(Cleanup(cells[11]), out var selfPositiveToday))
			SelfPositiveToday = selfPositiveToday;

		if (ushort.TryParse(Cleanup(cells[12]), out var selfPositiveTotal))
			SelfPositiveTotal = selfPositiveTotal;
	}

	private static ReadOnlySpan<char> Cleanup(INode cell)
		=> cell.TextContent.Replace(",", "");

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