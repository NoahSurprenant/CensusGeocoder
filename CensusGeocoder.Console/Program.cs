
namespace CensusGeocoder.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        var service = new GeocodingService();
        service.Vintage = "4";
        //var benchmarks = await service.GetBenchmarks();
        //var vintages = await service.GetVintages("4");

        var foo1 = await service.AddressToGeography(street: "100 N Capitol Ave", city: "Lansing", state: "MI", zip: "48933");
        var foo2 = await service.AddressToLocation(street: "100 N Capitol Ave", city: "Lansing", state: "MI", zip: "48933");

        //await service.BulkFile("C:\\Users\\username\\Documents\\test.csv");

        var foo3 = await service.BulkMemory([new("41253983-84fb-4a25-9650-11ee5ec467fd", StreetAddress: "100 N Capitol Ave", City: "Lansing", State: "MI", Zip: "48933")]);


    }
}
