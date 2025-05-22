using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OvertureParquetReader.Models;
using OvertureParquetReader.Services;

namespace OvertureParquetReader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet run <parquet-file-path> <country>");
                return;
            }

            string filePath = args[0];
            string country = args[1];

            var recordReader = new OverturePlaceRecordReader();

            // Test reading places
            List<Place> places = await recordReader.ReadPlacesAsync(filePath);
            Console.WriteLine($"Read {places.Count} places from the parquet file.");

            // Get regions in the specified country
            List<string> regions = await recordReader.GetRegionsByCountry(filePath, country);
            Console.WriteLine($"Found {regions.Count} regions in {country}.");
            Console.WriteLine("Regions:");
            foreach (var region in regions)
            {
                Console.WriteLine($"- {region}");
            }

            // Get restaurants by country and types
            List<string> restaurantTypes = new() { "restaurant", "cafe" };
            List<Place> restaurants = await recordReader.GetRestaurantsByCountryAndTypes(filePath, country, restaurantTypes);
            Console.WriteLine($"Found {restaurants.Count} restaurants in {country}.");

            // Print some details about the first few restaurants
            foreach (var restaurant in restaurants.Take(5))
            {
                if (restaurant.Addresses.Count > 0)
                {
                    var address = restaurant.Addresses[0];
                    Console.WriteLine($"- {restaurant.NamePrimary} ({restaurant.CategoryPrimary}) at {address.Locality}, {address.Region}");
                }
                else
                {
                    Console.WriteLine($"- {restaurant.NamePrimary} ({restaurant.CategoryPrimary}) - No address available");
                }
            }

            // If regions were found, get places in the first region as an example
            if (regions.Count > 0)
            {
                string firstRegion = regions[0];
                Console.WriteLine($"Getting places in {country}, {firstRegion}...");
                List<Place> placesInRegion = await recordReader.GetPlacesByCountryAndRegion(filePath, country, firstRegion);
                Console.WriteLine($"Found {placesInRegion.Count} places in {country}, {firstRegion}.");

                // Print some details about the first few places
                foreach (var place in placesInRegion.Take(5))
                {
                    if (place.Addresses.Count > 0)
                    {
                        var address = place.Addresses[0];
                        Console.WriteLine($"- {place.NamePrimary} ({place.CategoryPrimary}) at {address.Locality}, {address.Region}");
                    }
                    else
                    {
                        Console.WriteLine($"- {place.NamePrimary} ({place.CategoryPrimary}) - No address available");
                    }
                }
            }
        }
    }
}
