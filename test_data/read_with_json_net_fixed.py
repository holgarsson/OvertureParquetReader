import json
import pyarrow.parquet as pq

# Read the test file
table = pq.read_table('better_test.parquet')

# Convert to JSON for easier inspection
json_str = table.to_pandas().to_json(orient='records')
print("JSON representation:")
print(json.dumps(json.loads(json_str), indent=2))

# Write a C# compatible version with proper escaping
csharp_compatible = json_str.replace('"[', '"[new object[] {').replace(']"', '}').replace('"{', '{ new Dictionary<string, object> {{').replace('}",', '}},')
csharp_compatible = csharp_compatible.replace('":"', '","').replace('",', '},')

print("\nC# compatible JSON:")
print(csharp_compatible)

# Print the structure in a way that can be used directly in C#
print("\nC# code to parse this structure:")

records = json.loads(json_str)
for i, record in enumerate(records):
    print(f"// Record {i+1}")
    print(f"var record{i} = new {{")
    for key, value in record.items():
        if key == "Addresses":
            print("  Addresses: new List<object> {")
            for addr in value:
                print("    new Dictionary<string, object> {")
                for addr_key, addr_value in addr.items():
                    print(f"      {{ \"{addr_key}\", \"{addr_value}\" }},")
                print("  },")
            print("  },")
        else:
            print(f"  {key}: \"{value}\",")
    print("};")