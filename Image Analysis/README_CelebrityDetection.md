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

## Setup Instructions

1. **Configure Azure Credentials**
   - Replace `<enter your key here>` with your Computer Vision API key
   - Replace `<enter your endpoint URL here>` with your Computer Vision endpoint URL

2. **Add Sample Images**
   - Create an `Images` folder in the project directory
   - Add a file named `Faces.jpg` containing celebrity faces for local testing

3. **Run the Application**
   - The application will automatically analyze both remote and local images
   - Results will be displayed in the console

## Dependencies Added

- `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` - For celebrity detection API
- `Newtonsoft.Json` - For JSON parsing of API responses

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