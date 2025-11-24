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
                Console.WriteLine("1. ?? Celebrity Detection (Legacy API) - ?? Requires Special Access");
                Console.WriteLine("2. ?? Modern Image Analysis (Current API)");
                Console.WriteLine("3. ??? Hand Writing OCR");
                Console.WriteLine("4. ?? Adult Content Detection");
                Console.WriteLine("5. ?? Exit");
                Console.Write("\nEnter your choice (1-5): ");

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
                        await RunHandwritingOCR();
                        break;
                    case "4":
                        await RunAdultContentDetection();
                        break;
                    case "5":
                        continueRunning = false;
                        Console.WriteLine("?? Goodbye!");
                        break;
                    default:
                        Console.WriteLine("? Invalid choice. Please enter 1, 2, 3, 4, or 5.");
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
            Console.WriteLine("?? Celebrity Detection (Restricted Feature)");
            Console.WriteLine();
            Console.WriteLine("??  IMPORTANT: Celebrity Recognition is now a restricted feature.");
            Console.WriteLine("   Microsoft requires special approval for access.");
            Console.WriteLine("   Apply at: https://aka.ms/celebrityrecognition");
            Console.WriteLine();
            Console.WriteLine("   The following will demonstrate the celebrity detection code,");
            Console.WriteLine("   but will show an error due to feature restrictions.");
            Console.WriteLine();

            // Run it to show the current implementation
            await Identify_Celebrities.RunCelebrityDetectionAsync();
        }

        private static async Task RunModernImageAnalysis()
        {
            Console.WriteLine("?? Starting Modern Image Analysis...");
            Console.WriteLine();

            // Call the modern image analysis
            await ModernImageAnalysis.RunImageAnalysisAsync();
        }

        private static async Task RunHandwritingOCR()
        {
            Console.WriteLine("??? Starting Handwriting OCR Analysis...");
            Console.WriteLine();

            // Call the handwriting conversion method
            await HandWriting_Conversion_From_Image.RunHandwritingConversionAsync();
        }

        private static async Task RunAdultContentDetection()
        {
            Console.WriteLine("?? Starting Adult Content Detection...");
            Console.WriteLine();

            // Call the adult content detection method
            await Adult_Content_Detection.RunAdultContentDetectionAsync();
        }
    }
}