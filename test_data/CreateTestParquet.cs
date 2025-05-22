using System;
using System.Collections.Generic;
using System.IO;
using Parquet;
using Parquet.Data;

namespace TestData
{
    public class Place
    {
        public string? Id { get; set; }
        public List<Address> Addresses { get; set; } = new();
        public string? CategoryPrimary { get; set; }
        public string? NamePrimary { get; set; }
    }

    public class Address
    {
        public string? Freeform { get; set; }
        public string? Locality { get; set; }
        public string? Postcode { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating test parquet file...");

            var places = new List<Place>
            {
                new Place
                {
                    Id = "1",
                    NamePrimary = "Test Restaurant",
                    CategoryPrimary = "restaurant",
                    Addresses =
                    {
                        new Address
                        {
                            Country = "Germany",
                            Region = "Bayern",
                            Locality = "Munich"
                        }
                    }
                },
                new Place
                {
                    Id = "2",
                    NamePrimary = "Test Cafe",
                    CategoryPrimary = "cafe",
                    Addresses =
                    {
                        new Address
                        {
                            Country = "Germany",
                            Region = "Berlin",
                            Locality = "Berlin"
                        }
                    }
                },
                new Place
                {
                    Id = "3",
                    NamePrimary = "Test Hotel",
                    CategoryPrimary = "hotel",
                    Addresses =
                    {
                        new Address
                        {
                            Country = "Germany",
                            Region = "Hamburg",
                            Locality = "Hamburg"
                        }
                    }
                },
                new Place
                {
                    Id = "4",
                    NamePrimary = "Test Restaurant 2",
                    CategoryPrimary = "restaurant",
                    Addresses =
                    {
                        new Address
                        {
                            Country = "USA",
                            Region = "California",
                            Locality = "Los Angeles"
                        }
                    }
                }
            };

            var schema = new Parquet.Schema(new List<IField>
            {
                new DataField<string>("Id"),
                new DataField<List<Address>>("Addresses"),
                new DataField<string>("CategoryPrimary"),
                new DataField<string>("NamePrimary")
            });

            using (var stream = File.Create("/workspace/test_data/test.parquet"))
            {
                using var writer = await ParquetWriter.CreateAsync(schema, stream);

                using var rowGroupWriter = writer.CreateRowGroup();

                // Write each field as a separate column
                var idColumn = new DataColumn(new DataField<string>("Id"), places.Select(p => (object)p.Id).ToArray());
                var addressesColumn = new DataColumn(new DataField<List<Address>>("Addresses"), places.Select(p => (object)p.Addresses).ToArray());
                var categoryPrimaryColumn = new DataColumn(new DataField<string>("CategoryPrimary"), places.Select(p => (object)p.CategoryPrimary).ToArray());
                var namePrimaryColumn = new DataColumn(new DataField<string>("NamePrimary"), places.Select(p => (object)p.NamePrimary).ToArray());

                await rowGroupWriter.WriteColumnAsync(idColumn);
                await rowGroupWriter.WriteColumnAsync(addressesColumn);
                await rowGroupWriter.WriteColumnAsync(categoryPrimaryColumn);
                await rowGroupWriter.WriteColumnAsync(namePrimaryColumn);
            }

            Console.WriteLine("Test parquet file created at /workspace/test_data/test.parquet");
        }
    }
}