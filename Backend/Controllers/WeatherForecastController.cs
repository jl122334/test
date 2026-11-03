using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Cities =
        [
            "New York", "London", "Tokyo", "Sydney", "Paris", "Singapore", "Toronto", "Berlin"
        ];

        private static readonly string[] WindDirections =
        [
            "N", "NE", "E", "SE", "S", "SW", "W", "NW"
        ];

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<IEnumerable<WeatherForecast>> Get(
            [FromQuery] string? city = null,
            [FromQuery] int days = 5)
        {
            if (days is < 1 or > 30)
            {
                return BadRequest("The 'days' parameter must be between 1 and 30.");
            }

            var forecasts = Enumerable.Range(1, days).Select(index =>
            {
                var selectedCity = string.IsNullOrWhiteSpace(city)
                    ? Cities[Random.Shared.Next(Cities.Length)]
                    : city.Trim();

                var temperatureC = Random.Shared.Next(-15, 45);
                var humidity = Random.Shared.Next(25, 95);
                var windSpeed = Math.Round(Random.Shared.NextDouble() * 45 + 2, 1);
                var precipitationChance = humidity > 70
                    ? Random.Shared.Next(40, 95)
                    : Random.Shared.Next(0, 40);

                return new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    City = selectedCity,
                    TemperatureC = temperatureC,
                    FeelsLikeC = CalculateFeelsLikeC(temperatureC, humidity, windSpeed),
                    HumidityPercent = humidity,
                    WindSpeedKmh = windSpeed,
                    WindDirection = WindDirections[Random.Shared.Next(WindDirections.Length)],
                    PrecipitationChancePercent = precipitationChance,
                    UvIndex = CalculateUvIndex(temperatureC, precipitationChance),
                    Summary = GetSummaryForTemperature(temperatureC)
                };
            })
            .ToArray();

            return Ok(forecasts);
        }

        private static string GetSummaryForTemperature(int temperatureC) => temperatureC switch
        {
            <= 0 => "Freezing",
            <= 5 => "Bracing",
            <= 12 => "Chilly",
            <= 18 => "Cool",
            <= 23 => "Mild",
            <= 28 => "Warm",
            <= 32 => "Balmy",
            <= 37 => "Hot",
            <= 42 => "Sweltering",
            _ => "Scorching"
        };

        private static int CalculateFeelsLikeC(int temperatureC, int humidityPercent, double windSpeedKmh)
        {
            // Wind chill effect at low temperatures
            if (temperatureC <= 10 && windSpeedKmh > 15)
            {
                return temperatureC - (int)Math.Round(windSpeedKmh / 10.0);
            }

            // Heat index effect at high temperatures with high humidity
            if (temperatureC >= 27 && humidityPercent > 60)
            {
                return temperatureC + (int)Math.Round((humidityPercent - 60) / 10.0);
            }

            return temperatureC;
        }

        private static int CalculateUvIndex(int temperatureC, int precipitationChance)
        {
            if (precipitationChance > 70 || temperatureC <= 0)
            {
                return Random.Shared.Next(1, 3);
            }

            if (temperatureC >= 28)
            {
                return Random.Shared.Next(7, 12);
            }

            return Random.Shared.Next(3, 7);
        }
    }
}
