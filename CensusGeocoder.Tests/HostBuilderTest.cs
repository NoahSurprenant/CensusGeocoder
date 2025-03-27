using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CensusGeocoder.Tests;
public class HostBuilderTest
{
    private IHost host;
    [SetUp]
    public void Setup()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.RegisterGeocodingService();
        host = builder.Build();
    }

    [Test]
    public void DependencyInjectionWorks()
    {
        using var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<GeocodingService>();
        Assert.Pass("Successfully injected GeocodingService");
    }

    [TearDown]
    public void TearDown()
    {
        host.Dispose();
    }
}
