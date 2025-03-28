namespace CensusGeocoder.Tests;

public class Tests
{
    private GeocodingService service;
    [SetUp]
    public void Setup()
    {
        service = new GeocodingService();
    }

    [Test]
    public async Task OnelineAddressToLocation()
    {
        var response = await service.OnelineAddressToLocation(address: "100 N Capitol Ave, Lansing, MI 48933");
        Assert.That(response.addressMatches.ToList(), Has.Count.EqualTo(1));
        Assert.That(response.addressMatches.First().coordinates.x, Is.EqualTo(-84.553809193124m));
        Assert.That(response.addressMatches.First().coordinates.y, Is.EqualTo(42.73373146656m));
    }


    [Test]
    public async Task AddressToLocation()
    {
        var response = await service.AddressToLocation(street: "100 N Capitol Ave", city: "Lansing", state: "MI", zip: "48933");
        Assert.That(response.addressMatches.ToList(), Has.Count.EqualTo(1));
        Assert.That(response.addressMatches.First().coordinates.x, Is.EqualTo(-84.553809193124m));
        Assert.That(response.addressMatches.First().coordinates.y, Is.EqualTo(42.73373146656m));
    }

    [Test]
    public async Task BulkMemory()
    {
        var response = (await service.BulkMemory([new(UniqueId: "41253983-84fb-4a25-9650-11ee5ec467fd", StreetAddress: "100 N Capitol Ave", City: "Lansing", State: "MI", Zip: "48933")])).ToList();
        Assert.That(response, Has.Count.EqualTo(1));
        Assert.That(response.First().Found, Is.True);
        Assert.That(response.First().Match, Is.EqualTo(Match.Match));
        Assert.That(response.First().Latitude, Is.EqualTo(42.73373146656m));
        Assert.That(response.First().Longitude, Is.EqualTo(-84.553809193124m));
    }

    [Test]
    public async Task HandleComma()
    {
        // Having a comma in the address should encode correctly so the API can parse the CSV
        var response = await service.BulkMemory([new(UniqueId: "foo", StreetAddress: "1700 CAPITAL AVE, SUITE 200", City: "Plano", State: "TX", Zip: "75074")]);
        Assert.That(response, Has.Count.EqualTo(1));
        Assert.That(response.First().Found, Is.True);
        Assert.That(response.First().Match, Is.EqualTo(Match.Match));
        Assert.That(response.First().Latitude, Is.EqualTo(33.011011211636m));
        Assert.That(response.First().Longitude, Is.EqualTo(-96.689146259288m));
    }

    [Test]
    public async Task DoesNotExist()
    {
        // CSV Response should properly parse when there is a response for an address with no match
        var response = (await service.BulkMemory([new(UniqueId: "41253983-84fb-4a25-9650-11ee5ec467fd", StreetAddress: "99999 N Capitol Ave", City: "Lansing", State: "MI", Zip: "48933")])).ToList();
        Assert.That(response, Has.Count.EqualTo(1));
        Assert.That(response.First().Found, Is.False);
        Assert.That(response.First().Match, Is.EqualTo(Match.NoMatch));
        Assert.That(response.First().Latitude, Is.Null);
        Assert.That(response.First().Longitude, Is.Null);
    }

    [Test]
    public async Task GetBenchmarks()
    {
        var response = (await service.GetBenchmarks()).ToList();
        var defaults = response.Select(x => x.isDefault).ToList();
        var benchmarkNames = response.Select(x => x.benchmarkName).ToArray();

        Assert.That(response, Has.Count.GreaterThan(0));

        Assert.Multiple(() =>
        {
            Assert.That(defaults, Has.Exactly(1).EqualTo(true));
            Assert.That(benchmarkNames, Has.Some.EqualTo("Public_AR_Current"));
            Assert.That(benchmarkNames, Has.Some.Matches(@"Public_AR_ACS\d{4}"));
            Assert.That(benchmarkNames, Has.Some.Matches(@"Public_AR_Census\d{4}"));
        });
    }

    [Test]
    public async Task GetVintages()
    {
        var benchmarks = await service.GetBenchmarks();
        var defaultBenchmark = benchmarks.Where(x => x.isDefault).Select(x => x.id).FirstOrDefault();

        Assert.That(defaultBenchmark, Is.Not.Null);

        var response = await service.GetVintages(defaultBenchmark);

        Assert.That(response.vintages.ToList(), Has.Count.GreaterThan(0));
    }
}
