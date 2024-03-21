using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Define the TextEditor class that extends the App class
public class TextEditor : App
{
    public TextMeshProUGUI textMeshPro;

    // Override the ResetApp method to reset FileManager to default state
    protected override void Reset()
    {   
        textMeshPro.text = "This is a square.";
        // Implement FileManager-specific reset behavior here
    }
   
    public void ChangeText()
    {
        // Assign the new text to the TextMeshPro component
        textMeshPro.text = "This is a modified square.";
    }
    
    // Add TextEditor-specific functionality here if needed
}