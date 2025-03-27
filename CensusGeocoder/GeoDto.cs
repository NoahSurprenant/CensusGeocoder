namespace CensusGeocoder;


public class GeoDto
{
    public Input input { get; set; } = null!;
    public Addressmatch[] addressMatches { get; set; } = [];
}

public class Input
{
    public Address address { get; set; } = null!;
    public Vintage? vintage { get; set; } // Only on geo response, not location
    public Benchmark benchmark { get; set; } = null!;
}

public class Address
{
    public string city { get; set; } = null!;
    public string street { get; set; } = null!;
    public string state { get; set; } = null!;
}

public class Addressmatch
{
    public Tigerline tigerLine { get; set; } = null!;
    public Geographies? geographies { get; set; } // Only on geo response
    public Coordinates coordinates { get; set; } = null!;
    public Addresscomponents addressComponents { get; set; } = null!;
    public string matchedAddress { get; set; } = null!;
}

public class Tigerline
{
    public string side { get; set; } = null!;
    public string tigerLineId { get; set; } = null!;
}

public class Geographies
{
    public State[] States { get; set; } = [];
    public CombinedStatisticalArea[] CombinedStatisticalAreas { get; set; } = [];
    public CountySubdivision[] CountySubdivisions { get; set; } = [];
    public UrbanArea[] UrbanAreas { get; set; } = [];
    public IncorporatedPlace[] IncorporatedPlaces { get; set; } = [];
    public County[] Counties { get; set; } = [];
    public _2024StateLegislativeDistrictsUpper[] _2024StateLegislativeDistrictsUpper { get; set; } = [];
    public _2024StateLegislativeDistrictsLower[] _2024StateLegislativeDistrictsLower { get; set; } = [];
    public _2020CensusBlocks[] _2020CensusBlocks { get; set; } = [];
    public CensusTract[] CensusTracts { get; set; } = [];
    public _119ThCongressionalDistricts[] _119thCongressionalDistricts { get; set; } = [];
}

public class State
{
    public string STATENS { get; set; } = null!;
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string STUSAB { get; set; } = null!;
    public string OID { get; set; } = null!;    
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string DIVISION { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public string REGION { get; set; } = null!;
    public int OBJECTID { get; set; }   
    public string CENTLON { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
}

public class CombinedStatisticalArea
{
    public string POP100 { get; set; } = null!;
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CSA { get; set; } = null!;
    public string CENTLON { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string HU100 { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
}

public class CountySubdivision
{
    public string COUSUB { get; set; } = null!;
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string COUSUBCC { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string COUSUBNS { get; set; } = null!;
    public string COUNTY { get; set; } = null!;
}

public class UrbanArea
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string UA { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
}

public class IncorporatedPlace
{
    public int DISP_CLR { get; set; }
    public string NECTAPCI { get; set; } = null!;
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string PLACE { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string PLACECC { get; set; } = null!;
    public string CENTLON { get; set; } = null!;
    public string CBSAPCI { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string PLACENS { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
}

public class County
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string COUNTYCC { get; set; } = null!;
    public string COUNTYNS { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string COUNTY { get; set; } = null!;
}

public class _2024StateLegislativeDistrictsUpper
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string SLDU { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string LSY { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string LDTYP { get; set; } = null!;
}

public class _2024StateLegislativeDistrictsLower
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string SLDL { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string LSY { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string LDTYP { get; set; } = null!;
}

public class _2020CensusBlocks
{
    public string SUFFIX { get; set; } = null!;
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string BLOCK { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string TRACT { get; set; } = null!;
    public string CENTLON { get; set; } = null!;
    public string BLKGRP { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string LWBLKTYP { get; set; } = null!;
    public string UR { get; set; } = null!;
    public string COUNTY { get; set; } = null!;
}

public class CensusTract
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string TRACT { get; set; } = null!;
    public string CENTLON { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
    public string COUNTY { get; set; } = null!;
}

public class _119ThCongressionalDistricts
{
    public string GEOID { get; set; } = null!;
    public string CENTLAT { get; set; } = null!;
    public string CDSESSN { get; set; } = null!;
    public string AREAWATER { get; set; } = null!;
    public string BASENAME { get; set; } = null!;
    public string STATE { get; set; } = null!;
    public string OID { get; set; } = null!;
    public string LSADC { get; set; } = null!;
    public string FUNCSTAT { get; set; } = null!;
    public string INTPTLAT { get; set; } = null!;
    public string NAME { get; set; } = null!;
    public int OBJECTID { get; set; }
    public string CENTLON { get; set; } = null!;
    public string CD119 { get; set; } = null!;
    public string AREALAND { get; set; } = null!;
    public string INTPTLON { get; set; } = null!;
    public string MTFCC { get; set; } = null!;
}

public class Coordinates
{
    public decimal x { get; set; }
    public decimal y { get; set; }
}

public class Addresscomponents
{
    public string zip { get; set; } = null!;
    public string streetName { get; set; } = null!;
    public string preType { get; set; } = null!;
    public string city { get; set; } = null!;
    public string preDirection { get; set; } = null!;
    public string suffixDirection { get; set; } = null!;
    public string fromAddress { get; set; } = null!;
    public string state { get; set; } = null!;
    public string suffixType { get; set; } = null!;
    public string toAddress { get; set; } = null!;
    public string suffixQualifier { get; set; } = null!;
    public string preQualifier { get; set; } = null!;
}
