# Celebrity Detection Sample

This C# application demonstrates how to use Azure Computer Vision API to detect celebrities in images, both from remote URLs and local files.

## Features Converted from Python

This code is a C# translation of the original Python functionality:

### Remote Image Analysis
- Analyzes images from URLs to detect celebrities
- Uses the sample image from Azure Cognitive Services repository
- Displays celebrity names and confidence scores

### Local Image Analysis  
- Analyzes local image files for celebrity detection
- Expects images to be placed in the `Images/` folder
- Handles file not found scenarios gracefully

# How to Run Celebrity Detection

## Setup Instructions

### 1. Set Azure Credentials
You have two options for providing your Azure Computer Vision credentials:

#### Option A: Environment Variables (Recommended)
```bash
# In PowerShell or Command Prompt
set VISION_KEY=your_computer_vision_key_here
set VISION_ENDPOINT=your_computer_vision_endpoint_here
```

#### Option B: Direct Code Update
Edit the `Identify Celebrities.cs` file and replace:
- `<enter your key here>` with your actual API key
- `<enter your endpoint URL here>` with your actual endpoint URL

### 2. Add Sample Images (Optional)
Create an `Images` folder and add a `Faces.jpg` file for local testing:
```
Image Analysis/
??? Images/
?   ??? Faces.jpg  ? Add celebrity images here
??? Identify Celebrities.cs
??? Program.cs
```

### 3. Running the Application

#### Method 1: Using the Menu System (New - Recommended)
1. **Run the main project** - This will show you a menu with options
2. **Choose Option 1** for Celebrity Detection
3. The app will run both remote and local celebrity detection

#### Method 2: Run Celebrity Detection Directly
You can still run the celebrity detection directly by:
1. **Set Startup Project** to run `Identify_Celebrities.Main()` specifically
2. Or call `await Identify_Celebrities.RunCelebrityDetectionAsync()` from any other method

#### Method 3: From Visual Studio
1. **Set as Startup Project**: Right-click on the project ? "Set as Startup Project"
2. **Press F5 or Ctrl+F5** to run
3. The menu system will automatically start

## What You'll See

```
=== Azure AI-102 Image Analysis Samples ===

Choose an option:
1. ?? Celebrity Detection (Legacy API)
2. ?? Modern Image Analysis  
3. ?? Exit

Enter your choice (1-3): 1
```

When you choose Celebrity Detection, you'll see:
```
===== Celebrity Detection Sample =====
??  Note: Celebrity detection uses legacy Azure Computer Vision API

? Computer Vision client authenticated successfully
   Endpoint: https://your-endpoint.cognitiveservices.azure.com/

===== Detect Domain-specific Content - Remote =====
?? Analyzing remote image: https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg

?? Celebrities detected in remote image:
   ? Celebrity Name (Confidence: 0.9876 - ?? High)

===== Detect Domain-specific Content - Local =====
?? Analyzing local image: Images/Faces.jpg

?? Celebrities detected in local image:
   ? Another Celebrity (Confidence: 0.8234 - ?? High)
```

## Troubleshooting

### "No celebrities detected" 
- This is normal if the image doesn't contain recognizable celebrities
- Try the remote sample first (it uses a known celebrity image)

### "Local image file not found"
- Create an `Images` folder in your project directory
- Add a `Faces.jpg` file with celebrity faces
- Or skip local testing and just use the remote example

### "Please set your Azure Computer Vision credentials"
- Make sure you've set the environment variables or updated the code
- Verify your Computer Vision resource is active in Azure Portal
- Check that your endpoint URL is correct (should end with `.cognitiveservices.azure.com/`)

## Dependencies Added

- `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` - For celebrity detection API
- `Newtonsoft.Json` - For JSON parsing of API responses

## Running Options Summary

| Method | How to Run | Best For |
|--------|------------|----------|
| **Menu System** | Press F5 ? Choose option 1 | **Recommended** - Easy to use |
| **Direct Call** | Call `RunCelebrityDetectionAsync()` | Integration with other code |
| **Standalone** | Run `Identify_Celebrities.Main()` | Testing celebrity detection only |

## Sample Output

```
===== Celebrity Detection Sample =====

===== Detect Domain-specific Content - remote =====
Celebrities in the remote image:
- [Celebrity Name] (Confidence: 0.9876)

===== Detect Domain-specific Content - local =====
Celebrities in the local image:
- [Celebrity Name] (Confidence: 0.9234)
```

## Notes

- The celebrity detection feature uses domain-specific analysis
- Confidence scores range from 0.0 to 1.0 (higher is more confident)
- The application handles various error scenarios including missing files and API errors
- Make sure your Azure Computer Vision resource has the necessary permissions for domain-specific analysis