using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;

namespace Azure_AI_102_Samples
{
    internal class Identify_Celebrities
    {
        // Add your Computer Vision subscription key and endpoint
        private static string subscriptionKey = "<enter your key here>";
        private static string endpoint = "<enter your endpoint URL here>";

        private static ComputerVisionClient computervisionClient;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("===== Celebrity Detection Sample =====");
            Console.WriteLine();

            // Initialize the Computer Vision client
            computervisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionKey))
            {
                Endpoint = endpoint
            };

            // Run celebrity detection examples
            await DetectCelebritiesRemote();
            await DetectCelebritiesLocal();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Detect Domain-specific Content - remote
        /// This example detects celebrities in remote images.
        /// </summary>
        private static async Task DetectCelebritiesRemote()
        {
            Console.WriteLine("===== Detect Domain-specific Content - remote =====");

            // URL of one or more celebrities
            string remoteImageUrlCelebs = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files" +
                  "/master/ComputerVision/Images/faces.jpg";

            try
            {
                // Call API with content type (celebrities) and URL
                var detectDomainResultsCelebsRemote = await computervisionClient.AnalyzeImageByDomainAsync("celebrities", remoteImageUrlCelebs);

                // Print detection results with name
                Console.WriteLine("Celebrities in the remote image:");

                if (detectDomainResultsCelebsRemote?.Result != null)
                {
                    var resultJson = JObject.Parse(detectDomainResultsCelebsRemote.Result.ToString());
                    var celebritiesArray = resultJson["celebrities"];

                    if (celebritiesArray == null || !celebritiesArray.HasValues)
                    {
                        Console.WriteLine("No celebrities detected.");
                    }
                    else
                    {
                        foreach (var celeb in celebritiesArray)
                        {
                            string name = celeb["name"]?.ToString();
                            double confidence = celeb["confidence"]?.Value<double>() ?? 0.0;
                            Console.WriteLine($"- {name} (Confidence: {confidence:F4})");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No celebrities detected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detecting celebrities in remote image: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Detect Domain-specific Content - local
        /// This example detects celebrities in local images.
        /// </summary>
        private static async Task DetectCelebritiesLocal()
        {
            Console.WriteLine("===== Detect Domain-specific Content - local =====");

            // Open local image file containing a celebrity
            string localImagePathCelebrity = "Images/Faces.jpg";

            try
            {
                if (!File.Exists(localImagePathCelebrity))
                {
                    Console.WriteLine($"Local image file not found: {localImagePathCelebrity}");
                    Console.WriteLine("Please ensure the image file exists in the specified path.");
                    return;
                }

                using (var localImageCelebrity = File.OpenRead(localImagePathCelebrity))
                {
                    // Call API with the type of content (celebrities) and local image
                    var detectDomainResultsCelebsLocal = await computervisionClient.AnalyzeImageByDomainInStreamAsync("celebrities", localImageCelebrity);

                    // Print which celebrities (if any) were detected
                    Console.WriteLine("Celebrities in the local image:");

                    if (detectDomainResultsCelebsLocal?.Result != null)
                    {
                        var resultJson = JObject.Parse(detectDomainResultsCelebsLocal.Result.ToString());
                        var celebritiesArray = resultJson["celebrities"];

                        if (celebritiesArray == null || !celebritiesArray.HasValues)
                        {
                            Console.WriteLine("No celebrities detected.");
                        }
                        else
                        {
                            foreach (var celeb in celebritiesArray)
                            {
                                string name = celeb["name"]?.ToString();
                                double confidence = celeb["confidence"]?.Value<double>() ?? 0.0;
                                Console.WriteLine($"- {name} (Confidence: {confidence:F4})");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No celebrities detected.");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Local image file not found: {localImagePathCelebrity}");
                Console.WriteLine("Please ensure the image file exists in the specified path.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detecting celebrities in local image: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}
