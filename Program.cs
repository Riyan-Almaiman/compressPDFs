using System;
using System.IO;
using System.Diagnostics;


Console.Write("Enter the directory path with the PDF files you want to compress: ");


string sourceDirectory = Console.ReadLine().Trim('"');

while (!Directory.Exists(sourceDirectory))
{
    Console.ResetColor();

   
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine("The provided source directory does not exist. Please enter a valid path.");
    Console.ResetColor();

    Console.Write("Enter the directory path with the PDF files you want to compress: ");
    sourceDirectory = Console.ReadLine().Trim('"');
}

Console.Write("Enter the destination directory path: ");
string? destinationDirectory = Console.ReadLine().Trim('"');

while (!Directory.Exists(destinationDirectory))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("The provided destination directory does not exist. Do you want to create it? (yes/no)");
    Console.ResetColor();

    string response = Console.ReadLine().ToLower().Trim('"');

    if (response == "yes" || response == "y")
    {
        Directory.CreateDirectory(destinationDirectory);
        Console.WriteLine("Destination directory created successfully.");
        break;
    }
    else
    {
        Console.WriteLine("Please provide a valid destination directory:");
        destinationDirectory = Console.ReadLine().Trim('"');
    }
}


Console.Clear();
string[] pdfFiles = Directory.GetFiles(sourceDirectory, "*.pdf");

if (pdfFiles.Length == 0)
{
    Console.WriteLine("No PDF files found.");
    return; 
}


Console.WriteLine("Choose a compression quality:");
Console.WriteLine("1. level 1 (low resolution, smallest files)");
Console.WriteLine("2. level 2 (medium resolution)");
Console.WriteLine("3. level 3 (high resolution)");
Console.WriteLine("4. level 4 (highest resolution - recommended)");

string choice = Console.ReadLine();

string compressionSetting = "/prepress";  // default setting

switch (choice)
{
    case "1":
        compressionSetting = "/screen";
        break;
    case "2":
        compressionSetting = "/ebook";
        break;
    case "3":
        compressionSetting = "/printer";
        break;
    case "4":
        compressionSetting = "/prepress";
        break;

    default:
        Console.WriteLine("Invalid choice. Defaulting to level 4 compression.");
        break;
}


Console.Clear();
int processedFilesCount = 0;

foreach (string filePath in pdfFiles)
{
    string fileName = Path.GetFileName(filePath);
    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
    string outputFilePath = Path.Combine(destinationDirectory, fileNameWithoutExtension + "_compressed.pdf");

    CompressPDF(filePath, outputFilePath, compressionSetting);

    processedFilesCount++;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write($"\rProcessed {processedFilesCount} of {pdfFiles.Length} PDFs.");
}
Console.WriteLine(); 



static void CompressPDF(string inputPath, string outputPath, string compressionSetting)
{
    Process process = new Process();

    process.StartInfo.FileName = @"C:\Users\ralmaiman\source\repos\compressPDFs\GhostscriptBinaries\bin\gswin64.exe";
    process.StartInfo.Arguments = $"-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS={compressionSetting} -dNOPAUSE -dBATCH  -sOutputFile=\"{outputPath}\" \"{inputPath}\"";
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.UseShellExecute = false;

    process.Start();
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        Console.WriteLine($"Failed to compress {inputPath}. Exit code: {process.ExitCode}");
    }
  
}


Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("Done");
Console.WriteLine("Compressed pdfs path: " + destinationDirectory);
Console.ReadLine();