using ImageResize.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ImageResize
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IImageStorageService, ImageStorageService>(provider =>
               new ImageStorageService(connectionString: Environment.GetEnvironmentVariable("StorageAccount"), logger: provider.GetService<ILogger<ImageStorageService>>())
            );

            services.AddSingleton<IThumbStorageService, ThumbStorageService>(provider =>
               new ThumbStorageService(connectionString: Environment.GetEnvironmentVariable("StorageAccount"), logger: provider.GetService<ILogger<ThumbStorageService>>())
            );

            services.AddSingleton<ILogStorageService, LogStorageService>(provider =>
               new LogStorageService(connectionString: Environment.GetEnvironmentVariable("StorageAccount"), logger: provider.GetService<ILogger<LogStorageService>>())
            );

            services.AddSingleton<IThumbService, ThumbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
