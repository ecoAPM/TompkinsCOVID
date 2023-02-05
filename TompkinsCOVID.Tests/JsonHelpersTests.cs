using System.Text.Json;
using Xunit;

namespace TompkinsCOVID.Tests;

public sealed class JsonHelpersTests
{
	[Fact]
	public void CanGetUInt16FromJSON()
	{
		//arrange
		var json = JsonSerializer.Deserialize<JsonElement>("{\"test\":\"123\"}");

		//act
		var value = json.GetUInt16("test");

		//assert
		Assert.Equal(123, value!.Value);
	}

	[Fact]
	public void CanGetDecimalFromJSON()
	{
		//arrange
		var json = JsonSerializer.Deserialize<JsonElement>("{\"test\":\"12.34\"}");

		//act
		var value = json.GetDecimal("test");

		//assert
		Assert.Equal(12.34m, value!.Value);
	}

	[Fact]
	public void NonexistentElementReturnsNull()
	{
		//arrange
		var json = JsonSerializer.Deserialize<JsonElement>("{\"test\":\"123\"}");

		//act
		var value = json.GetUInt16("test2");

		//assert
		Assert.Null(value);
	}

	[Fact]
	public void InvalidValueReturnsNull()
	{
		//arrange
		var json = JsonSerializer.Deserialize<JsonElement>("{\"test\":\"123a\"}");

		//act
		var value = json.GetUInt16("test2");

		//assert
		Assert.Null(value);
	}
}