using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace WeatherApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Weather> Weather { get; set; }
    }
}
