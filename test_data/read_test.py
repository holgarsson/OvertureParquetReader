import pyarrow.parquet as pq
import json

# Read the test file
table = pq.read_table('better_test.parquet')
print("Table schema:", table.schema)

# Convert to JSON for easier inspection
json_str = table.to_pandas().to_json(orient='records', lines=True)
print("\nJSON representation:")
print(json_str)

# Print first record details
first_record = json.loads(json_str.split('\n')[0])
print("\nFirst record details:")
print(f"ID: {first_record['Id']}")
print(f"Name: {first_record['NamePrimary']}")
print(f"Category: {first_record['CategoryPrimary']}")

addresses = first_record.get('Addresses', [])
if addresses:
    print(f"\nAddresses count: {len(addresses)}")
    if len(addresses) > 0:
        print("First address:", json.dumps(addresses[0], indent=2))
else:
    print("\nNo addresses found!")