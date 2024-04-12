using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Define the TextEditor class that extends the App class
public class TextEditor : App
{
    public TextMeshProUGUI textMeshPro;
    public TMP_InputField inputField;

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
    
    // Called when the script instance is being loaded
    // private void Start()
    // {
    //     // Add a listener to the OnValueChanged event of the Input Field
    //     inputField.onValueChanged.AddListener(OnTextInput);
    // }

    // Function to handle text input
    public void OnTextInput(string text)
    {
        // Handle text input here
        Debug.Log("Text Input: " + text);
    }
}