using Microsoft.Extensions.DependencyInjection;

namespace CensusGeocoder;
public static class Extensions
{
    public static void RegisterGeocodingService(this IServiceCollection services)
    {
        services.AddHttpClient<GeocodingService>();
    }
}
