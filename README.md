# **Generic Classification System (Naive Bayes in C\#)**

A generic machine learning classification system based on the **Naive Bayes** algorithm, developed in C\#. The system is designed to train on any arbitrary dataset (provided via CSV) and supports both an interactive mode (user input via the console) and a batch mode for processing entire input files.

## **Team & Division of Work**

| Team Member | Assigned Classes & Files                                                                                                          |
| :---------- | :-------------------------------------------------------------------------------------------------------------------------------- |
| **NETANEL** | IPredictor.cs, IWriter.cs, NaiveBasePredictor.cs, NavieBaseModel.cs, ConsoleWriter.cs, CSVReader.cs, CSVWriter.cs, PathManager.cs |
| **AVI**     | ITrainer.cs, DataSet.cs, NaiveBaseTrain.cs, Pipeline.cs, Program.cs                                                               |

## **Project Structure & File Tree**

| Folder / File                                | Description                                                                                                          |
| :------------------------------------------- | :------------------------------------------------------------------------------------------------------------------- |
| **SpamClassificationSystem/src/interfaces/** | Contains the system interfaces defining contracts for reading, writing, training, and predicting.                    |
| IPredictor.cs                                | Interface defining the prediction operation for a data sample.                                                       |
| IReader.cs                                   | Interface defining data reading and loading into a data structure.                                                   |
| ISerilizer.cs                                | An empty interface reserved for future serialization extensions.                                                     |
| ITrainer.cs                                  | Interface defining the model training process over a dataset.                                                        |
| IWriter.cs                                   | Interface defining the output writing of prediction results (to console or file).                                    |
| **SpamClassificationSystem/src/models/**     | Contains core classes for data structures, the training algorithm, prediction logic, and the statistical model.      |
| DataSet.cs                                   | Wrapper class holding column headers (Labels), data rows, and the target column.                                     |
| NaiveBasePredictor.cs                        | Implements IPredictor to calculate predicted probabilities for new samples based on the Naive Bayes model.           |
| NaiveBaseTrain.cs                            | Implements ITrainer to perform the training process (calculating prior probabilities and conditional probabilities). |
| NavieBaseModel.cs                            | Model class storing training statistics (labels, probabilities, and handling mechanism for unseen values).           |
| **SpamClassificationSystem/src/utils/**      | Utility classes for input/output handling (CSV reading and writing, console printing).                               |
| ConsoleWriter.cs                             | Implements IWriter to print input rows and their predictions directly to the console.                                |
| CSVReader.cs                                 | Implements IReader to parse CSV files with rigorous validation of headers, rows, and delimiters.                     |
| CSVWriter.cs                                 | Implements IWriter to export predictions directly to a new CSV file inside the output directory.                     |
| **SpamClassificationSystem/src/workflow/**   | Manages the core workflow logic of the application.                                                                  |
| PathManager.cs                               | Path management utility resolving input and output directories.                                                      |
| Pipeline.cs                                  | Orchestrates the main execution flow for both interactive and batch modes (reading, training, predicting, writing).  |
| **SpamClassificationSystem/**                | Root source directory.                                                                                               |
| Program.cs                                   | Application entry point, managing command-line argument parsing and global exception handling.                       |

## **Project Workflow (End-to-End)**

> 1. **Initialization & Argument Parsing:** The application starts at Program.cs, evaluating command-line arguments to determine whether to run in _Interactive mode_ (single training file path provided) or _Batch mode_ (training file and input file paths provided).
> 2. **Training Data Loading:** CSVReader reads the training CSV file, performs integrity validations (unique headers, matching columns, non-empty rows), and returns a DataSet containing headers, rows, and target values.
> 3. **Model Training:** The Train method in NaiveBaseTrain processes the DataSet:

- Calculates prior probabilities for each target label based on relative frequency.
- Calculates conditional probabilities for every combination of label, feature, and value, incorporating Laplace smoothing for unseen values (Unseen Probabilities).

> This process produces and returns a NavieBaseModel instance.

> 4. **Prediction Execution:**

- _Interactive Mode:_ The system prompts the user to enter values for each feature via the console, formats and normalizes the input, and runs NaiveBasePredictor to calculate the highest scoring label. The result is printed via ConsoleWriter.
- _Batch Mode:_ The system reads a batch input file via CSVReader, iterates row-by-row, computes a prediction for each record, and exports the results simultaneously to the console (ConsoleWriter) and a timestamped CSV file in the output directory (CSVWriter).

## **Classes & Methods Reference**

### **interfaces**

> - **IPredictor:**

- Predict(Dictionary\<string, string\> sample): Executes prediction for a given data sample.
  > - **IReader:**
- Read(string path): Reads data from a file path and returns a DataSet.
  > - **ITrainer:**
- Train(DataSet dataSet): Trains the model on the dataset and returns a NavieBaseModel.
  > - **IWriter:**
- WritePrediction(...): Writes or displays the prediction outcome.

### **models**

> - **DataSet:**

- Constructors and getter methods (GetLabels, GetRows, GetTarget) for managing tabular data structures.
  > - **NaiveBasePredictor:**
- NaiveBasePredictor(NavieBaseModel model): Constructor receiving the trained model.
- Predict(Dictionary\<string, string\> sample): Computes Naive Bayes probabilities per label and returns the optimal label.
  > - **NaiveBaseTrain:**
- Train(DataSet dataSet): Manages computation of priors, conditional probabilities, and unseen smoothing across all features.
  > - **NavieBaseModel:**
- Stores labels, prior probabilities dictionary, conditional probabilities, and fallback unseen probabilities.
- GetProbability(string lebel, string feature, string value): Returns conditional probability for a specific combination or falls back to the unseen probability.

### **utils**

> - **ConsoleWriter:**

- WritePrediction(...): Prints row details and prediction result to the console.
  > - **CSVReader:**
- Read(string path) \-\> DataSet: Handles text file reading, line splitting, and data structure validation.
- ParseHeaders(string headerLine) \-\> List\<string\>: Parses and validates file headers.
- ParseRow(...) \-\> Dictionary\<string, string\>: Parses a single CSV line into a value dictionary.
  > - **CSVWriter:**
- CSVWriter(string path, char delimiter): Constructor setting up the output file path and delimiter.
- WritePrediction(...): Writes the header line if not already written, and appends the result row.
- CreateHeaderLine(...) / CreateResultLine(...): Helper methods constructing output line strings.

### **workflow**

> - **PathManager:**

- Manages relative and absolute paths for input and output directories.
  > - **Pipeline:**
- RunInteractive(...): Executes the interactive user session.
- RunBatch(...): Processes batch input rows and outputs predictions.
- UserEnterData(...): Prompts user for feature inputs and formats/normalizes them.

## **Running Instructions**

You can run the project using the .NET CLI from your terminal:

### **Interactive Mode (Real-time user input):**

dotnet run \-- train.csv

### **Batch Mode (Processing an entire input file and generating an automated CSV output):**

dotnet run \-- train.csv input.csv

Output files will be automatically generated inside the root output/ directory with a timestamped filename.
