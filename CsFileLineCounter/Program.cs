using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsFileLineCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the path to the solution directory:");
            string solutionDirectory = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(solutionDirectory) || !Directory.Exists(solutionDirectory))
            {
                Console.WriteLine("Invalid directory. Exiting program.");
                return;
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string csvFileName = $"file_line_counts_{timestamp}.csv";
            string csvPath = Path.Combine(solutionDirectory, csvFileName);

            var summary = new Dictionary<string, (int NumberOfCsFiles, int NumberOfLines, string PathOfProject)>();

            using (var writer = new StreamWriter(csvPath))
            {
                writer.WriteLine("Sl.No.,Project,File Name,Lines,Path");

                var csprojFiles = Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories);
                int slNo = 1;
                foreach (var csproj in csprojFiles)
                {
                    var projectDir = Path.GetDirectoryName(csproj);
                    var projectName = Path.GetFileNameWithoutExtension(csproj);

                    var csFiles = Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories)
                                           .Where(file => !file.Contains(Path.Combine(projectDir, "obj")) &&
                                                          !file.Contains(Path.Combine(projectDir, "bin")));

                    int projectFileCount = 0;
                    int projectLineCount = 0;

                    foreach (var file in csFiles)
                    {
                        var lineCount = File.ReadLines(file).Count();
                        var fileName = Path.GetFileName(file);
                        writer.WriteLine($"\"{slNo++}\",\"{projectName}\",\"{fileName}\",\"{lineCount}\",\"{file}\"");

                        projectFileCount++;
                        projectLineCount += lineCount;
                    }

                    if (summary.ContainsKey(projectName))
                    {
                        summary[projectName] = (summary[projectName].NumberOfCsFiles + projectFileCount,
                                                summary[projectName].NumberOfLines + projectLineCount,
                                                projectDir);
                    }
                    else
                    {
                        summary.Add(projectName, (projectFileCount, projectLineCount, projectDir));
                    }
                }

                // Write Summary
                writer.WriteLine("\nSummary\n");
                writer.WriteLine("Sl.No.,Project Name,NumberOfCsFiles,NumberOfLines,PathOfProject");
                int summarySlNo = 1;
                foreach (var item in summary)
                {
                    writer.WriteLine($"\"{summarySlNo++}\",\"{item.Key}\",\"{item.Value.NumberOfCsFiles}\",\"{item.Value.NumberOfLines}\",\"{item.Value.PathOfProject}\"");
                }

                // Write Grand Totals
                writer.WriteLine("\nGrand Totals\n");
                writer.WriteLine($"Total Unique Projects,,{summary.Count}");
                writer.WriteLine($"Total .cs Files,,{summary.Sum(s => s.Value.NumberOfCsFiles)}");
                writer.WriteLine($"Total Lines,,{summary.Sum(s => s.Value.NumberOfLines)}");
            }

            Console.WriteLine($"CSV file created at: {csvPath}");
        }
    }
}
