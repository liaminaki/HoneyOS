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
        textMeshPro.text = "I AM IN TEXT EDITOR.";
        // Implement FileManager-specific reset behavior here
    }
   
    public void ChangeText()
    {
        // Assign the new text to the TextMeshPro component
        textMeshPro.text = "I AM IN TEXT EDITOR. I MODIFIED SHITZ.";
    }
    
    // Add TextEditor-specific functionality here if needed
}