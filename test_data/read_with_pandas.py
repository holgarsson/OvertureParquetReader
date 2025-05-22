import pandas as pd
import pyarrow.parquet as pq

# Read the test file with pandas
df = pq.read_table('better_test.parquet').to_pandas()
print("DataFrame shape:", df.shape)
print("\nColumns:", df.columns.tolist())

# Print first row details
print("\nFirst row:")
for col in df.columns:
    print(f"{col}: {df.iloc[0][col]}")

# Check the structure of Addresses column
if 'Addresses' in df.columns:
    print("\nAddresses column type:", type(df['Addresses'].iloc[0]))
    print("Addresses sample:", df['Addresses'].iloc[0])

    # Try to access nested fields
    if hasattr(df['Addresses'].iloc[0], '__getitem__'):
        address = df['Addresses'].iloc[0][0]  # Get first address in the list
        print("\nFirst address details:")
        for key, value in address.items():
            print(f"{key}: {value}")
else:
    print("No Addresses column found!")