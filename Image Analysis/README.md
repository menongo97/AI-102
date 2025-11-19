# Azure AI-102 Image Analysis Samples

This project demonstrates various Azure Computer Vision capabilities using both legacy and modern APIs.

## ?? **Quick Start**

### Option 1: Run with Menu (Recommended)
1. **Set up your Azure credentials** (see Configuration section below)
2. **Press F5** in Visual Studio OR run `dotnet run` in the "Image Analysis" directory
3. Choose from the menu options:
   - **Option 1**: Celebrity Detection (will show API restriction error)
   - **Option 2**: Modern Image Analysis (works with current API)
   - **Option 3**: Exit

### Option 2: Run Individual Components
Each class has its own `Main` method for standalone execution.

## ?? **Project Structure**

| File | Purpose | API Used | Status |
|------|---------|----------|--------|
| `Program.cs` | **Main menu system** | - | ? Working |
| `ModernImageAnalysis.cs` | **Image analysis with current API** | Azure.AI.Vision.ImageAnalysis | ? Working |
| `Identify_Celebrities.cs` | **Celebrity detection (restricted)** | Legacy Computer Vision | ?? Requires approval |
| `CognitiveServices.cs` | Original sample code | Azure.AI.Vision.ImageAnalysis | ? Working |

## ?? **Configuration**

### Setting Up Credentials
You have two options for providing your Azure Computer Vision credentials:

#### Option A: Environment Variables (Recommended for Security)
```bash
# Windows Command Prompt
set AZURE_VISION_KEY=your_computer_vision_key_here
set AZURE_VISION_ENDPOINT=https://your-endpoint.cognitiveservices.azure.com/

# Windows PowerShell
$env:AZURE_VISION_KEY="your_computer_vision_key_here"
$env:AZURE_VISION_ENDPOINT="https://your-endpoint.cognitiveservices.azure.com/"

# Linux/Mac
export AZURE_VISION_KEY=your_computer_vision_key_here
export AZURE_VISION_ENDPOINT=https://your-endpoint.cognitiveservices.azure.com/
```

#### Option B: Direct Code Update (Less Secure)
Edit the relevant `.cs` files and replace:
- `PASTE_YOUR_COMPUTER_VISION_SUBSCRIPTION_KEY_HERE` with your actual API key
- `https://YOUR_ENDPOINT.cognitiveservices.azure.com/` with your actual endpoint URL

**?? Important**: If you use Option B, **never commit your API keys to Git!**

### Image Sources
- **Remote Images**: URLs to sample images from Azure and Pexels
- **Local Images**: Place in `Images/` folder (optional)

## ?? **Features Overview**

### ? **Modern Image Analysis (Option 2)**
**What it does:**
- ?? **Image Captions**: Main description of the image
- ?? **Dense Captions**: Detailed descriptions of different regions
- ?? **People Detection**: Detects and locates people in images
- ?? **Object Detection**: Identifies and locates objects
- ??? **Smart Tags**: Categorized by confidence level (High/Medium/Low)

**Sample Output:**
```
===== Modern Image Analysis =====
?? Using Azure AI Vision API (Current/Supported)

? Modern Azure AI Vision client authenticated successfully
   Endpoint: https://your-endpoint.cognitiveservices.azure.com/

----------------------------------------------------------
ANALYZE IMAGE - URL

?? Analyzing the image: https://images.pexels.com/photos/4021773/pexels-photo-4021773.jpeg

?? Main Caption:
   a group of people sitting at a table (Confidence: 0.8234)

?? People Detected: 4
Person 1: ?? High confidence (0.9123)
     ?? Location: X=45, Y=67, Width=123, Height=234

??? Image Tags (25 total):
   ?? High Confidence:
      • person (0.9876)
      • people (0.9543)
      • table (0.8765)
```

### ?? **Celebrity Detection (Option 1)**
**Current Status**: **RESTRICTED** - Requires special Microsoft approval

**What happens when you run it:**
- Shows authentication success
- Attempts to analyze images for celebrities
- **Displays restriction error** (this is expected)
- Explains how to apply for access

**Error Message You'll See:**
```
? Computer Vision API error: {"error":{"code":"InvalidRequest","innererror":{"code":"UnsupportedFeature","message":"Feature is not supported. Please apply for access at https://aka.ms/celebrityrecognition"}}}
```

## ?? **What to Try**

### ? **Recommended: Modern Image Analysis**
1. Set up your Azure credentials using environment variables
2. Run the application
3. Choose **Option 2** (Modern Image Analysis)
4. See comprehensive analysis including:
   - People detection with confidence levels
   - Object detection with locations
   - Detailed captions for different image regions
   - Smart categorized tags

### ?? **Educational: Celebrity Detection**
1. Choose **Option 1** to see:
   - How the legacy API authentication works
   - The specific error message for restricted features
   - Code structure for domain-specific analysis
   - Information about applying for access

## ??? **Development Notes**

### API Differences
| Feature | Legacy API | Modern API |
|---------|------------|------------|
| **Authentication** | `ApiKeyServiceClientCredentials` | `AzureKeyCredential` |
| **Client** | `ComputerVisionClient` | `ImageAnalysisClient` |
| **Celebrity Detection** | ? Available (with approval) | ? Not supported |
| **People Detection** | ? Limited | ? Enhanced |
| **Object Detection** | ? Basic | ? Advanced |
| **Dense Captions** | ? Not available | ? Available |

### Code Quality Features
- **Modern C# patterns**: Using latest .NET 8 features
- **Async/await**: Proper async programming
- **Error handling**: Comprehensive try-catch with specific error types
- **User experience**: Emoji indicators and clear output formatting
- **Resource management**: Proper disposal of clients
- **Security**: Environment variable support to avoid hardcoded secrets

## ?? **Important Security Notes**

1. **Never commit API keys to Git**: Use environment variables or Azure Key Vault
2. **Celebrity Detection Restriction**: This feature now requires special approval from Microsoft
3. **Modern API Recommended**: For new projects, use the `ModernImageAnalysis.cs` approach with `Azure.AI.Vision.ImageAnalysis`

## ?? **Support**

- **Azure Computer Vision Documentation**: [Microsoft Docs](https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/)
- **Celebrity Recognition Access**: https://aka.ms/celebrityrecognition
- **Modern API Reference**: [Azure.AI.Vision.ImageAnalysis](https://docs.microsoft.com/en-us/dotnet/api/overview/azure/ai.vision.imageanalysis-readme)

## ?? **Ready to Run!**

1. Set up your Azure credentials using environment variables
2. Press **F5** in Visual Studio or run `dotnet run`
3. Choose Option 2 for the best working experience!

Your application is fully configured and ready to explore Azure Computer Vision capabilities!