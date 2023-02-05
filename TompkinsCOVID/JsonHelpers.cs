using System.Text.Json;

namespace TompkinsCOVID;

public static class JsonHelpers
{
	public static ushort? GetUInt16(this JsonElement json, string property)
	{
		var value = json.GetValue(property);
		return value is not null && ushort.TryParse(value, out var typed)
			? typed
			: null;
	}
	public static decimal? GetDecimal(this JsonElement json, string property)
	{
		var value = json.GetValue(property);
		return value is not null && decimal.TryParse(value, out var typed)
			? typed
			: null;
	}

	private static string? GetValue(this JsonElement json, string property)
		=> json.TryGetProperty(property, out var element)
			? element.GetString()
			: null;
}