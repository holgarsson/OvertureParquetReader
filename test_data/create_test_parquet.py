import pandas as pd
import pyarrow as pa
import pyarrow.parquet as pq

# Create a simple DataFrame with test data
data = {
    'Id': ['1', '2', '3'],
    'NamePrimary': ['Test Restaurant 1', 'Test Cafe 1', 'Test Hotel 1'],
    'CategoryPrimary': ['restaurant', 'cafe', 'hotel']
}

# Create a DataFrame for addresses
addresses = pd.DataFrame([
    {'Freeform': 'Main St 1', 'Locality': 'Munich', 'Region': 'Bayern', 'Country': 'Germany'},
    {'Freeform': 'Berlin St 2', 'Locality': 'Berlin', 'Region': 'Berlin', 'Country': 'Germany'},
    {'Freeform': 'Hamburg St 3', 'Locality': 'Hamburg', 'Region': 'Hamburg', 'Country': 'Germany'}
])

# Create a table with the data
table = pa.Table.from_pandas(pd.DataFrame(data))

# Save as parquet file
pq.write_table(table, '/workspace/test_data/test.parquet')

print("Test parquet file created at /workspace/test_data/test.parquet")