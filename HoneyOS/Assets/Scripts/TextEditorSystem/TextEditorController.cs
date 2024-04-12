using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using SFB;
using UnityEngine.Networking;

// BUG ALERT!
// 1. On start of text editor, when clicking filename, text are not highlighted. This is because of placeholder in inputfield.
// 2. When opening file, since value (textfield and/or filename) has changed, SaveButton.interactable is set to true. Should be false.

// POTENTIAL ISSUES
// 1. Shortcut checking for unsaved changes by checking if SaveButton is interactable.

public class TextEditorController : MonoBehaviour
{

    [SerializeField] TMP_InputField FileName;
    [SerializeField] TMP_InputField TextField;
    private string currentFilePath;


    void Start()
    {
        // File name
        FileName.onEndEdit.AddListener(OnEndEditFileName);
        TextField.onValueChanged.AddListener(OnValueChangedTextField);

        NewFile();
    }

    public void Save()
    {
        if (!string.IsNullOrEmpty(currentFilePath))
        {
            File.WriteAllText(currentFilePath, TextField.text);
            ClosePopUp();
        }
        else
            SaveAs();
        
        ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
    }

    public void SaveAs()
    {
        // Open Save File Panel
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", FileName.text, "txt");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, TextField.text);
            currentFilePath = path;
            FileName.text = Path.GetFileName(path);
            OnEndEditFileName(FileName.text);
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
            ClosePopUp();
        }
    }

    public void OpenFile()
    {
        // Open Open File Panel
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri));
            currentFilePath = paths[0];
            FileName.text = Path.GetFileName(paths[0]);
            OnEndEditFileName(FileName.text);
            ClosePopUp();
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
        }
    }

    public void CheckOpenFile()
    {
        if (ButtonManager.Instance.SaveButton.interactable)
            PopUpManager.Instance.ShowPopUp("UnsavedChangesOpenFile");
        else
            OpenFile();
    }

    public void NewFile()
    {
        currentFilePath = null;
        FileName.text = "";
        TextField.text = "";
        ClosePopUp();
        // Set Save Button not interactable when first loading up text since no changes have been made
        ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
    }
    public void CheckNewFile()
    {
        if (ButtonManager.Instance.SaveButton.interactable)
            PopUpManager.Instance.ShowPopUp("UnsavedChangesNewFile");
        else 
            NewFile();  
    }

    private IEnumerator OutputRoutineOpen(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log("WWW ERROR: " + www.error);
        else
            TextField.text = www.downloadHandler.text;
    }

    void OnEndEditFileName(string newFileName)
    {
        FileName.text = newFileName;
        if (!FileName.text.EndsWith(".bby"))
            FileName.text += ".bby";
    }

    void OnValueChangedTextField(string newContent)
    {
        ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(true);
        ButtonManager.Instance.SaveAsButton.GetComponent<ButtonController>().SetInteractable(true);
    }

    void ClosePopUp()
    {
        if (PopUpManager.Instance.UnsavedChangesOpenFile.activeInHierarchy)
            PopUpManager.Instance.UnsavedChangesOpenFile.GetComponent<PopupController>().Hide();
        if (PopUpManager.Instance.UnsavedChangesNewFile.activeInHierarchy)
            PopUpManager.Instance.UnsavedChangesNewFile.GetComponent<PopupController>().Hide();
    }    
}
