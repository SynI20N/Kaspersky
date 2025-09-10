#!/bin/bash

# Output CSV filename
OUTPUT_FILE="logs.csv"

# Number of rows to generate (excluding header)
NUM_ROWS=10000

# Define sample product names
PRODUCTS=("AlphaApp" "BetaTool" "GammaService" "DeltaEngine" "OmegaSuite")

# Write header
echo "TimeStamp,Severity,Product,Version,ErrorCode" > "$OUTPUT_FILE"

# Generate rows
for ((i=1; i<=NUM_ROWS; i++)); do
  # Generate timestamp (incremental by minute)
  TIMESTAMP=$(date -d "2009-05-01 + $i seconds" +"%-d/%-m/%Y %-I:%M:%S %p")

  # Random severity (0–3)
  SEVERITY=$((RANDOM % 4))

  # Random product
  PRODUCT=${PRODUCTS[$RANDOM % ${#PRODUCTS[@]}]}

  # Random version (e.g., 1.0.5)
  VERSION="$((RANDOM % 5 + 1)).$((RANDOM % 2)).$((RANDOM % 2))"

  # Random error code (0–100)
  ERROR_CODE=$((RANDOM % 101))

  # Write row to CSV
  echo "$TIMESTAMP,$SEVERITY,$PRODUCT,$VERSION,$ERROR_CODE" >> "$OUTPUT_FILE"
done

echo "CSV file '$OUTPUT_FILE' with $NUM_ROWS rows generated successfully."
