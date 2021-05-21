using Microsoft.Extensions.Configuration;

namespace LikarKrapkaComUI
{
    public static class ConfigurationHelper
    {
        public static T Get<T>(this IConfiguration configuration, string sectionName)
        {
            return configuration.GetSection(sectionName).Get<T>();
        }
    }

}
