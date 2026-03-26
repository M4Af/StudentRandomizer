using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Services
{
    public class FileService
    {
        private readonly string _basePath;

        public FileService()
        {
            _basePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "StudentRandomizer");

            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        public async Task SaveClassAsync(string className, List<string> students, int luckyNumber)
        {
            try
            {
                var filePath = Path.Combine(_basePath, $"{className}.txt");
                var content = new System.Text.StringBuilder();
                content.AppendLine($"CLASS:{className}|LUCKY:{luckyNumber}");
                content.AppendLine("---STUDENTS---");

                foreach (var student in students)
                {
                    content.AppendLine(student);
                }

                await File.WriteAllTextAsync(filePath, content.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving: {ex.Message}");
            }
        }

        public async Task<(List<string> students, int luckyNumber)> LoadClassAsync(string className)
        {
            try
            {
                var filePath = Path.Combine(_basePath, $"{className}.txt");

                if (!File.Exists(filePath))
                    return (new List<string>(), 0);

                var lines = await File.ReadAllLinesAsync(filePath);
                var students = new List<string>();
                int luckyNumber = 0;
                bool inStudentsSection = false;

                foreach (var line in lines)
                {
                    if (line.StartsWith("CLASS:") && line.Contains("LUCKY:"))
                    {
                        var parts = line.Split('|');
                        if (parts.Length > 1)
                        {
                            var luckyPart = parts[1].Replace("LUCKY:", "");
                            luckyNumber = int.Parse(luckyPart);
                        }
                    }
                    else if (line == "---STUDENTS---")
                    {
                        inStudentsSection = true;
                        continue;
                    }
                    else if (inStudentsSection && !string.IsNullOrWhiteSpace(line))
                    {
                        students.Add(line);
                    }
                }

                return (students, luckyNumber);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading: {ex.Message}");
                return (new List<string>(), 0);
            }
        }

        public async Task<List<string>> GetAvailableClassesAsync()
        {
            try
            {
                if (!Directory.Exists(_basePath))
                    return new List<string>();

                var files = Directory.GetFiles(_basePath, "*.txt");
                return files.Select(f => Path.GetFileNameWithoutExtension(f)).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task DeleteClassAsync(string className)
        {
            try
            {
                var filePath = Path.Combine(_basePath, $"{className}.txt");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
