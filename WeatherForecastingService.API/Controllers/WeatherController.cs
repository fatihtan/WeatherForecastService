using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WeatherForecastingService.API.Services;

namespace WeatherForecastingService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeatherByCity(string city)
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherByCityAsync(city);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> Get5Day3HourForecastByCity(string city)
        {
            try
            {
                var forecast = await _weatherService.Get5Day3HourForecastByCityAsync(city);
                return Ok(forecast);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("bydate")]
        [SwaggerOperation(Summary = "Fetches weather data for a specific date and city.",
                          Description = "This function is only available for paid plans..")]
        public async Task<IActionResult> GetWeatherByCityAndDate(string city, DateTime date)
        {
            try
            {
                var weatherOnDate = await _weatherService.GetWeatherByCityAndDateAsync(city, date);
                return Ok(weatherOnDate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
