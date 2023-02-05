using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public sealed class NYSDOH_CDC : IHealthDepartment
{
	private readonly HttpClient _http;
	private readonly IConfigurationSection _config;

	public NYSDOH_CDC(HttpClient http, IConfigurationSection config)
	{
		_http = http;
		_config = config;
	}

	public async Task<IDictionary<DateOnly, Record>> GetRecords()
		=> await GetRecordsSince(DateOnly.FromDateTime(DateTime.UnixEpoch));

	public async Task<IDictionary<DateOnly, Record>> GetRecordsSince(DateOnly date)
	{
		var all = _config.GetChildren()
			.ToDictionary(c => c.Key, async c => await GetData(c.Key, date));

		var tests = Parse(await all["tests"], "test_date");
		var hospitalizationData = Parse(await all["hospitalizations"], "as_of_date");
		var fatalityData = Parse(await all["fatalities"], "as_of_date");
		var vaccinationData = Parse(await all["vaccinations"], "date");
		var earliestDate = vaccinationData.Min(v => v.Key);

		return tests
			.Where(t => t.Key > date && t.Key >= earliestDate)
			.ToDictionary(t => t.Key, t => ToRecord(t.Key, t.Value, hospitalizationData, fatalityData, vaccinationData));
	}

	private static Record ToRecord(DateOnly date, JsonElement testData, IDictionary<DateOnly, JsonElement> hospitalizationData, IDictionary<DateOnly, JsonElement> fatalityData, IDictionary<DateOnly, JsonElement> vaccinationData)
	{
		if (!hospitalizationData.TryGetValue(date, out var hospitalizations))
			hospitalizations = hospitalizationData.FirstOrDefault(h => h.Key <= date).Value;

		if (!fatalityData.TryGetValue(date, out var fatalities))
			fatalities = fatalityData.FirstOrDefault(h => h.Key <= date).Value;

		if (!vaccinationData.TryGetValue(date, out var vaccinations))
			vaccinations = vaccinationData.FirstOrDefault(h => h.Key <= date).Value;

		return new Record
		{
			Date = DateOnly.FromDateTime(testData.GetProperty("test_date").GetDateTime()).AddDays(1),
			PositiveToday = testData.GetUInt16("new_positives"),
			TestedToday = testData.GetUInt16("total_number_of_tests"),
			PositiveTotal = testData.GetUInt16("cumulative_number_of_positives"),
			Hospitalized = hospitalizations.GetUInt16("patients_currently"),
			Deceased = fatalities.GetUInt16("total_by_place_of_fatality"),
			PartiallyVaccinated = vaccinations.GetDecimal("administered_dose1_pop_pct"),
			FullyVaccinated = vaccinations.GetDecimal("series_complete_pop_pct"),
			VaxxedAndBoosted = vaccinations.GetDecimal("booster_doses_vax_pct"),
			BivalentBoosted = vaccinations.GetDecimal("bivalent_booster_5plus_pop_pct")
		};
	}

	private static IDictionary<DateOnly, JsonElement> Parse(JsonElement data, string dateField)
		=> data.EnumerateArray().ToDictionary(element => DateOnly.FromDateTime(element.GetProperty(dateField).GetDateTime()));

	private async Task<JsonElement> GetData(string type, DateOnly date)
	{
		var url = _config[type]!.Replace("{date}", date.ToString("O"));
		var response = await _http.GetAsync(url);
		var stream = await response.Content.ReadAsStreamAsync();
		var json = await JsonDocument.ParseAsync(stream);
		return json.RootElement;
	}
}