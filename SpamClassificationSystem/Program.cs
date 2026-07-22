using SpamClassificationSystem.src.interfaces;
using SpamClassificationSystem.src.models;
using SpamClassificationSystem.src.utils;
using SpamClassificationSystem.src.workflow;
using System;
using System.IO;

namespace SpamClassificationSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Creates the objects required by the pipeline.
                IReader reader = new CSVReader();
                ITrainer trainer = new NaiveBaseTrain();

                Pipeline pipeline = new Pipeline(reader,trainer);

                // Interactive mode:
                // Receives only the training file path.
                if (args.Length == 1)
                {
                    IWriter consoleWriter = new ConsoleWriter();

                    pipeline.RunInteractive(args[0],consoleWriter);
                }

                // Batch mode:
                // Receives the training file and input file paths.
                else if (args.Length == 2)
                {
                    IWriter consoleWriter = new ConsoleWriter();

                    string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(),"output");

                    Directory.CreateDirectory(outputDirectory);

                    string inputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(args[1]);

                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                    string outputFileName = $"{inputFileNameWithoutExtension}_output_{currentDate}.csv";

                    string outputPath = Path.Combine(outputDirectory,outputFileName);

                    IWriter csvWriter = new CSVWriter(outputPath);

                    pipeline.RunBatch(args[0],args[1],consoleWriter,csvWriter);

                    Console.WriteLine();
                    Console.WriteLine($"Predictions were written to: {outputPath}");
                }
                else
                {
                    PrintUsage();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {Path.GetFileName(ex.FileName)}");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Invalid CSV data: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid argument: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine();

            Console.WriteLine("Interactive mode:");
            Console.WriteLine("dotnet run -- train.csv");

            Console.WriteLine();

            Console.WriteLine("Batch mode:");
            Console.WriteLine("dotnet run -- train.csv input.csv");
        }
    }
}