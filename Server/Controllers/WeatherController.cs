using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Dto;
using Server.Models;
using WeatherApp.Helpers;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IOptions<OpenWeatherConfig> _openWeatherConfig;

        public WeatherController(IOptions<OpenWeatherConfig> openWeatherConfig)
        {
            _openWeatherConfig = openWeatherConfig;
        }

        [HttpPost()]
        public async Task<IActionResult> PostWeather([FromBody] WeatherForCreationDto weatherForCreation)
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_openWeatherConfig.Value.Uri);

                    var query = $"?appid={_openWeatherConfig.Value.Key}&units=imperial&lat={weatherForCreation.Lat}&lon={weatherForCreation.Lon}";

                    var response = await client.GetAsync(query);
                    var content = await response.Content.ReadAsStringAsync();

                    var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

                    var weather = new Weather
                    {
                        Address = weatherForCreation.Address,
                        CurrentTemp = float.Parse(weatherResponse.Main.CurrentTemp),
                        MinTemp = float.Parse(weatherResponse.Main.MinTemp),
                        MaxTemp = float.Parse(weatherResponse.Main.MaxTemp),
                        Cached = false
                    };

                    return Ok(weather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }
}
