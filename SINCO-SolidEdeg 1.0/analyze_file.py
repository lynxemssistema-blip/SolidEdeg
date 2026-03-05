
import re

file_path = r'c:\Users\adm\Documents\SolidEdeg rev 4.0\Macros\SINCO-SolidEdeg 1.0-24-02-2026-metalfisa AntyGravity\SINCO-SolidEdeg 1.0\SINCO-SolidEdeg 1.0\frmDadosPecaCorrente.vb'

with open(file_path, 'r', encoding='latin-1') as f:
    lines = f.readlines()

# Extract lines 2619 and 2620
query_lines = lines[2618:2620] # 0-indexed
query_string = "".join(query_lines)

# Find all @parameters
params = re.findall(r'@\w+', query_string)
from collections import Counter
counts = Counter(params)

print("Placeholder counts in query:")
found_dup = False
for p, c in counts.items():
    if c > 1:
        print(f"DUPLICATE: {p} ({c} times)")
        found_dup = True
if not found_dup:
    print("No duplicates in query.")

# Check AddWithValue calls in btnInserirnaOS_Click
btn_block = "".join(lines[2622:2682])
add_calls = re.findall(r'AddWithValue\("(@\w+)"', btn_block)
add_counts = Counter(add_calls)

print("\nAddWithValue counts in block:")
found_dup_call = False
for p, c in add_counts.items():
    if c > 1:
        print(f"DUPLICATE CALL: {p} ({c} times)")
        found_dup_call = True
if not found_dup_call:
    print("No duplicate AddWithValue calls.")
