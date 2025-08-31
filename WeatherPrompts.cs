using System.ComponentModel;
using ModelContextProtocol.Server;

namespace weather;

// これは目的ごとに必要なプロンプトをコマンド化しているようなもん

[McpServerPromptType]
public static class WeatherPrompts
{
    [McpServerPrompt]
    [Description("Get a weather report for a specific location")]
    public static string GetWeatherReport(
        [Description("City name")] string city,
        [Description("Latitude of the location")] double latitude,
        [Description("Longitude of the location")] double longitude)
    {
        return $"""
        Please provide a weather report for {city} at coordinates ({latitude}, {longitude}).
        
        Use the get_forecast tool to fetch the current weather forecast, then format it in a user-friendly way.
        Include:
        - Current conditions
        - Temperature
        - Wind information
        - Detailed forecast
        
        Format the response clearly and concisely.
        """;
    }

    [McpServerPrompt]
    [Description("Check for weather alerts in a US state")]
    public static string CheckStateAlerts(
        [Description("US state code (e.g., CA, NY, TX)")] string stateCode)
    {
        return $"""
        Check for any active weather alerts in {stateCode}.
        
        Use the get_alerts tool to fetch current weather alerts for the state.
        If there are alerts:
        - List them clearly
        - Highlight severity levels
        - Include any instructions
        
        If there are no alerts, confirm that the state is clear of weather warnings.
        """;
    }

    [McpServerPrompt]
    [Description("Compare weather between two locations")]
    public static string CompareWeather(
        [Description("First city name")] string city1,
        [Description("First city latitude")] double lat1,
        [Description("First city longitude")] double lon1,
        [Description("Second city name")] string city2,
        [Description("Second city latitude")] double lat2,
        [Description("Second city longitude")] double lon2)
    {
        return $"""
        Compare the weather between {city1} ({lat1}, {lon1}) and {city2} ({lat2}, {lon2}).
        
        Use the get_forecast tool for both locations and provide:
        - Temperature comparison
        - Weather conditions comparison
        - Wind conditions
        - Which location has better weather and why
        
        Present the comparison in a clear, side-by-side format if possible.
        """;
    }
}