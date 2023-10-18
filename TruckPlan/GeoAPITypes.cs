using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckPlanning
{
    //public class DataSource
    //{
    //    public string SourceName { get; set; }
    //    public string Attribution { get; set; }
    //    public string License { get; set; }
    //    public string URL { get; set; }
    //}

    //public class Timezone
    //{
    //    public string Name { get; set; }
    //    public string OffsetSTD { get; set; }
    //    public int OffsetSTDSeconds { get; set; }
    //    public string OffsetDST { get; set; }
    //    public int OffsetDSTSeconds { get; set; }
    //    public string AbbreviationSTD { get; set; }
    //    public string AbbreviationDST { get; set; }
    //}

    //public class Rank
    //{
    //    public double Importance { get; set; }
    //    public double Popularity { get; set; }
    //}

    //public class Properties
    //{
    //    public DataSource DataSource { get; set; }
    //    public string Name { get; set; }
    //    public string Country { get; set; }
    //    public string CountryCode { get; set; }
    //    public string State { get; set; }
    //    public string City { get; set; }
    //    public string Postcode { get; set; }
    //    public string District { get; set; }
    //    public string Suburb { get; set; }
    //    public string Street { get; set; }
    //    public string HouseNumber { get; set; }
    //    public double Lon { get; set; }
    //    public double Lat { get; set; }
    //    public int Distance { get; set; }
    //    public string ResultType { get; set; }
    //    public string Formatted { get; set; }
    //    public string AddressLine1 { get; set; }
    //    public string AddressLine2 { get; set; }
    //    public string Category { get; set; }
    //    public Timezone Timezone { get; set; }
    //    public string PlusCode { get; set; }
    //    public Rank Rank { get; set; }
    //    public string PlaceId { get; set; }
    //}

    //public class Geometry
    //{
    //    public string Type { get; set; }
    //    public List<double> Coordinates { get; set; }
    //}

    //public class Feature
    //{
    //    public string Type { get; set; }
    //    public Properties Properties { get; set; }
    //    public Geometry Geometry { get; set; }
    //    public List<double> Bbox { get; set; }
    //}

    //public class Query
    //{
    //    public double Lat { get; set; }
    //    public double Lon { get; set; }
    //    public string PlusCode { get; set; }
    //}

    //public class RootObject
    //{
    //    public string Type { get; set; }
    //    public List<Feature> Features { get; set; }
    //    public Query Query { get; set; }
    //}

    public record DataSource(
    string SourceName,
    string Attribution,
    string License,
    string URL
);

    public record Timezone(
        string Name,
        string OffsetSTD,
        int OffsetSTDSeconds,
        string OffsetDST,
        int OffsetDSTSeconds,
        string AbbreviationSTD,
        string AbbreviationDST
    );

    public record Rank(
        double Importance,
        double Popularity
    );

    public record Properties(
        DataSource DataSource,
        string Name,
        string Country,
        string CountryCode,
        string State,
        string City,
        string Postcode,
        string District,
        string Suburb,
        string Street,
        string HouseNumber,
        double Lon,
        double Lat,
        double Distance,
        string ResultType,
        string Formatted,
        string AddressLine1,
        string AddressLine2,
        string Category,
        Timezone Timezone,
        string PlusCode,
        Rank Rank,
        string PlaceId
    );

    public record Geometry(
        string Type,
        List<double> Coordinates
    );

    public record Feature(
        string Type,
        Properties Properties,
        Geometry Geometry,
        List<double> Bbox
    );

    public record Query(
        double Lat,
        double Lon,
        string PlusCode
    );

    public record GeoResponse(
        string Type,
        List<Feature> Features,
        Query Query
    );


}
