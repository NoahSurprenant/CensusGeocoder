namespace CensusGeocoder;

public class LocationDto
{
    public Input input { get; set; } = null!;
    public Addressmatch[] addressMatches { get; set; } = [];
}

