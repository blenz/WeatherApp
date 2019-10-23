using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_openWeatherConfig.Value.Uri);

                    var response = await client.GetAsync($"{_openWeatherConfig.Value.Path}?q=London&appid={_openWeatherConfig.Value.Key}");

                    return Ok(response);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }
}
