using System;
using System.Diagnostics;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;

namespace dotYT3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This tool may not be used to download video or audio content from YouTube you do not either own or have specific permission to access. \nThis is designed to be a recovery tool only");
            Console.WriteLine("I understand this Y/N");
            var answer = Console.ReadLine();
            if (answer.ToLower().Contains("n"))
            {
                Environment.Exit(-1);
            }

            Console.WriteLine("Enter desitination folder:");
            var source = Console.ReadLine();

            var result = source.Substring(source.Length - 1);
            if (result != "/")
            {
                source = source + @"\";
            }

            var youtube = YouTube.Default;

            Console.WriteLine("Enter YouTube URL:");
            var vidSource = Console.ReadLine();

            Console.WriteLine("Would you like to recover the (A)udio or (V)ideo?");
            var recoveryChoice = Console.ReadLine();
            if (recoveryChoice.ToLower() == "a")
            {
                var vid = youtube.GetVideo(vidSource);
                Console.WriteLine("\nDownloading video data...");
                File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

                var inputFile = new MediaFile { Filename = source + vid.FullName };
                var outputFile = new MediaFile { Filename = $"{source + vid.FullName.Replace(".mp4", "")}.mp3" };

                if (File.Exists(outputFile.Filename))
                {
                    File.Delete(source + vid.FullName);
                    Console.WriteLine("\nThis file already exists in this folder and download will not continue.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }
                else
                {
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);

                        Console.WriteLine("\nConverting to mp3...");
                        engine.Convert(inputFile, outputFile);

                    }
                    Console.WriteLine("\nCleaning everything up...");
                    File.Delete(source + vid.FullName);

                    if (File.Exists(outputFile.Filename))
                    {
                        Console.WriteLine("\n" + outputFile.Filename + " download COMPLETED. \nPress any key to continue...");
                        Console.ReadLine();
                        Process.Start(source);
                    }
                    else
                    {
                        Console.WriteLine("\n" + outputFile.Filename + " download FAILED. \nPress any key to continue...");
                        Console.ReadLine();
                    }
                }
            }
            else if (recoveryChoice.ToLower() == "v")
            {
                var vid = youtube.GetVideo(vidSource);
                Console.WriteLine("\nDownloading video data...");
                File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

                var file = new MediaFile { Filename = source + vid.FullName };
                if (File.Exists(file.Filename))
                {
                    Console.WriteLine("\n" + file.Filename + " download COMPLETED. \nPress any key to continue...");
                    Console.ReadLine();
                    Process.Start(source);
                }
                else
                {
                    Console.WriteLine("\n" + file.Filename + " download FAILED. \nPress any key to continue...");
                    Console.ReadLine();
                }
            }

        }
    }
}
