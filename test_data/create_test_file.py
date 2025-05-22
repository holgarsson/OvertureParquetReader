import pyarrow as pa
import pyarrow.parquet as pq
from datetime import datetime

# Define the Address schema
address_field = pa.field('Address', pa.struct([
    ('Freeform', pa.string()),
    ('Locality', pa.string()),
    ('Postcode', pa.string()),
    ('Region', pa.string()),
    ('Country', pa.string())
]))

# Create some test addresses
addresses = [
    {'Freeform': '123 Main St, Berlin', 'Locality': 'Berlin', 'Postcode': '10115', 'Region': 'Berlin', 'Country': 'Germany'},
    {'Freeform': '456 Market St, Munich', 'Locality': 'Munich', 'Postcode': '80331', 'Region': 'Bavaria', 'Country': 'Germany'},
    {'Freeform': '789 Oak St, Hamburg', 'Locality': 'Hamburg', 'Postcode': '20095', 'Region': 'Hamburg', 'Country': 'Germany'}
]

# Define the Place schema
place_schema = pa.schema([
    ('Id', pa.string()),
    ('NamePrimary', pa.string()),
    ('CategoryPrimary', pa.string()),
    ('Addresses', pa.list_(address_field))
])

# Create some test places with addresses
places = [
    {'Id': '1', 'NamePrimary': 'Test Restaurant 1', 'CategoryPrimary': 'restaurant', 'Addresses': [addresses[0]]},
    {'Id': '2', 'NamePrimary': 'Test Cafe 1', 'CategoryPrimary': 'cafe', 'Addresses': [addresses[1]]},
    {'Id': '3', 'NamePrimary': 'Test Hotel 1', 'CategoryPrimary': 'hotel', 'Addresses': [addresses[2]]}
]

# Convert to Arrow table
place_table = pa.Table.from_pydict({
    'Id': [place['Id'] for place in places],
    'NamePrimary': [place['NamePrimary'] for place in places],
    'CategoryPrimary': [place['CategoryPrimary'] for place in places],
    'Addresses': [[place['Addresses']] for place in places]
}, schema=place_schema)

# Write to parquet file
pq.write_table(place_table, 'realistic_test.parquet')
print("Created realistic test parquet file with addresses.")