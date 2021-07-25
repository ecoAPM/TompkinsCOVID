using System;
using AngleSharp.Dom;

namespace COVID
{
    public record Record
    {
        public readonly DateTime Date;
        public readonly uint TestedTotal;
        public readonly ushort TestedToday;
        public readonly byte PositiveToday;
        public readonly ushort PositiveTotal;
        public readonly ushort Recovered;
        public readonly ushort ActiveCases;
        public readonly byte Hospitalized;
        public readonly byte Deceased;
        public readonly uint PartiallyVaccinated;
        public readonly uint FullyVaccinated;

        public Record(IHtmlCollection<IElement> cells)
        {
            DateTime.TryParse(Cleanup(cells[0]), out Date);
            uint.TryParse(Cleanup(cells[1]), out TestedTotal);
            ushort.TryParse(Cleanup(cells[2]), out TestedToday);
            byte.TryParse(Cleanup(cells[3]), out PositiveToday);
            ushort.TryParse(Cleanup(cells[4]), out PositiveTotal);
            ushort.TryParse(Cleanup(cells[5]), out Recovered);
            ushort.TryParse(Cleanup(cells[6]), out ActiveCases);
            byte.TryParse(Cleanup(cells[7]), out Hospitalized);
            byte.TryParse(Cleanup(cells[8]), out Deceased);
            uint.TryParse(Cleanup(!string.IsNullOrWhiteSpace(cells[11].TextContent) ? cells[11] : cells[9]), out PartiallyVaccinated);
            uint.TryParse(Cleanup(!string.IsNullOrWhiteSpace(cells[12].TextContent) ? cells[12] : cells[10]), out FullyVaccinated);
        }

        private static ReadOnlySpan<char> Cleanup(IElement cell)
            => cell.TextContent.Replace(",", "");
    }
}