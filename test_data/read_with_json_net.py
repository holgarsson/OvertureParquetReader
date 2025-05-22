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