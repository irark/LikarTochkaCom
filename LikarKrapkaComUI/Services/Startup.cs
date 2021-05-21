using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using LikarKrapkaComUI.Services.HttpServices;

namespace LikarKrapkaComUI.Services
{
    public static class Startup
    {
        public static void AddHttpServices(this IServiceCollection services)
        {
            services.AddScoped<HospitalsService>();
            services.AddScoped<DoctorsService>();
            services.AddScoped<SpecializationsService>();
            services.AddScoped<OfficesService>();
            services.AddScoped<PatientsService>();
            services.AddScoped<RecordsService>();
        }

        public static void UseHttpServices(this IApplicationBuilder app)
        {
        }
    }

}
