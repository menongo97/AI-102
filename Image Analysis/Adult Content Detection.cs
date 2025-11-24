using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace Azure_AI_102_Samples
{
    public class Adult_Content_Detection
    {
        // Use the same credentials as other files
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_VISION_KEY") ?? "<Enter Subscription Key here>";
        private static readonly string endpoint = Environment.GetEnvironmentVariable("AZURE_VISION_ENDPOINT") ?? "<Enter endpoint>";

        private static ComputerVisionClient? computervisionClient;

        /// <summary>
        /// Public method to run adult content detection from other classes
        /// </summary>
        public static async Task RunAdultContentDetectionAsync()
        {
            Console.WriteLine("===== Adult Content Detection Sample =====");
            Console.WriteLine("?? Using Azure Computer Vision API for Adult Content Analysis");
            Console.WriteLine("   Analyzes images for adult, racy, and gory content");
            Console.WriteLine();

            try
            {
                // Initialize the Computer Vision client
                computervisionClient = AuthenticateComputerVisionClient(endpoint, subscriptionKey);

                // Run adult content detection examples
                await DetectAdultContentRemoteAsync();
                //await DetectAdultContentLocalAsync();

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("? Adult content detection complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Application error: {ex.Message}");
            }
            finally
            {
                computervisionClient?.Dispose();
            }
        }

        /// <summary>
        /// Authenticate Computer Vision client
        /// </summary>
        private static ComputerVisionClient AuthenticateComputerVisionClient(string endpoint, string key)
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
            {
                Endpoint = endpoint
            };

            Console.WriteLine("? Computer Vision client authenticated successfully");
            Console.WriteLine($"   Endpoint: {endpoint}");
            Console.WriteLine();

            return client;
        }

        /// <summary>
        /// Detect adult content in remote image
        /// </summary>
        private static async Task DetectAdultContentRemoteAsync()
        {
            Console.WriteLine("===== Adult Content Detection - Remote Image =====");

            // URL of a sample image (using a safe sample image)
            const string remoteImageUrl = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/landmark.jpg";

            try
            {
                Console.WriteLine($"?? Analyzing remote image: {remoteImageUrl}");

                // Specify the visual features to analyze
                var features = new List<VisualFeatureTypes?>()
                {
                    VisualFeatureTypes.Adult
                };

                // Call the API
                var analysisResult = await computervisionClient!.AnalyzeImageAsync(remoteImageUrl, features);

                // Display adult content analysis results
                Console.WriteLine("\n?? Adult Content Analysis Results:");
                DisplayAdultContentResults(analysisResult.Adult);
            }
            catch (ComputerVisionErrorResponseException cvEx)
            {
                Console.WriteLine($"? Computer Vision API error: {cvEx.Response.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error analyzing remote image: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Detect adult content in local image
        /// </summary>
        private static async Task DetectAdultContentLocalAsync()
        {
            Console.WriteLine("===== Adult Content Detection - Local Image =====");

            // Try multiple possible paths for local images
            string[] possiblePaths = {
                Path.Combine("Image Analysis", "Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                Path.Combine("Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                "Images/Teach-Handwriting-Step-13-Version-2.jpg"
            };

            string? localImagePath = null;

            // Find the first existing image file
            foreach (string path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    localImagePath = path;
                    break;
                }
            }

            if (localImagePath == null)
            {
                Console.WriteLine("??  No local image found for analysis.");
                Console.WriteLine("   Skipping local image analysis.");
                Console.WriteLine("   Suggested: Add an image file to the Images folder.");
                return;
            }

            try
            {
                Console.WriteLine($"?? Analyzing local image: {localImagePath}");

                // Specify the visual features to analyze
                var features = new List<VisualFeatureTypes?>()
                {
                    VisualFeatureTypes.Adult
                };

                using var imageStream = File.OpenRead(localImagePath);

                // Call the API with local image stream
                var analysisResult = await computervisionClient!.AnalyzeImageInStreamAsync(imageStream, features);

                // Display adult content analysis results
                Console.WriteLine($"\n?? Adult Content Analysis Results for: {Path.GetFileName(localImagePath)}");
                DisplayAdultContentResults(analysisResult.Adult);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"? Local image file not found: {localImagePath}");
            }
            catch (ComputerVisionErrorResponseException cvEx)
            {
                Console.WriteLine($"? Computer Vision API error: {cvEx.Response.Content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error analyzing local image: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Display adult content detection results with detailed analysis
        /// </summary>
        private static void DisplayAdultContentResults(AdultInfo adultInfo)
        {
            if (adultInfo == null)
            {
                Console.WriteLine("   ? No adult content analysis results available");
                return;
            }

            Console.WriteLine("=====================================");

            // Adult Content Analysis
            Console.WriteLine($"?? Adult Content:");
            Console.WriteLine($"   Score: {adultInfo.AdultScore:F4} (0.0 = Safe, 1.0 = Adult)");
            Console.WriteLine($"   Classification: {GetContentClassification(adultInfo.AdultScore)} {GetConfidenceIcon(adultInfo.AdultScore)}");
            Console.WriteLine($"   Is Adult Content: {(adultInfo.IsAdultContent ? "?? YES" : "? NO")}");
            Console.WriteLine();

            // Racy Content Analysis
            Console.WriteLine($"??? Racy Content:");
            Console.WriteLine($"   Score: {adultInfo.RacyScore:F4} (0.0 = Safe, 1.0 = Racy)");
            Console.WriteLine($"   Classification: {GetContentClassification(adultInfo.RacyScore)} {GetConfidenceIcon(adultInfo.RacyScore)}");
            Console.WriteLine($"   Is Racy Content: {(adultInfo.IsRacyContent ? "?? YES" : "? NO")}");
            Console.WriteLine();

            // Gore Content Analysis
            Console.WriteLine($"?? Gore Content:");
            Console.WriteLine($"   Score: {adultInfo.GoreScore:F4} (0.0 = Safe, 1.0 = Gory)");
            Console.WriteLine($"   Classification: {GetContentClassification(adultInfo.GoreScore)} {GetConfidenceIcon(adultInfo.GoreScore)}");
            Console.WriteLine($"   Is Gory Content: {(adultInfo.IsGoryContent ? "?? YES" : "? NO")}");
            Console.WriteLine();

            // Overall Safety Assessment
            bool isSafeContent = !adultInfo.IsAdultContent && !adultInfo.IsRacyContent && !adultInfo.IsGoryContent;

            Console.WriteLine($"??? Overall Safety Assessment:");
            Console.WriteLine($"   Content Safety: {(isSafeContent ? "? SAFE for general audiences" : "?? MAY NOT BE SAFE for general audiences")}");

            // Provide recommendations
            if (!isSafeContent)
            {
                Console.WriteLine($"?? Recommendations:");
                if (adultInfo.IsAdultContent)
                    Console.WriteLine($"   - Content contains adult material");
                if (adultInfo.IsRacyContent)
                    Console.WriteLine($"   - Content may be suggestive or racy");
                if (adultInfo.IsGoryContent)
                    Console.WriteLine($"   - Content may contain graphic/violent material");
                Console.WriteLine($"   - Consider content filtering or age restrictions");
            }
        }

        /// <summary>
        /// Get content classification based on score
        /// </summary>
        private static string GetContentClassification(double score)
        {
            return score switch
            {
                >= 0.8 => "Very High Risk",
                >= 0.6 => "High Risk", 
                >= 0.4 => "Medium Risk",
                >= 0.2 => "Low Risk",
                _ => "Very Low Risk"
            };
        }

        /// <summary>
        /// Get confidence icon based on score
        /// </summary>
        private static string GetConfidenceIcon(double score)
        {
            return score switch
            {
                >= 0.8 => "??",
                >= 0.6 => "??", 
                >= 0.4 => "??",
                >= 0.2 => "??",
                _ => "?"
            };
        }
    }
}