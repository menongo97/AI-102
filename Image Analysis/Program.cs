using System;
using System.Threading.Tasks;
using Azure_AI_102_Samples;

namespace Azure_AI_102_Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Azure AI-102 Image Analysis Samples ===");
            Console.WriteLine();

            bool continueRunning = true;

            while (continueRunning)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. ?? Celebrity Detection (Legacy API)");
                Console.WriteLine("2. ?? Modern Image Analysis");
                Console.WriteLine("3. ?? Exit");
                Console.Write("\nEnter your choice (1-3): ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        await RunCelebrityDetection();
                        break;
                    case "2":
                        await RunModernImageAnalysis();
                        break;
                    case "3":
                        continueRunning = false;
                        Console.WriteLine("?? Goodbye!");
                        break;
                    default:
                        Console.WriteLine("? Invalid choice. Please enter 1, 2, or 3.");
                        break;
                }

                if (continueRunning)
                {
                    Console.WriteLine("\n" + new string('-', 50));
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static async Task RunCelebrityDetection()
        {
            Console.WriteLine("?? Starting Celebrity Detection...");
            Console.WriteLine();

            await Identify_Celebrities.RunCelebrityDetectionAsync();
        }

        private static async Task RunModernImageAnalysis()
        {
            Console.WriteLine("?? Starting Modern Image Analysis...");
            Console.WriteLine();
            Console.WriteLine("??  This feature requires integration with the modern CognitiveServices.cs code.");
            Console.WriteLine("   You can run that separately or integrate it here.");
            Console.WriteLine("   For now, please use the CognitiveServices.cs file directly.");

            await Task.CompletedTask; // Placeholder
        }
    }
}