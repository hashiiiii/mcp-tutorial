using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace weather;

// GET みたいなもん - 読み取り専用のデータ提供
// 実際には　Tools を実行する前に AI がコンテキストを強化するために事前に参照しておくもの

[McpServerResourceType]
public static class WeatherResources
{
    private static readonly string DataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
    
    [McpServerResource]
    [Description("Current weather API documentation")]
    public static string WeatherApiDocs()
    {
        return """
        # Weather API Documentation
        
        ## Base URL
        https://api.weather.gov
        
        ## Main Endpoints
        - GET /points/{latitude},{longitude} - Get forecast URL for location
        - GET /alerts/active/area/{state} - Get active weather alerts for a US state
        - GET /gridpoints/{office}/{x},{y}/forecast - Get forecast for grid point
        
        ## Rate Limiting
        - Default rate limit: 60 requests per minute
        - User-Agent header is required
        
        ## Response Format
        All responses are in GeoJSON format with weather-specific properties
        """;
    }

    [McpServerResource]
    [Description("Major US cities with coordinates for weather lookups")]
    public static string CityLocations()
    {
        try
        {
            var filePath = Path.Combine(DataPath, "weather-locations.json");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            
            // フォールバック：ファイルが見つからない場合
            return JsonSerializer.Serialize(new 
            {
                error = "Location data file not found",
                fallback = new[]
                {
                    new { city = "New York", state = "NY", latitude = 40.7128, longitude = -74.0060 },
                    new { city = "Los Angeles", state = "CA", latitude = 34.0522, longitude = -118.2437 }
                }
            }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = $"Failed to load locations: {ex.Message}" });
        }
    }

    [McpServerResource]
    [Description("US state codes and names for weather alerts")]
    public static string UsStateCodes()
    {
        try
        {
            var filePath = Path.Combine(DataPath, "us-states.json");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            
            // フォールバック：主要な州のみ
            return JsonSerializer.Serialize(new
            {
                error = "State codes file not found",
                fallback = new[]
                {
                    new { code = "CA", name = "California" },
                    new { code = "NY", name = "New York" },
                    new { code = "TX", name = "Texas" },
                    new { code = "FL", name = "Florida" }
                }
            }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = $"Failed to load state codes: {ex.Message}" });
        }
    }

    [McpServerResource]
    [Description("Weather terminology and definitions")]
    public static string WeatherTerminology()
    {
        try
        {
            var filePath = Path.Combine(DataPath, "weather-terminology.json");
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            
            // フォールバック：基本用語のみ
            return JsonSerializer.Serialize(new
            {
                error = "Terminology file not found",
                fallback = new
                {
                    alerts = new
                    {
                        watch = "Conditions are favorable for severe weather",
                        warning = "Severe weather is imminent or occurring",
                        advisory = "Weather may cause inconvenience"
                    }
                }
            }, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = $"Failed to load terminology: {ex.Message}" });
        }
    }
}