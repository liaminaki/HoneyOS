using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Define the FileManager class that extends the App class
public class FileManager : App
{   
    public TextMeshProUGUI textMeshPro;

    // Override the ResetApp method to reset FileManager to default state
    protected override void Reset()
    {   
        textMeshPro.text = "This is a triangle.";
        // Implement FileManager-specific reset behavior here
    }
   
    public void ChangeText()
    {
        // Assign the new text to the TextMeshPro component
        textMeshPro.text = "This is a modified triangle.";
    }

    // Add FileManager-specific functionality here if needed

    
}