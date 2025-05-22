import pyarrow as pa
import pyarrow.parquet as pq

# Create test data with nested structure
data = {
    'Id': ['1', '2', '3'],
    'NamePrimary': ['Test Restaurant 1', 'Test Cafe 1', 'Test Hotel 1'],
    'CategoryPrimary': ['restaurant', 'cafe', 'hotel'],
    'Addresses': [
        [{'Country': 'Germany', 'Freeform': '123 Main St, Berlin', 'Locality': 'Berlin', 'Postcode': '10115', 'Region': 'Berlin'}],
        [{'Country': 'Germany', 'Freeform': '456 Market St, Munich', 'Locality': 'Munich', 'Postcode': '80331', 'Region': 'Bavaria'}],
        [{'Country': 'Germany', 'Freeform': '789 Oak St, Hamburg', 'Locality': 'Hamburg', 'Postcode': '20095', 'Region': 'Hamburg'}]
    ]
}

# Create a table with the data
table = pa.Table.from_pydict(data)
print("Table schema:", table.schema)

# Write to parquet file
pq.write_table(table, 'better_test.parquet')
print("Created better test parquet file.")