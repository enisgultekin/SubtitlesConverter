using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Domain;
using Domain.Models;
using Domain.TextProcessing.Implementation;
using Infrastructure.FileSystem;

namespace SubtitlesConverter.Presentation
{
    class Program
    {
        private static string ToolName => Assembly.GetExecutingAssembly().GetName().Name;

        private static string UsageText =>
            $"{ToolName} <source file>.txt <output file>.srt";

        static void ShowUsage() => Console.WriteLine(UsageText);

        static bool Verify(string[] args)
        {
            if (args.Length == 2)
            {
                return File.Exists(args[0]);
            }

            return false;
        }

        static void Process(FileInfo source, FileInfo destination)
        {
            try
            {
                Subtitles subtitles = new SubtitlesBuilder()
                    .For(new TextFileReader(source))
                    .Using(new LinesTrimmer())
                    .Using(new SentencesBreaker())
                    .Using(new LinesBreaker(95, 45))
                    .Build();
                subtitles.SaveAsSrt(new TextFileWriter(destination));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing text: {ex.Message}");
            }
        }

        static void Main(string[] args)
        {
            if (Verify(args))
                Process(new FileInfo(args[0]), new FileInfo(args[1]));
            else
                ShowUsage();
        }
    }
}