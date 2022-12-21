using AngleSharp;
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

	public async Task<IDictionary<DateTime, Record>> GetRecords()
	{
		var html = await _http.GetAsync(_url);
		var content = html.Content.ReadAsStreamAsync();

		var browser = BrowsingContext.New(Configuration.Default);

		async void Request(VirtualResponse r) => r.Content(await content);
		var dom = await browser.OpenAsync(Request);

		var rows = dom.DocumentElement.QuerySelectorAll("tbody tr");
		var records = new Dictionary<DateTime, Record>();
		foreach (var row in rows)
		{
			var cells = row.QuerySelectorAll("td");

			var day = cells[0].TextContent;
			if (string.IsNullOrWhiteSpace(day) || !DateTime.TryParse(day, out var rowDate))
				continue;

			records.Add(rowDate, new Record(cells.ToList()));
		}

		return records;
	}

	public async Task<IDictionary<DateTime, Record>> GetRecordsSince(DateTime latest)
	{
		var records = await GetRecords();

		return records.Where(r => r.Key > latest)
			.ToDictionary(r => r.Key, r => r.Value);
	}
}