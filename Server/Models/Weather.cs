namespace Server.Models
{
    public class Weather
    {
        public string Address { get; set; }
        public float CurrentTemp { get; set; }
        public float MinTemp { get; set; }
        public float MaxTemp { get; set; }
        public bool Cached { get; set; }
    }
}
