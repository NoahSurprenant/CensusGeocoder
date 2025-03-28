﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CensusGeocoder;

//https://geocoding.geo.census.gov/geocoder/Geocoding_Services_API.pdf
public class GeocodingService
{
    private readonly HttpClient _httpClient;
    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public GeocodingService()
    {
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Can be set to either the name or id, ex: Public_AR_Current or 4
    /// </summary>
    public string Benchmark { get; set; } = "Public_AR_Current";

    public string? Vintage { get; set; }
    private string _vintage => Vintage ?? throw new ArgumentNullException(nameof(Vintage));

    public async Task BulkFile(string file, CancellationToken ct = default)
    {
        using var content = new StreamContent(File.OpenRead(file));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        await Bulk(content, ct);
    }

    public async Task<List<BulkLineResponse>> BulkMemory(BulkLine[] lines, CancellationToken ct = default)
    {
        var sb = new StringBuilder();
        foreach (var line in lines)
        {
            sb.Append(Encode(line.UniqueId));
            sb.Append(',');
            sb.Append(Encode(line.StreetAddress));
            sb.Append(',');
            sb.Append(Encode(line.City));
            sb.Append(',');
            sb.Append(Encode(line.State));
            sb.Append(',');
            sb.Append(Encode(line.Zip));
            sb.Append('\n');
        }
        string csvContent = sb.ToString();
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        using var content = new StreamContent(memoryStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        var response = await Bulk(content, ct);

        using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
        var list = new List<BulkLineResponse>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new Exception("Line was null");
            var fields = line.Split("\",\"");
            var uniqueID = Parse(fields[0]);
            var input = Parse(fields[1]);
            var matchStr = Parse(fields[2]);
            var found = matchStr switch
            {
                "Match" => true,
                "Tie" => false,
                "No_Match" => false,
                _ => throw new ArgumentOutOfRangeException(nameof(matchStr), matchStr, $"{nameof(matchStr)} was out of range. Expected Match, No_Match, or Tie, but got {matchStr}"),
            };
            var match = matchStr switch
            {
                "Match" => Match.Match,
                "Tie" => Match.Tie,
                "No_Match" => Match.NoMatch,
                _ => throw new ArgumentOutOfRangeException(nameof(matchStr), matchStr, $"{nameof(matchStr)} was out of range. Expected Match, No_Match or Tie, but got {matchStr}"),
            };
            var matchType = found ? Parse(fields[3]) : null;
            var validatedAddress = found ? Parse(fields[4]) : null;
            var coordinates = found ? Parse(fields[5]) : null;
            decimal? lat = null;
            decimal? lon = null;
            if (coordinates is not null)
            {
                var split = coordinates.Split(',');
                lat = decimal.Parse(split[1]);
                lon = decimal.Parse(split[0]);
            }
            var tigerLine1 = found ? Parse(fields[6]) : null;
            var tigerLine2 = found ? Parse(fields[7]) : null;
            list.Add(
                new BulkLineResponse(UniqueId: uniqueID, Input: input, Found: found, Match: match,
                MatchKind: matchType, ValidatedAddress: validatedAddress, lat, lon, TigerLineId: tigerLine1, TigerLineSide: tigerLine2)
                );
        }
        return list;
    }

    private string Encode(string raw)
    {
        // Commas have to be surrounded by quotes
        return raw.Replace(",", "\",\"");
    }

    private string Parse(string raw)
    {
        return raw.Trim('"').Trim();
    }

    private async Task<HttpResponseMessage> Bulk(HttpContent content, CancellationToken ct = default)
    {
        var returnType = ReturnType.locations;
        var uri = $"https://geocoding.geo.census.gov/geocoder/{returnType}/addressbatch";

        using var multipartFormContent = new MultipartFormDataContent
        {
            { content, "addressFile", "localfile.csv" },
            { new StringContent(Benchmark), "benchmark" },
        };

        //vintage is required for geographies
        if (returnType is ReturnType.geographies)
            multipartFormContent.Add(new StringContent(_vintage), "vintage");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(uri),
            Content = multipartFormContent
        };

        return await _httpClient.SendAsync(request, ct);
    }

    public async Task<LocationDto> OnelineAddressToLocation(string address, CancellationToken ct = default)
    {
        var response = await OnelineAddress(ReturnType.locations, address, ct);
        var dto = JsonSerializer.Deserialize<LocationResponse>(response)!;
        return dto.result;
    }

    public async Task<GeoDto> OnelineAddressToGeography(string address, CancellationToken ct = default)
    {
        var response = await OnelineAddress(ReturnType.geographies, address, ct);
        var dto = JsonSerializer.Deserialize<GeoResponse>(response)!;
        return dto.result;
    }

