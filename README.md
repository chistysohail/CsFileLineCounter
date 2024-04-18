# CsFileLineCounter

This project contains a simple .NET 6 console application that scans through a specified directory for C# projects, counts the lines of code in each `.cs` file, and outputs the results to a uniquely named CSV file each time it's run.

## Features

- Scans a solution directory for `.cs` project files.
- Counts lines of code in each `.cs` file, excluding the `obj` and `bin` directories.
- Outputs the count to a CSV file with columns for project name, file name, line count, and file path.
- Generates a unique CSV file each time to avoid conflicts with open files.

After cloning, Run the application with:
dotnet run

When prompted, enter the full path to the solution directory you wish to analyze.

Output
The application will create a CSV file in the root of the specified solution directory with a name following the pattern file_line_counts_yyyymmddHHMMss.csv. This file will contain the following columns:

Project: The name of the project.
File Name: The name of the .cs file.
Lines: The number of lines in the file.
Path: The full path to the .cs file.
Summary at the bottom.
