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
        private static readonly string subscriptionKey = Environment.GetEnvironmentVariable("AZURE_VISION_KEY") ?? "<Paste your key here>";
        private static readonly string endpoint = Environment.GetEnvironmentVariable("AZURE_VISION_ENDPOINT") ?? "< Paste your endpoint here>";

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

            // Get the current working directory for debugging
            string currentDir = Directory.GetCurrentDirectory();
            Console.WriteLine($"🔧 Current working directory: {currentDir}");

            // Try multiple possible paths for the image
            string[] possiblePaths = {
                Path.Combine("Image Analysis", "Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                Path.Combine("Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                Path.Combine("..", "Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                Path.Combine("..", "..", "Image Analysis", "Images", "Teach-Handwriting-Step-13-Version-2.jpg"),
                Path.Combine(currentDir, "Image Analysis", "Images", "Teach-Handwriting-Step-13-Version-2.jpg")
            };

            string localImagePath = null;

            // Find the first path that exists
            foreach (string path in possiblePaths)
            {
                Console.WriteLine($"🔍 Checking path: {path}");
                if (File.Exists(path))
                {
                    localImagePath = path;
                    Console.WriteLine($"✅ Found image at: {localImagePath}");
                    break;
                }
                else
                {
                    Console.WriteLine($"❌ Not found at: {path}");
                }
            }

            if (localImagePath != null && File.Exists(localImagePath))
            {
                Console.WriteLine($"📁 Using local image: {localImagePath}");

                try
                {
                    // Analyze local image file
                    byte[] imageData = await File.ReadAllBytesAsync(localImagePath);
                    Console.WriteLine($"📦 Image file size: {imageData.Length:N0} bytes");

                    ImageAnalysisResult result = await client.AnalyzeAsync(
               BinaryData.FromBytes(imageData),
               VisualFeatures.Read);

                    Console.WriteLine($"📊 Image analysis completed. Model version: {result.ModelVersion}");
                    Console.WriteLine($"📄 Analyzed file: {Path.GetFileName(localImagePath)}");
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
                        Console.WriteLine("💡 Authentication failed. Please check:");
                        Console.WriteLine("   - Your subscription key is valid and active");
                        Console.WriteLine("   - Your endpoint URL is correct");
                        Console.WriteLine("   - Your Azure subscription has sufficient credits");
                    }
                    else if (ex.Status == 400)
                    {
                        Console.WriteLine("💡 Bad request. Check if the image format is supported.");
                    }
                    else if (ex.Status == 403)
                    {
                        Console.WriteLine("💡 Access forbidden. Check your Azure resource permissions.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"❌ Could not find the image file in any of the expected locations!");
                Console.WriteLine("💡 Searched in the following locations:");
                foreach (string path in possiblePaths)
                {
                    Console.WriteLine($"   - {Path.GetFullPath(path)}");
                }

                Console.WriteLine($"💡 Current working directory: {currentDir}");

                // List available image files for debugging
                try
                {
                    var searchDirs = new[] {
   "Image Analysis\\Images",
       "Images",
     ".",
       Path.Combine(currentDir, "Image Analysis", "Images")
     };

                    foreach (var dir in searchDirs)
                    {
                        if (Directory.Exists(dir))
                        {
                            Console.WriteLine($"💡 Contents of {Path.GetFullPath(dir)}:");
                            var files = Directory.GetFiles(dir, "*.*")
                                .Where(file =>
                                    {
                                        var ext = Path.GetExtension(file).ToLowerInvariant();
                                        return ext == ".jpg" || ext == ".jpeg" || ext == ".png" ||
                                            ext == ".bmp" || ext == ".gif" || ext == ".tiff";
                                    });

                            foreach (var file in files)
                            {
                                Console.WriteLine($"   - {Path.GetFileName(file)}");
                            }
                            break; // Stop after finding the first valid directory
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"💡 Error listing directories: {ex.Message}");
                }
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
