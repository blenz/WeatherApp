using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IOptions<RedisConfig> _redisConfig;
        private readonly IDistributedCache _cache;

        public WeatherController(
            IOptions<OpenWeatherConfig> openWeatherConfig,
            IOptions<RedisConfig> redisConfig,
            IDistributedCache cache
        )
        {
            _openWeatherConfig = openWeatherConfig;
            _redisConfig = redisConfig;
            _cache = cache;
        }

        [HttpPost()]
        public async Task<IActionResult> PostWeather([FromBody] WeatherForCreationDto weatherForCreation)
        {
            // check for cached weather
            var cachedWeather = checkCache(weatherForCreation.Zip);
            if (cachedWeather != null)
            {
                cachedWeather.Cached = true;

                return Ok(cachedWeather);
            }

            // if not cached, call to openweather api
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
                        Zip = weatherForCreation.Zip,
                        Cached = false
                    };

                    // cache the weather
                    cacheWeather(weather);

                    return Ok(weather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

        private Weather checkCache(int zip)
        {
            var cached = _cache.Get(zip.ToString());

            if (cached != null)
            {
                using(var ms = new MemoryStream(cached))
                {
                    var cachedWeather = new BinaryFormatter()
                        .Deserialize(ms) as Weather;

                    return cachedWeather;
                }
            }

            return null;
        }

        private void cacheWeather(Weather weather)
        {
            string zipString = weather.Zip.ToString();

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_redisConfig.Value.WeatherTtl));

            using(var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, weather);

                _cache.Set(zipString, ms.ToArray(), options);
            }
        }

    }
}
