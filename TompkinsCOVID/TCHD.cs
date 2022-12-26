using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;

namespace TompkinsCOVID;

public sealed class TCHD : IHealthDepartment
{
	private readonly HttpClient _http;
	private readonly string _url;

	public TCHD(HttpClient http, string url)
	{
		_http = http;
		_url = url;
	}

	public async Task<IDictionary<DateOnly, Record>> GetRecords()
	{
		var html = await _http.GetAsync(_url);
		var content = html.Content.ReadAsStreamAsync();

		var browser = BrowsingContext.New(Configuration.Default);

		async void Request(VirtualResponse r) => r.Content(await content);
		var dom = await browser.OpenAsync(Request);

		var rows = dom.DocumentElement.QuerySelectorAll("tbody tr");
		var records = new Dictionary<DateOnly, Record>();
		foreach (var row in rows)
		{
			var cells = row.QuerySelectorAll("td");

			var day = cells[0].TextContent;
			if (string.IsNullOrWhiteSpace(day) || !DateOnly.TryParse(day, out var rowDate))
				continue;

			records.Add(rowDate, FromSpreadsheet(cells.ToList()));
		}

		return records;
	}

	public async Task<IDictionary<DateOnly, Record>> GetRecordsSince(DateOnly date)
	{
		var records = await GetRecords();

		return records.Where(r => r.Key > date)
			.ToDictionary(r => r.Key, r => r.Value);
	}

	public static Record FromSpreadsheet(IList<IElement> tchdCells)
	{
		if (!DateOnly.TryParse(tchdCells[0].TextContent, out var date))
			throw new ArgumentException("Could not read date from cells", nameof(tchdCells));

		return new Record
		{
			Date = date,
			TestedToday = ushort.TryParse(Cleanup(tchdCells[2]), out var testedToday)
				? testedToday
				: null,

			PositiveToday = ushort.TryParse(Cleanup(tchdCells[3]), out var positiveToday)
				? positiveToday
				: null,

			PositiveTotal = ushort.TryParse(Cleanup(tchdCells[4]), out var positiveTotal)
				? positiveTotal
				: null,

			ActiveCases = ushort.TryParse(Cleanup(tchdCells[6]), out var activeCases)
				? activeCases
				: null,

			Hospitalized = ushort.TryParse(Cleanup(tchdCells[7]), out var hospitalized)
				? hospitalized
				: null,

			Deceased = ushort.TryParse(Cleanup(tchdCells[8]), out var deceased)
				? deceased
				: null,

			PartiallyVaccinated = uint.TryParse(Cleanup(tchdCells[9]), out var partiallyVaccinated)
				? partiallyVaccinated
				: null,

			FullyVaccinated = uint.TryParse(Cleanup(tchdCells[10]), out var fullyVaccinated)
				? fullyVaccinated
				: null
		};
	}

	private static ReadOnlySpan<char> Cleanup(INode cell)
		=> cell.TextContent.Replace(",", "");
}