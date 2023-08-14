import tkinter as tk
from tkinter import filedialog
import os
import random

# Define the genetic algorithm parameters
num_genes = 5
population_size = 10
num_children = 5
num_generations = 10

# Define the base input file (load and export)
def load_base_input_file(file_path):
    with open(file_path, 'r') as file:
        content = file.read()
    return content

def export_modified_input_file(modified_content, generation, child_index):
    output_folder = output_folder_entry.get()
    output_file_name = f"modified_{generation}_{child_index}.txt"
    output_file_path = os.path.join(output_folder, output_file_name)
    with open(output_file_path, 'w') as file:
        file.write(modified_content)

# Genetic Algorithm Functions

def initialize_individual():
    return [random.uniform(0, 100) for _ in range(num_genes)]  # Set the range for parameter values (0-100)

def modify_input_content(content, genes):
    # Define placeholders for parameters in the content
    placeholders = ["PARAM_SPEED", "PARAM_THROTTLE", "PARAM_RAWTHROTTLE", "PARAM_RAWBRAKE", "PARAM_BRAKEPRESSURE"]
    
    modified_content = content
    for placeholder, gene in zip(placeholders, genes):
        modified_content = modified_content.replace(placeholder, str(gene))
    return modified_content

def run_genetic_algorithm():
    base_input_file = input_file_entry.get()

    for generation in range(num_generations):
        for child_index in range(num_children):
            genes = initialize_individual()
            genes = [round(gene, 2) for gene in genes]  # Round to two decimal places
            input_content = load_base_input_file(base_input_file)
            modified_content = modify_input_content(input_content, genes)
            export_modified_input_file(modified_content, generation, child_index)

# Create the main window
root = tk.Tk()
root.title("Genetic Algorithm for Input File Generation")

# Input file selection
input_file_label = tk.Label(root, text="Select Base Input File:")
input_file_label.pack()
input_file_entry = tk.Entry(root)
input_file_entry.pack()

def browse_input_file():
    input_file_path = filedialog.askopenfilename()
    input_file_entry.delete(0, tk.END)
    input_file_entry.insert(0, input_file_path)

browse_input_button = tk.Button(root, text="Browse", command=browse_input_file)
browse_input_button.pack()

# Output folder entry
output_folder_label = tk.Label(root, text="Output Folder:")
output_folder_label.pack()
output_folder_entry = tk.Entry(root)
output_folder_entry.pack()

# Run genetic algorithm button
run_genetic_algorithm_button = tk.Button(root, text="Run Genetic Algorithm", command=run_genetic_algorithm)
run_genetic_algorithm_button.pack()

# Start the main event loop
root.mainloop()