    public async Task<LocationDto> AddressToLocation(string street, string? city, string? state, string? zip, CancellationToken ct = default)
    {
        var response = await Address(ReturnType.locations, street, city, state, zip, ct);
        var dto = JsonSerializer.Deserialize<LocationResponse>(response)!;
        return dto.result;
    }

    public async Task<GeoDto> AddressToGeography(string street, string? city, string? state, string? zip, CancellationToken ct = default)
    {
        using var response = await Address(ReturnType.geographies, street, city, state, zip, ct);
        var dto = JsonSerializer.Deserialize<GeoResponse>(response)!;
        return dto.result;
    }

    private async Task<Stream> OnelineAddress(ReturnType returnType, string address, CancellationToken ct = default)
    {
        var searchType = SearchType.onelineaddress;
        var uri = $"https://geocoding.geo.census.gov/geocoder/{returnType}/{searchType}?benchmark={Benchmark}&address={address}&format=json";

        //vintage is required for geographies
        if (returnType is ReturnType.geographies)
            uri += $"&vintage={_vintage}";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await _httpClient.SendAsync(request, ct);
        return await response.Content.ReadAsStreamAsync(ct);
    }

    private async Task<Stream> Address(ReturnType returnType, string street, string? city, string? state, string? zip, CancellationToken ct = default)
    {
        var searchType = SearchType.address;
        if (zip is null && (city is null && state is null))
            throw new Exception("At minimum city and state must be provided, or zip must be provided");

        var uri = $"https://geocoding.geo.census.gov/geocoder/{returnType}/{searchType}?benchmark={Benchmark}&street={street}&format=json";

        if (city is not null)
            uri += $"&city={city}";

        if (state is not null)
            uri += $"&state={state}";

        if (zip is not null)
            uri += $"&city={zip}";

        //vintage is required for geographies
        if (returnType is ReturnType.geographies)
            uri += $"&vintage={_vintage}";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await _httpClient.SendAsync(request, ct);

        return await response.Content.ReadAsStreamAsync(ct);
    }

    public async Task<GeoDto> Coordinates(decimal x, decimal y, CancellationToken ct = default)
    {
        var returnType = ReturnType.geographies;
        var searchType = SearchType.coordinates;
        var uri = $"https://geocoding.geo.census.gov/geocoder/{returnType}/{searchType}?benchmark={Benchmark}&x={x}&y={y}&vintage={_vintage}&format=json";

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        var response = await _httpClient.SendAsync(request, ct);
        return JsonSerializer.Deserialize<GeoResponse>(await response.Content.ReadAsStreamAsync(ct))!.result;
    }

    public async Task<Benchmark[]> GetBenchmarks(CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://geocoding.geo.census.gov/geocoder/benchmarks");
        var response = await _httpClient.SendAsync(request, ct);
        var dto = JsonSerializer.Deserialize<BenchmarkResponse>(await response.Content.ReadAsStreamAsync(ct))!;
        return dto.benchmarks;
    }

    /// <summary>
    /// You must provide Benchmark ID, not name
    /// </summary>
    public async Task<VintageDto> GetVintages(string benchmarkID, CancellationToken ct = default)
    {
        //Ex: https://geocoding.geo.census.gov/geocoder/vintages?benchmark=4
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://geocoding.geo.census.gov/geocoder/vintages?benchmark={benchmarkID}");
        var response = await _httpClient.SendAsync(request, ct);
        var dto = JsonSerializer.Deserialize<VintageDto>(await response.Content.ReadAsStreamAsync(ct))!;
        return dto;
    }

    // Private DTOs that can be mapped away
    private class BenchmarkResponse
    {
        public Benchmark[] benchmarks { get; set; } = [];
    }

    private class GeoResponse
    {
        public GeoDto result { get; set; } = null!;
    }

    private class LocationResponse
    {
        public LocationDto result { get; set; } = null!;
    }
}

public enum ReturnType
{
    locations,
    geographies,
}

public enum Match
{
    Match,
    NoMatch,
    Tie,
}

public enum SearchType
{
    onelineaddress,
    address,
    //addressPR, // I do not care about PR support
    coordinates,
}

public record BulkLine(string UniqueId, string StreetAddress, string City, string State, string Zip);
public record BulkLineResponse(string UniqueId, string Input, bool Found, Match Match, string? MatchKind, string? ValidatedAddress, decimal? Latitude, decimal? Longitude, string? TigerLineId, string? TigerLineSide);