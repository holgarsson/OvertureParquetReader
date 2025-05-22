using System;
using System.Collections.Generic;
using System.IO;
using Parquet;
using Parquet.Data;

namespace TestParquet
{
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
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

            // Create a schema for the parquet file
            var schemaBuilder = new SchemaDefinition();
            schemaBuilder.AddField(new FieldDefinition("Id", ParquetDataType.Int32));
            schemaBuilder.AddField(new FieldDefinition("Name", ParquetDataType.String));
            schemaBuilder.AddField(new FieldDefinition("Country", ParquetDataType.String));

            // Serialize to parquet
            using (var stream = File.Create("people.parquet"))
            {
                var writer = await ParquetWriter.CreateAsync(schemaBuilder.Build(), stream);
                foreach (var person in people)
                {
                    writer.Write(person.Id, "Id");
                    writer.Write(person.Name, "Name");
                    writer.Write(person.Country, "Country");
                }
                await writer.DisposeAsync();
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
                        if (field.Name == "Id")
                        {
                            DataColumn? columnData = await rowGroupReader.ReadColumnAsync(field);
                            if (columnData?.Data is int[] ids)
                            {
                                for (int j = 0; j < ids.Length; j++)
                                {
                                    var person = new Person
                                    {
                                        Id = ids[j],
                                        Name = ((string[])await rowGroupReader.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "Name")).Result.Data)[j],
                                        Country = ((string[])await rowGroupReader.ReadColumnAsync(reader.Schema.GetDataFields().First(f => f.Name == "Country")).Result.Data)[j]
                                    };
                                    readPeople.Add(person);
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Read {readPeople.Count} people from the parquet file");
            foreach (var person in readPeople)
            {
                Console.WriteLine($"- {person.Name} ({person.Country})");
            }
        }
    }
}
