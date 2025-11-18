/*
 * Computer Vision SDK QuickStart - MODERNIZED VERSION
 *
 * Examples included:
 *  - Authenticate
 *  - OCR (Read API): Read file from URL
 *  - OCR (Read API): Read file from local
 *
 *  Prerequisites:
 *   - Visual Studio 2019 (or 2017, but note this is a .Net Core console app, not .Net Framework)
 *   - NuGet library: Azure.AI.Vision.ImageAnalysis
 *   - Azure Computer Vision resource from https://ms.portal.azure.com
 *   - Create a .Net Core console app, then copy/paste this Program.cs file into it. Be sure to update the namespace if it's different.
 *
 *How to run:
 *    - Once your prerequisites are complete, press the Start button in Visual Studio.
 *    - Each example displays a printout of its results.
 *
 * References:
 *- .NET SDK: https://docs.microsoft.com/en-us/dotnet/api/overview/azure/ai.vision.imageanalysis-readme
 * - Computer Vision documentation: https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/
 */

using System;
using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Threading.Tasks;
using System.IO;

namespace ComputerVisionQuickstart
{
    class Program
    {
    // <snippet_vars>
        // Add your Computer Vision subscription key and endpoint
        static string subscriptionKey = "PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE";
        static string endpoint = "PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE";
        // </snippet_vars>
        // </snippet_using_and_vars>

        // Download these images (link in prerequisites), or you can use any appropriate image on your local machine.
        private const string ANALYZE_LOCAL_IMAGE = "celebrities.jpg";

        // <snippet_analyze_url>
        // URL image  (image of puppy)
        private const string ANALYZE_URL_IMAGE = "https://moderatorsampleimages.blob.core.windows.net/samples/sample16.png";
     // </snippet_analyze_url>

   static void Main(string[] args)
        {
     Console.WriteLine("Azure Cognitive Services Computer Vision - .NET quickstart example");
            Console.WriteLine();

   // <snippet_main_calls>
// Create a client
        ImageAnalysisClient client = Authenticate(endpoint, subscriptionKey);

    // Analyze an image to generate captions.
  AnalyzeImageUrl(client, ANALYZE_URL_IMAGE).Wait();
    // </snippet_main_calls>

            //  AnalyzeImageLocal(client, ANALYZE_LOCAL_IMAGE).Wait();

            Console.WriteLine("----------------------------------------------------------");
     Console.WriteLine();
            Console.WriteLine("Computer Vision quickstart is complete.");
            Console.WriteLine();
   Console.WriteLine("Press enter to exit...");
  Console.ReadLine();
        }

 // <snippet_auth>
        /*
         * AUTHENTICATE
         * Creates a Computer Vision client used by each example.
         */
   public static ImageAnalysisClient Authenticate(string endpoint, string key)
        {
       ImageAnalysisClient client = new ImageAnalysisClient(
                new Uri(endpoint),
      new AzureKeyCredential(key));
            return client;
        }
        // </snippet_auth>
        /*
      * END - Authenticate
 */

        // <snippet_visualfeatures>
        /* 
         * ANALYZE IMAGE - URL IMAGE
         * Analyze URL image. Extracts captions, and tags.
         */
        public static async Task AnalyzeImageUrl(ImageAnalysisClient client, string imageUrl)
   {
      Console.WriteLine("----------------------------------------------------------");
          Console.WriteLine("ANALYZE IMAGE - URL");
       Console.WriteLine();

            // Creating features to be extracted from the image
     VisualFeatures visualFeatures =
      VisualFeatures.Caption |
        VisualFeatures.Tags;
        // </snippet_visualfeatures>

            Console.WriteLine($"Analyzing the image {Path.GetFileName(imageUrl)}...");
            Console.WriteLine();
     // <snippet_analyze>
            // Analyze the URL image 
  ImageAnalysisResult result = await client.AnalyzeAsync(
new Uri(imageUrl),
        visualFeatures);

            // <snippet_describe>
            // Summarizes the image content.
            Console.WriteLine("Summary:");
            Console.WriteLine($"{result.Caption.Text} with confidence {result.Caption.Confidence:F4}");
            Console.WriteLine();
    // </snippet_describe>

     // <snippet_tags>
            // Image tags and their confidence score
     Console.WriteLine("Tags:");
        foreach (var tag in result.Tags.Values)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence:F4}");
        }
     Console.WriteLine();
            // </snippet_tags>
        }
    /*
         * END - ANALYZE IMAGE - URL IMAGE
         */

      /*
       * ANALYZE IMAGE - LOCAL IMAGE
	     * Analyze local image. Extracts captions, and tags.
       */
        public static async Task AnalyzeImageLocal(ImageAnalysisClient client, string localImage)
        {
     Console.WriteLine("----------------------------------------------------------");
  Console.WriteLine("ANALYZE IMAGE - LOCAL IMAGE");
 Console.WriteLine();

            // Creating features to be extracted from the image
          VisualFeatures visualFeatures =
             VisualFeatures.Caption |
        VisualFeatures.Tags;

            Console.WriteLine($"Analyzing the local image {Path.GetFileName(localImage)}...");
            Console.WriteLine();

      using (Stream analyzeImageStream = File.OpenRead(localImage))
       {
           // Convert stream to BinaryData
      BinaryData imageData = BinaryData.FromStream(analyzeImageStream);

      // Analyze the local image.
             ImageAnalysisResult result = await client.AnalyzeAsync(
   imageData,
       visualFeatures);

       // Summarizes the image content.
 if (result.Caption != null)
     {
           Console.WriteLine("Summary:");
Console.WriteLine($"{result.Caption.Text} with confidence {result.Caption.Confidence:F4}");
         Console.WriteLine();
       }

    // Image tags and their confidence score
          if (result.Tags != null)
    {
      Console.WriteLine("Tags:");
             foreach (var tag in result.Tags.Values)
  {
      Console.WriteLine($"{tag.Name} {tag.Confidence:F4}");
          }
   Console.WriteLine();
     }
            }
    }
        /*
         * END - ANALYZE IMAGE - LOCAL IMAGE
         */
    }
}