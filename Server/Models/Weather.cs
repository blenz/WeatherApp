using System;

namespace Server.Models
{
    [Serializable()]
    public class Weather
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int CurrentTemp { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public int? Zip { get; set; }
        public bool Cached { get; set; }
    }
}
