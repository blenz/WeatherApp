using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Server.Dto;
using Server.Models;
using WeatherApp.Data;
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
        private readonly DataContext _context;

        public WeatherController(
            IOptions<OpenWeatherConfig> openWeatherConfig,
            IOptions<RedisConfig> redisConfig,
            IDistributedCache cache,
            DataContext context
        )
        {
            _openWeatherConfig = openWeatherConfig;
            _redisConfig = redisConfig;
            _cache = cache;
            _context = context;
        }

        [HttpGet]
        public ActionResult<Weather> GetWeather()
        {
            var weather = _context.Weather
                .OrderByDescending(w => w.Id)
                .ToList();

            return Ok(weather);
        }

        [HttpPost]
        public async Task<IActionResult> PostWeather([FromBody] WeatherForCreationDto weatherForCreation)
        {
            // check for cached weather
            var cachedWeather = checkCache(weatherForCreation);
            if (cachedWeather != null)
            {
                cachedWeather.Cached = true;

                // persist to db
                _context.Weather.Add(cachedWeather);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetWeather), new { id = cachedWeather.Id }, cachedWeather);
            }

            // if not cached, call to openweather api
            using(var client = new HttpClient())
            {
                try
                {
                    // query openweather
                    client.BaseAddress = new Uri(_openWeatherConfig.Value.Uri);

                    var query = $"?appid={_openWeatherConfig.Value.Key}&units=imperial&lat={weatherForCreation.Lat}&lon={weatherForCreation.Lng}";

                    var response = await client.GetAsync(query);
                    var content = await response.Content.ReadAsStringAsync();

                    var weatherResponse = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

                    // create new weather
                    var weather = new Weather
                    {
                        Address = weatherForCreation.Address,
                        CurrentTemp = (int) Convert.ToDouble(weatherResponse.Main.CurrentTemp),
                        MinTemp = (int) Convert.ToDouble(weatherResponse.Main.MinTemp),
                        MaxTemp = (int) Convert.ToDouble(weatherResponse.Main.MaxTemp),
                        Zip = weatherForCreation.Zip,
                        Cached = false
                    };

                    // cache the weather
                    cacheWeather(weather);

                    // persist to db
                    _context.Weather.Add(weather);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction(nameof(GetWeather), new { id = weather.Id }, weather);
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

        private Weather checkCache(WeatherForCreationDto weatherForCreationDto)
        {
            // use zip as cache key else address
            var key = weatherForCreationDto.Zip == null
                ? weatherForCreationDto.Address
                : weatherForCreationDto.Zip.ToString();

            var cached = _cache.Get(key);

            if (cached == null)
                return null;

            using(var ms = new MemoryStream(cached))
            {
                var cachedWeather = new BinaryFormatter()
                    .Deserialize(ms) as Weather;

                return cachedWeather;
            }
        }

        private void cacheWeather(Weather weather)
        {
            // use zip as cache key else address
            var key = weather.Zip == null
                ? weather.Address
                : weather.Zip.ToString();

            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_redisConfig.Value.WeatherTtl));

            using(var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, weather);

                _cache.Set(key, ms.ToArray(), options);
            }
        }

    }
}
