namespace CensusGeocoder;
public class VintageDto
{
    public Vintage[] vintages { get; set; } = [];
    public string selectedBenchmark { get; set; } = null!;
    public Benchmark[] benchmarks { get; set; } = [];
}

public class Vintage
{
    public bool isDefault { get; set; }
    public string id { get; set; } = null!;
    public string vintageName { get; set; } = null!;
    public string vintageDescription { get; set; } = null!;
}
