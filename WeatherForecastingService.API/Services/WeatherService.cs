using Microsoft.Extensions.Options;
using WeatherForecastingService.API.Models;

namespace WeatherForecastingService.API.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;
    private readonly GeocodingService _geocodingService;

    public WeatherService(HttpClient httpClient, IOptions<WeatherApiOptions> options, GeocodingService geocodingService)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _geocodingService = geocodingService;
    }

    public async Task<CurrentWeatherResponse> GetCurrentWeatherByCityAsync(string city)
    {
        var coordinates = await _geocodingService.GetCoordinatesAsync(city);
        if (coordinates.lat == 0 && coordinates.lon == 0)
        {
            throw new Exception("Geocoding failed");
        }

        var response = await _httpClient.GetFromJsonAsync<CurrentWeatherResponse>(
            $"{_options.BaseUrl}/data/2.5/weather?lat={coordinates.lat}&lon={coordinates.lon}&appid={_options.ApiKey}");
        return response;
    }

    public async Task<ForecastResponse> Get5Day3HourForecastByCityAsync(string city)
    {
        var coordinates = await _geocodingService.GetCoordinatesAsync(city);
        if (coordinates.lat == 0 && coordinates.lon == 0)
        {
            throw new Exception("Geocoding failed");
        }

        var response = await _httpClient.GetFromJsonAsync<ForecastResponse>(
            $"{_options.BaseUrl}/data/2.5/forecast?lat={coordinates.lat}&lon={coordinates.lon}&appid={_options.ApiKey}");
        return response;
    }

    public async Task<WeatherDataOnDate> GetWeatherByCityAndDateAsync(string city, DateTime date)
    {
        var coordinates = await _geocodingService.GetCoordinatesAsync(city);
        if (coordinates.lat == 0 && coordinates.lon == 0)
        {
            throw new Exception("Geocoding failed");
        }

        var timestamp = ((DateTimeOffset)date).ToUnixTimeSeconds();
        var response = await _httpClient.GetAsync(
            $"{_options.BaseUrl}/data/3.0/onecall/timemachine?lat={coordinates.lat}&lon={coordinates.lon}&dt={timestamp}&appid={_options.ApiKey}");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new Exception("Unauthorized access - Your plan does not support historical data.");
        }
        else
        {
            response.EnsureSuccessStatusCode();
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherDataOnDate>();
            return weatherData;
        }
    }
}