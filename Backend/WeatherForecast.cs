namespace Backend
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public string City { get; set; } = string.Empty;

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)Math.Round(TemperatureC * 9.0 / 5.0);

        public int FeelsLikeC { get; set; }

        public int HumidityPercent { get; set; }

        public double WindSpeedKmh { get; set; }

        public string WindDirection { get; set; } = string.Empty;

        public int PrecipitationChancePercent { get; set; }

        public int UvIndex { get; set; }

        public string? Summary { get; set; }
    }
}
