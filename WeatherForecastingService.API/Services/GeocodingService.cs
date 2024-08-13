using Microsoft.Extensions.Options;
using WeatherForecastingService.API.Models;

namespace WeatherForecastingService.API.Services;

public class GeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;

    public GeocodingService(HttpClient httpClient, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<(double lat, double lon)> GetCoordinatesAsync(string city)
    {
        var response = await _httpClient.GetFromJsonAsync<GeocodingResponse[]>(
            $"{_options.BaseUrl}/geo/1.0/direct?q={city}&limit=1&appid={_options.ApiKey}");
        if (response != null && response.Length > 0)
        {
            return (response[0].Lat, response[0].Lon);
        }
        return (0, 0);
    }
}