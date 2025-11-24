using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;

namespace Azure_AI_102_Samples
{
    public class Identify_Celebrities
    {
        // Use the same credentials as other files  
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_VISION_KEY") ?? "<PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE>";
        private static readonly string endpoint = Environment.GetEnvironmentVariable("AZURE_VISION_ENDPOINT") ?? "<PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE>";

        private static ComputerVisionClient? computervisionClient;

        // Public method that can be called from other classes
        public static async Task RunCelebrityDetectionAsync()
        {
            Console.WriteLine("===== Celebrity Detection Sample =====");
            Console.WriteLine("⚠️  Note: Celebrity detection uses legacy Azure Computer Vision API");
            Console.WriteLine("   This feature now requires special Microsoft approval.");
            Console.WriteLine();

            try
            {
                // Initialize the Computer Vision client with modern error handling
                computervisionClient = AuthenticateComputerVisionClient(endpoint, subscriptionKey);

                // Run celebrity detection examples
                await DetectCelebritiesRemoteAsync();
                await DetectCelebritiesLocalAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Application error: {ex.Message}");
            }
            finally
            {
                computervisionClient?.Dispose();
            }
        }

        // Keep the original Main method for standalone execution
        public static async Task Main(string[] args)
        {
            await RunCelebrityDetectionAsync();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Authenticate Computer Vision client (Legacy API required for celebrity detection)
        /// </summary>
        private static ComputerVisionClient AuthenticateComputerVisionClient(string endpoint, string key)
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

            Console.WriteLine("✅ Computer Vision client authenticated successfully");
            Console.WriteLine($"   Endpoint: {endpoint}");
            Console.WriteLine();

            return client;
        }

        /// <summary>
        /// Detect Domain-specific Content - remote
        /// This example detects celebrities in remote images.
        /// </summary>
        private static async Task DetectCelebritiesRemoteAsync()
        {
            Console.WriteLine("===== Detect Domain-specific Content - Remote =====");

            // URL of one or more celebrities
            const string remoteImageUrlCelebs = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg";

            try
            {
                Console.WriteLine($"🔍 Analyzing remote image: {remoteImageUrlCelebs}");

                // Call API with content type (celebrities) and URL
                var detectDomainResults = await computervisionClient!.AnalyzeImageByDomainAsync("celebrities", remoteImageUrlCelebs);

                // Print detection results with name
                Console.WriteLine("\n👥 Celebrities detected in remote image:");
                await DisplayCelebrityResults(detectDomainResults);
            }
            catch (ComputerVisionErrorResponseException cvEx)
            {
                Console.WriteLine($"❌ Computer Vision API error: {cvEx.Response.Content}");
                Console.WriteLine("   This error is expected - celebrity detection requires special approval.");
                Console.WriteLine("   Apply for access at: https://aka.ms/celebrityrecognition");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error detecting celebrities in remote image: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Detect Domain-specific Content - local
        /// This example detects celebrities in local images.
        /// </summary>
        private static async Task DetectCelebritiesLocalAsync()
        {
            Console.WriteLine("===== Detect Domain-specific Content - Local =====");

            // Open local image file containing a celebrity
            const string localImagePathCelebrity = "Images/Faces.jpg";

            try
            {
                if (!File.Exists(localImagePathCelebrity))
                {
                    Console.WriteLine($"⚠️  Local image file not found: {localImagePathCelebrity}");
                    Console.WriteLine("   Please ensure the image file exists in the specified path.");
                    Console.WriteLine("   You can download a sample image from:");
                    Console.WriteLine("   https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg");
                    return;
                }

                Console.WriteLine($"🔍 Analyzing local image: {localImagePathCelebrity}");

                using var localImageStream = File.OpenRead(localImagePathCelebrity);

                // Call API with the type of content (celebrities) and local image
                var detectDomainResults = await computervisionClient!.AnalyzeImageByDomainInStreamAsync("celebrities", localImageStream);

                // Print which celebrities (if any) were detected
                Console.WriteLine("\n👥 Celebrities detected in local image:");
                await DisplayCelebrityResults(detectDomainResults);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"❌ Local image file not found: {localImagePathCelebrity}");
                Console.WriteLine("   Please ensure the image file exists in the specified path.");
            }
            catch (ComputerVisionErrorResponseException cvEx)
            {
                Console.WriteLine($"❌ Computer Vision API error: {cvEx.Response.Content}");
                Console.WriteLine("   This error is expected - celebrity detection requires special approval.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error detecting celebrities in local image: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Display celebrity detection results
        /// </summary>
        private static async Task DisplayCelebrityResults(DomainModelResults results)
        {
            await Task.Run(() =>
                {
                    if (results?.Result == null)
                    {
                        Console.WriteLine("   No celebrities detected (null result)");
                        return;
                    }

                    try
                    {
                        var resultJson = JObject.Parse(results.Result.ToString()!);
                        var celebritiesArray = resultJson["celebrities"];

                        if (celebritiesArray == null || !celebritiesArray.HasValues)
                        {
                            Console.WriteLine("   No celebrities detected");
                            return;
                        }

                        var celebrities = new List<(string Name, double Confidence)>();

                        foreach (var celeb in celebritiesArray)
                        {
                            string name = celeb["name"]?.ToString() ?? "Unknown";
                            double confidence = celeb["confidence"]?.Value<double>() ?? 0.0;
                            celebrities.Add((name, confidence));
                        }

                        // Sort by confidence descending
                        celebrities.Sort((a, b) => b.Confidence.CompareTo(a.Confidence));

                        foreach (var (name, confidence) in celebrities)
                        {
                            var confidenceLevel = confidence switch
                            {
                                >= 0.8 => "🟢 High",
                                >= 0.5 => "🟡 Medium",
                                _ => "🔴 Low"
                            };

                            Console.WriteLine($"   ⭐ {name} (Confidence: {confidence:F4} - {confidenceLevel})");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"   ❌ Error parsing results: {ex.Message}");
                    }
                });
        }
    }
}
