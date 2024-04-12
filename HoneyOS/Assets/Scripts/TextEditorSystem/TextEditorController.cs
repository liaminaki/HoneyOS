using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using SFB;
using UnityEngine.Networking;

// BUG ALERT!
// 1. On start of text editor, when clicking filename, text are not highlighted. This is because of placeholder in inputfield.
// 2. When opening file, since value (textfield and/or filename) has changed, SaveButton.interactable is set to true. Should be false. [SOLVED]
// 3. On popup: When user clicks save (specific to SaveAs()) which opens Save Panel and exits panel w/o saving, Open File Panel opens. 
//    Open File Panel should not open when saving is cancelled. [SOLVED]
// 4. Continuation of #3. When cancelling Open File Panel and cancelling Unsaved Changes Pop Up, the next time user clicks Open File
//    Button, the Open File Panel opens without signifying unsaved changes. [SOLVED]

// POTENTIAL ISSUES
// 1. Shortcut checking for unsaved changes by checking if SaveButton is interactable.

public class TextEditorController : MonoBehaviour
{

    [SerializeField] TMP_InputField FileName;
    [SerializeField] TMP_InputField TextField;
    private string currentFilePath;
    private bool SaveCancelled = false;


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
            ClosePopUp(); // FAULTY. REASON FOR CLOSING WHEN EXITING OPENFILE PANEL AFTER SAVE() ON POPUP
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
        }
        else
            SaveAs();
    }

    public void SaveAs()
    {
        // Open Save File Panel
        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", FileName.text, "txt");
        Debug.Log("SaveAs() path after cancelling: " + path);
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, TextField.text);
            currentFilePath = path;
            FileName.text = Path.GetFileName(path);
            OnEndEditFileName(FileName.text);
            ClosePopUp();
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
            SaveCancelled = false;
        }
        else
        {
            SaveCancelled = true;
        }
    }

    public void OpenFile()
    {
        // Open Open File Panel
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "txt", false);
        if (paths.Length > 0)
        {
            Debug.Log("Passed even cancel");
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri));
            currentFilePath = paths[0];
            FileName.text = Path.GetFileName(paths[0]);
            OnEndEditFileName(FileName.text);
            ClosePopUp();
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
            SaveCancelled = false;
        }
    }

    public void CheckOpenFile()
    {
        if (ButtonManager.Instance.SaveButton.interactable)
            PopUpManager.Instance.ShowPopUp("UnsavedChangesOpenFile");
        else
            OpenFile();
    }

    public void SmartOpenFile()
    {
        if (!SaveCancelled)
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
        SaveCancelled = false;
    }
    public void CheckNewFile()
    {
        if (ButtonManager.Instance.SaveButton.interactable)
            PopUpManager.Instance.ShowPopUp("UnsavedChangesNewFile");
        else 
            NewFile();  
    }
    
    public void SmartNewFile()
    {
        if (!SaveCancelled)
            NewFile();
    }

    private IEnumerator OutputRoutineOpen(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log("WWW ERROR: " + www.error);
        else
        {
            TextField.text = www.downloadHandler.text;
            ButtonManager.Instance.SaveButton.GetComponent<ButtonController>().SetInteractable(false);
        }
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
    }

    void ClosePopUp()
    {
        if (PopUpManager.Instance.UnsavedChangesOpenFile.activeInHierarchy)
            PopUpManager.Instance.UnsavedChangesOpenFile.GetComponent<PopupController>().Hide();
        if (PopUpManager.Instance.UnsavedChangesNewFile.activeInHierarchy)
            PopUpManager.Instance.UnsavedChangesNewFile.GetComponent<PopupController>().Hide();
    }    
}
