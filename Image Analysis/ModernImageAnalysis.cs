using System;
using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace Azure_AI_102_Samples
{
    public class ModernImageAnalysis
    {
        // Use the same credentials as CognitiveServices.cs
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_VISION_KEY") ?? "<PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE>";
        private static readonly string endpoint = Environment.GetEnvironmentVariable("AZURE_VISION_ENDPOINT") ?? "<PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE>";

        // Public method to run the modern image analysis
        public static async Task RunImageAnalysisAsync()
        {
            Console.WriteLine("===== Modern Image Analysis =====");
            Console.WriteLine("?? Using Azure AI Vision API (Current/Supported)");
            Console.WriteLine();

            try
            {
                // Create a client
                ImageAnalysisClient client = AuthenticateClient(endpoint, subscriptionKey);

                // Analyze an image to generate captions, tags, people, objects
                await AnalyzeImageUrlAsync(client);

                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("? Modern image analysis complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error in modern image analysis: {ex.Message}");
            }
        }

        /// <summary>
        /// Authenticate Azure AI Vision client
        /// </summary>
        private static ImageAnalysisClient AuthenticateClient(string endpoint, string key)
        {
            var client = new ImageAnalysisClient(
                new Uri(endpoint),
  new AzureKeyCredential(key));

            Console.WriteLine("? Modern Azure AI Vision client authenticated successfully");
            Console.WriteLine($"   Endpoint: {endpoint}");
            Console.WriteLine();

            return client;
        }

        /// <summary>
        /// Analyze image from URL with comprehensive features
        /// </summary>
        private static async Task AnalyzeImageUrlAsync(ImageAnalysisClient client)
        {
            // URL image with people
            const string imageUrl = "https://images.pexels.com/photos/4021773/pexels-photo-4021773.jpeg";

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("ANALYZE IMAGE - URL");
            Console.WriteLine();

            // Creating comprehensive features to be extracted from the image
            VisualFeatures visualFeatures =
         VisualFeatures.Caption |
         VisualFeatures.Tags |
          VisualFeatures.People |
          VisualFeatures.Objects |
       VisualFeatures.DenseCaptions;

            Console.WriteLine($"?? Analyzing the image: {imageUrl}");
            Console.WriteLine();

            // Analyze the URL image 
            ImageAnalysisResult result = await client.AnalyzeAsync(
                   new Uri(imageUrl),
              visualFeatures);

            // Main caption
            if (result.Caption != null)
            {
                Console.WriteLine("?? Main Caption:");
                Console.WriteLine($"   {result.Caption.Text} (Confidence: {result.Caption.Confidence:F4})");
                Console.WriteLine();
            }

            // Dense captions (detailed descriptions of different regions)
            if (result.DenseCaptions != null && result.DenseCaptions.Values.Count > 0)
            {
                Console.WriteLine($"?? Detailed Region Descriptions ({result.DenseCaptions.Values.Count} regions):");
                var topCaptions = result.DenseCaptions.Values.Take(5); // Show top 5
                foreach (var caption in topCaptions)
                {
                    Console.WriteLine($"• {caption.Text} (Confidence: {caption.Confidence:F4})");
                    var bbox = caption.BoundingBox;
                    Console.WriteLine($"     ?? Location: X={bbox.X}, Y={bbox.Y}, Width={bbox.Width}, Height={bbox.Height}");
                }
                Console.WriteLine();
            }

            // People detection
            if (result.People != null && result.People.Values.Count > 0)
            {
                Console.WriteLine($"?? People Detected: {result.People.Values.Count}");
                for (int i = 0; i < result.People.Values.Count; i++)
                {
                    var person = result.People.Values[i];
                    var bbox = person.BoundingBox;
                    var confidenceLevel = person.Confidence switch
                    {
                        >= 0.8f => "?? High",
                        >= 0.5f => "?? Medium",
                        _ => "?? Low"
                    };
                    Console.WriteLine($"   Person {i + 1}: {confidenceLevel} confidence ({person.Confidence:F4})");
                    Console.WriteLine($"     ?? Location: X={bbox.X}, Y={bbox.Y}, Width={bbox.Width}, Height={bbox.Height}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("?? No people detected in this image");
                Console.WriteLine();
            }

            // Objects detection
            if (result.Objects != null && result.Objects.Values.Count > 0)
            {
                Console.WriteLine($"?? Objects Detected: {result.Objects.Values.Count}");
                foreach (var obj in result.Objects.Values)
                {
                    var primaryTag = obj.Tags[0]; // Get the primary tag
                    var bbox = obj.BoundingBox;
                    var confidenceLevel = primaryTag.Confidence switch
                    {
                        >= 0.8f => "??",
                        >= 0.5f => "??",
                        _ => "??"
                    };
                    Console.WriteLine($"   {confidenceLevel} {primaryTag.Name} (Confidence: {primaryTag.Confidence:F4})");
                    Console.WriteLine($"     ?? Location: X={bbox.X}, Y={bbox.Y}, Width={bbox.Width}, Height={bbox.Height}");
                }
                Console.WriteLine();
            }

            // Tags with confidence levels
            if (result.Tags != null && result.Tags.Values.Count > 0)
            {
                Console.WriteLine($"???  Image Tags ({result.Tags.Values.Count} total):");

                // Group tags by confidence level
                var highConfidence = result.Tags.Values.Where(t => t.Confidence >= 0.8f).ToList();
                var mediumConfidence = result.Tags.Values.Where(t => t.Confidence >= 0.5f && t.Confidence < 0.8f).ToList();
                var lowConfidence = result.Tags.Values.Where(t => t.Confidence < 0.5f).ToList();

                if (highConfidence.Any())
                {
                    Console.WriteLine("   ?? High Confidence:");
                    foreach (var tag in highConfidence.Take(10)) // Show top 10
                    {
                        Console.WriteLine($"      • {tag.Name} ({tag.Confidence:F4})");
                    }
                }

                if (mediumConfidence.Any())
                {
                    Console.WriteLine("   ?? Medium Confidence:");
                    foreach (var tag in mediumConfidence.Take(5)) // Show top 5
                    {
                        Console.WriteLine($"      • {tag.Name} ({tag.Confidence:F4})");
                    }
                }

                // Only show low confidence if requested
                if (lowConfidence.Any() && lowConfidence.Count < 5)
                {
                    Console.WriteLine("   ?? Lower Confidence:");
                    foreach (var tag in lowConfidence.Take(3))
                    {
                        Console.WriteLine($"      • {tag.Name} ({tag.Confidence:F4})");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}