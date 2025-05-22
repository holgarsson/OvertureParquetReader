using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Parquet;
using Parquet.Data;
using OvertureParquetReader.Models;

namespace OvertureParquetReader.Services
{
    public class OverturePlaceRecordReader
    {
        /// <summary>
        /// Reads places from a parquet file using the Parquet.NET library.
        /// </summary>
        /// <param name="filePath">The path to the parquet file.</param>
        /// <returns>A list of Place objects.</returns>
        public async Task<List<Place>> ReadPlacesAsync(string filePath)
        {
            List<Place> placeList = new();

            using (Stream fileStream = File.OpenRead(filePath))
            {
                // Deserialize all records from the parquet file
                IAsyncEnumerable<Place> places = Parquet.Serialization.ParquetSerializer.DeserializeAllAsync<Place>(fileStream);

                await foreach (var place in places)
                {
                    placeList.Add(place);
                }
            }

            return placeList;
        }

        /// <summary>
        /// Reads and prints information about a parquet file.
        /// </summary>
        /// <param name="filePath">The path to the parquet file.</param>
        public async Task ReadParquetFile(string filePath)
        {
            using Stream fileStream = File.OpenRead(filePath);
            using var parquetReader = await ParquetReader.CreateAsync(fileStream);

            Console.WriteLine($"Schema: {parquetReader.Schema}");

            // Print all fields in the schema
            foreach (var field in parquetReader.Schema.Fields)
            {
                Console.WriteLine($"Field: {field.Name}");
            }

            // Process only the first row group for demonstration purposes
            if (parquetReader.RowGroupCount > 0)
            {
                using ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(0);

                // Print all data fields in the row group
                var dataFields = parquetReader.Schema.GetDataFields();
                foreach (var field in dataFields)
                {
                    Console.WriteLine($"Data Field: {field.Name}");
                }
            }
        }

        /// <summary>
        /// Gets restaurants by country and type from a parquet file.
        /// </summary>
        /// <param name="filePath">The path to the parquet file.</param>
        /// <param name="country">The country to filter by.</param>
        /// <param name="restaurantTypes">A list of restaurant types to filter by.</param>
        /// <returns>A list of Place objects that match the criteria.</returns>
        public async Task<List<Place>> GetRestaurantsByCountryAndTypes(string filePath, string country, List<string> restaurantTypes)
        {
            // Use a more efficient approach by filtering as we read
            var filteredPlaces = new List<Place>();

            using (Stream fileStream = File.OpenRead(filePath))
            {
                IAsyncEnumerable<Place> places = Parquet.Serialization.ParquetSerializer.DeserializeAllAsync<Place>(fileStream);

                await foreach (var place in places)
                {
                    if (place.Addresses != null &&
                        place.Addresses.Any(a => a.Country != null && a.Country.Equals(country, StringComparison.OrdinalIgnoreCase)) &&
                        restaurantTypes.Contains(place.CategoryPrimary ?? "", StringComparer.OrdinalIgnoreCase))
                    {
                        filteredPlaces.Add(place);
                    }
                }
            }

            return filteredPlaces;
        }

        /// <summary>
        /// Gets all regions within a country from the parquet file.
        /// </summary>
        /// <param name="filePath">The path to the parquet file.</param>
        /// <param name="country">The country to filter by.</param>
        /// <returns>A list of unique regions in the specified country.</returns>
        public async Task<List<string>> GetRegionsByCountry(string filePath, string country)
        {
            var regions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (Stream fileStream = File.OpenRead(filePath))
            {
                IAsyncEnumerable<Place> places = Parquet.Serialization.ParquetSerializer.DeserializeAllAsync<Place>(fileStream);

                await foreach (var place in places)
                {
                    if (place.Addresses != null)
                    {
                        foreach (var address in place.Addresses)
                        {
                            if (address.Country != null && address.Country.Equals(country, StringComparison.OrdinalIgnoreCase) &&
                                !string.IsNullOrEmpty(address.Region))
                            {
                                regions.Add(address.Region);
                            }
                        }
                    }
                }
            }

            return regions.ToList();
        }

        /// <summary>
        /// Gets places by country and region.
        /// </summary>
        /// <param name="filePath">The path to the parquet file.</param>
        /// <param name="country">The country to filter by.</param>
        /// <param name="region">The region to filter by.</param>
        /// <returns>A list of Place objects in the specified country and region.</returns>
        public async Task<List<Place>> GetPlacesByCountryAndRegion(string filePath, string country, string region)
        {
            var places = new List<Place>();

            using (Stream fileStream = File.OpenRead(filePath))
            {
                IAsyncEnumerable<Place> allPlaces = Parquet.Serialization.ParquetSerializer.DeserializeAllAsync<Place>(fileStream);

                await foreach (var place in allPlaces)
                {
                    if (place.Addresses != null &&
                        place.Addresses.Any(a =>
                            a.Country != null && a.Country.Equals(country, StringComparison.OrdinalIgnoreCase) &&
                            a.Region != null && a.Region.Equals(region, StringComparison.OrdinalIgnoreCase)))
                    {
                        places.Add(place);
                    }
                }
            }

            return places;
        }
    }
}