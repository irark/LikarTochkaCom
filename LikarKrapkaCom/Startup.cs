using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LikarKrapkaComEntities.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.ResponseCompression;

namespace LikarKrapkaComAPI
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

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DBDoctorsContext>(options => options.UseSqlServer(connection));
            services.AddControllers();
            services.AddSignalR(hubOptions => { hubOptions.MaximumReceiveMessageSize = null; });
            services.AddSingleton<ServerHub>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowAllOrigins");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ServerHub>("/ServerHub");
            });
            
            app.UseHttpsRedirection();
            app.UseRouting();
        }

    }
}
