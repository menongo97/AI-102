using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace Azure_AI_102_Samples
{
    public class HandWriting_Conversion_From_Image
    {
        // Use the same credentials as CognitiveServices.cs
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_VISION_KEY") ?? "<PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE>";
        private static readonly string endpoint = Environment.GetEnvironmentVariable("AZURE_VISION_ENDPOINT") ?? "<PASTE_YOUR_COMPUTER_VISION_ENDPOINT_HERE>";

        // Public method to run handwriting conversion from image
        public static async Task RunHandwritingConversionAsync()
        {
            Console.WriteLine("===== Handwriting Conversion From Image =====");
            Console.WriteLine("🖋️  Using Azure AI Vision API for Handwriting Recognition");
            Console.WriteLine();
            try
            {
                // Create a client
                ImageAnalysisClient client = AuthenticateClient(endpoint, subscriptionKey);
                // Analyze an image to extract handwritten text
                await ConvertHandwritingFromImageAsync(client);
                Console.WriteLine("----------------------------------------------------------");
                Console.WriteLine("✅ Handwriting conversion complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in handwriting conversion: {ex.Message}");
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
            Console.WriteLine("✅ Modern Azure AI Vision client authenticated successfully");
            Console.WriteLine($"   Endpoint: {endpoint}");
            Console.WriteLine();
            return client;
        }

        /// <summary>
        /// Extract handwritten text from image using Azure AI Vision
        /// </summary>
        private static async Task ConvertHandwritingFromImageAsync(ImageAnalysisClient client)
        {
            Console.WriteLine("🔍 Analyzing image for handwritten text...");

            // You can use either a local image file or a URL
            // For demonstration, I'll show both options

            // Option 1: Using a URL (replace with your image URL)
            string imageUrl = "https://learn.microsoft.com/azure/ai-services/computer-vision/media/handwriting-sample.jpg";

            // Option 2: Using a local file (uncomment and modify path as needed)
            // string localImagePath = @"path\to\your\handwriting-sample.jpg";

            try
            {
                // Analyze image from URL
                ImageAnalysisResult result = await client.AnalyzeAsync(
                    BinaryData.FromObjectAsJson(new { url = imageUrl }),
                    VisualFeatures.Read);

                // Alternative: Analyze local image file
                // byte[] imageData = await File.ReadAllBytesAsync(localImagePath);
                // ImageAnalysisResult result = await client.AnalyzeAsync(
                //     BinaryData.FromBytes(imageData),
                //     VisualFeatures.Read);

                Console.WriteLine($"📊 Image analysis completed. Model version: {result.ModelVersion}");
                Console.WriteLine();

                // Extract and display the handwritten text
                if (result.Read?.Blocks != null && result.Read.Blocks.Any())
                {
                    Console.WriteLine("📝 Extracted Handwritten Text:");
                    Console.WriteLine("=====================================");

                    foreach (var block in result.Read.Blocks)
                    {
                        foreach (var line in block.Lines)
                        {
                            Console.WriteLine($"📄 Line: {line.Text}");

                            // Display individual words with confidence scores
                            foreach (var word in line.Words)
                            {
                                Console.WriteLine($"   💭 Word: '{word.Text}' (Confidence: {word.Confidence:F2})");
                            }
                            Console.WriteLine();
                        }
                    }

                    // Extract all text as a single string
                    var allText = string.Join(" ", result.Read.Blocks
                        .SelectMany(block => block.Lines)
                        .Select(line => line.Text));

                    Console.WriteLine("📋 Complete Extracted Text:");
                    Console.WriteLine("============================");
                    Console.WriteLine(allText);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("❌ No handwritten text detected in the image.");
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"❌ Azure AI Vision API error: {ex.Message}");
                if (ex.Status == 401)
                {
                    Console.WriteLine("💡 Please check your subscription key and endpoint configuration.");
                }
                else if (ex.Status == 400)
                {
                    Console.WriteLine("💡 Please check if the image URL is valid and accessible.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Alternative method to analyze handwriting from a local file
        /// </summary>
        public static async Task ConvertHandwritingFromLocalFileAsync(string imagePath)
        {
            Console.WriteLine("===== Handwriting Conversion From Local File =====");
            Console.WriteLine($"🖋️  Analyzing file: {imagePath}");
            Console.WriteLine();

            try
            {
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine($"❌ File not found: {imagePath}");
                    return;
                }

                ImageAnalysisClient client = AuthenticateClient(endpoint, subscriptionKey);

                byte[] imageData = await File.ReadAllBytesAsync(imagePath);

                ImageAnalysisResult result = await client.AnalyzeAsync(
                    BinaryData.FromBytes(imageData),
                    VisualFeatures.Read);

                Console.WriteLine($"📊 Image analysis completed for: {Path.GetFileName(imagePath)}");
                Console.WriteLine($"📊 Model version: {result.ModelVersion}");
                Console.WriteLine();

                if (result.Read?.Blocks != null && result.Read.Blocks.Any())
                {
                    Console.WriteLine("📝 Extracted Handwritten Text:");
                    Console.WriteLine("=====================================");

                    foreach (var block in result.Read.Blocks)
                    {
                        foreach (var line in block.Lines)
                        {
                            Console.WriteLine($"📄 {line.Text}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("❌ No handwritten text detected in the image.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error processing local file: {ex.Message}");
            }
        }
    }
}
