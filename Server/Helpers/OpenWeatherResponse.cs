using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherApp.Helpers
{
    public class OpenWeatherResponse
    {
        public string Name { get; set; }

        public IEnumerable<WeatherDescription> Weather { get; set; }

        public Main Main { get; set; }
    }

    public class WeatherDescription
    {
        public string Main { get; set; }
        public string Description { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public string CurrentTemp { get; set; }

        [JsonProperty("temp_min")]
        public string MinTemp { get; set; }

        [JsonProperty("temp_max")]
        public string MaxTemp { get; set; }
    }
}
