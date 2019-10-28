using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp.Data;
using WeatherApp.Helpers;

namespace WeatherApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<OpenWeatherConfig>(Configuration.GetSection("OpenWeather"));
            services.Configure<RedisConfig>(Configuration.GetSection("Redis"));

            services.AddCors();

            var redisConfig = Configuration.GetSection("Redis").Get<RedisConfig>();

            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConfig.Configuration;
                option.InstanceName = redisConfig.InstanceName;
            });

            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // run db migrations
            using(var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using(var context = scope.ServiceProvider.GetService<DataContext>())
                {
                    context.Database.Migrate();
                }
            }

            app.UseCors(x =>
                x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
