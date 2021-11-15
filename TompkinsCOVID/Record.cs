using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace TompkinsCOVID;

public record Record
{
	public readonly DateTime Date;
	public readonly uint? TestedTotal;
	public readonly ushort? TestedToday;
	public readonly byte? PositiveToday;
	public readonly ushort? PositiveTotal;
	public readonly ushort? Recovered;
	public readonly ushort? ActiveCases;
	public readonly byte? Hospitalized;
	public readonly byte? Deceased;
	public readonly uint? PartiallyVaccinated;
	public readonly uint? FullyVaccinated;

	public Record(IList<IElement> cells)
	{
		if (!DateTime.TryParse(Cleanup(cells[0]), out Date))
			throw new ArgumentException("Could not read date from cells", nameof(cells));

		if (uint.TryParse(Cleanup(cells[1]), out var testedTotal))
			TestedTotal = testedTotal;

		if (ushort.TryParse(Cleanup(cells[2]), out var testedToday))
			TestedToday = testedToday;

		if (byte.TryParse(Cleanup(cells[3]), out var positiveToday))
			PositiveToday = positiveToday;

		if (ushort.TryParse(Cleanup(cells[4]), out var positiveTotal))
			PositiveTotal = positiveTotal;

		if (ushort.TryParse(Cleanup(cells[5]), out var recovered))
			Recovered = recovered;

		if (ushort.TryParse(Cleanup(cells[6]), out var activeCases))
			ActiveCases = activeCases;

		if (byte.TryParse(Cleanup(cells[7]), out var hospitalized))
			Hospitalized = hospitalized;

		if (byte.TryParse(Cleanup(cells[8]), out var deceased))
			Deceased = deceased;

		if (uint.TryParse(Cleanup(!string.IsNullOrWhiteSpace(cells[11].TextContent) ? cells[11] : cells[9]), out var partiallyVaccinated))
			PartiallyVaccinated = partiallyVaccinated;

		if (uint.TryParse(Cleanup(!string.IsNullOrWhiteSpace(cells[12].TextContent) ? cells[12] : cells[10]), out var fullyVaccinated))
			FullyVaccinated = fullyVaccinated;
	}

	private static ReadOnlySpan<char> Cleanup(INode cell)
		=> cell.TextContent.Replace(",", "");

	public override string ToString()
		=> Date.ToShortDateString() + Environment.NewLine
			+ Environment.NewLine
			+ $"{PositiveToday?.ToString("N0") ?? "[unknown]"} new positive cases" + Environment.NewLine
			+ $"{ActiveCases?.ToString("N0") ?? "[unknown]"} active cases" + Environment.NewLine
			+ $"{PositiveTotal?.ToString("N0") ?? "[unknown]"} total positive cases" + Environment.NewLine
			+ Environment.NewLine
			+ $"{Hospitalized?.ToString("N0") ?? "[unknown]"} currently hospitalized" + Environment.NewLine
			+ $"{Deceased?.ToString("N0") ?? "[unknown]"} deceased" + Environment.NewLine
			+ $"{Recovered?.ToString("N0") ?? "[unknown]"} recovered" + Environment.NewLine
			+ Environment.NewLine
			+ $"{TestedToday?.ToString("N0") ?? "[unknown]"} tested today" + Environment.NewLine
			+ $"{TestedTotal?.ToString("N0") ?? "[unknown]"} tested total" + Environment.NewLine
			+ Environment.NewLine
			+ $"{PartiallyVaccinated?.ToString("N0") ?? "[unknown]"} partially vaccinated" + Environment.NewLine
			+ $"{FullyVaccinated?.ToString("N0") ?? "[unknown]"} fully vaccinated" + Environment.NewLine
			+ Environment.NewLine
			+ $"#twithaca";
}
