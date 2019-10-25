namespace WeatherApp.Helpers
{
    public class RedisConfig
    {
        public string Configuration { get; set; }
        public string InstanceName { get; set; }
        public int WeatherTtl { get; set; }
    }
}
