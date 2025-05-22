using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Parquet;
using Parquet.Data;

namespace TestParquet
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Country { get; set; } = "";
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Testing Parquet.NET");

            // Create a sample parquet file with Person records
            var people = new List<Person>
            {
                new Person { Id = 1, Name = "John Doe", Country = "USA" },
                new Person { Id = 2, Name = "Jane Smith", Country = "UK" },
                new Person { Id = 3, Name = "Hans Müller", Country = "Germany" }
            };

            using (var stream = File.Create("people.parquet"))
            {
                var schema = new Parquet.Schema(new List<IField>
                {
                    new DataField<int>("Id"),
                    new DataField<string>("Name"),
                    new DataField<string>("Country")
                });

                using var writer = await ParquetWriter.CreateAsync(schema, stream);

                using var rowGroupWriter = writer.CreateRowGroup();

                var idColumn = new DataColumn(new DataField<int>("Id"), people.Select(p => (object)p.Id).ToArray());
                var nameColumn = new DataColumn(new DataField<string>("Name"), people.Select(p => (object)p.Name).ToArray());
                var countryColumn = new DataColumn(new DataField<string>("Country"), people.Select(p => (object)p.Country).ToArray());

                await rowGroupWriter.WriteColumnAsync(idColumn);
                await rowGroupWriter.WriteColumnAsync(nameColumn);
                await rowGroupWriter.WriteColumnAsync(countryColumn);
            }

            Console.WriteLine("Created parquet file with sample data");

            // Read the parquet file
            var readPeople = new List<Person>();
            using (var stream = File.OpenRead("people.parquet"))
            {
                using var reader = await ParquetReader.CreateAsync(stream);

                for (int i = 0; i < reader.RowGroupCount; i++)
                {
                    using var rowGroupReader = reader.OpenRowGroupReader(i);
                    foreach (var field in reader.Schema.GetDataFields())
                    {
                        DataColumn? columnData = await rowGroupReader.ReadColumnAsync(field);
                        if (field.Name == "Id" && columnData?.Data is int[] ids)
                        {
                            if (field.Name == "Name" && columnData?.Data is string[] names)
                            {
                                if (field.Name == "Country" && columnData?.Data is string[] countries)
                                {
                                    for (int j = 0; j < ids.Length; j++)
                                    {
                                        readPeople.Add(new Person
                                        {
                                            Id = ids[j],
                                            Name = names[j],
                                            Country = countries[j]
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Read data from parquet file:");
            foreach (var person in readPeople)
            {
                Console.WriteLine($"ID: {person.Id}, Name: {person.Name}, Country: {person.Country}");
            }
        }
    }
}
