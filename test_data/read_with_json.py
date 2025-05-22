import pyarrow.parquet as pq
import json

# Read the test file
table = pq.read_table('better_test.parquet')

# Convert to JSON for easier inspection
json_str = table.to_pandas().to_json(orient='records')
print("JSON representation:")
print(json.dumps(json.loads(json_str), indent=2))