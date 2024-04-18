using System;
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

            // Create a unique file name with a timestamp
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string csvFileName = $"file_line_counts_{timestamp}.csv";
            string csvPath = Path.Combine(solutionDirectory, csvFileName);

            using (var writer = new StreamWriter(csvPath))
            {
                writer.WriteLine("Project,File Name,Lines,Path");

                var csprojFiles = Directory.GetFiles(solutionDirectory, "*.csproj", SearchOption.AllDirectories);
                foreach (var csproj in csprojFiles)
                {
                    var projectDir = Path.GetDirectoryName(csproj);
                    var projectName = Path.GetFileNameWithoutExtension(csproj);

                    var csFiles = Directory.GetFiles(projectDir, "*.cs", SearchOption.AllDirectories)
                                           .Where(file => !file.Contains(Path.Combine(projectDir, "obj")) &&
                                                          !file.Contains(Path.Combine(projectDir, "bin")));

                    foreach (var file in csFiles)
                    {
                        var lineCount = File.ReadLines(file).Count();
                        var fileName = Path.GetFileName(file);

                        writer.WriteLine($"\"{projectName}\",\"{fileName}\",{lineCount},\"{file}\"");
                    }
                }
            }

            Console.WriteLine($"CSV file created at: {csvPath}");
        }
    }
}
