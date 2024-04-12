using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Whisper.Samples
{
    /// <summary>
    /// Record audio clip from microphone and make a transcription.
    /// </summary>
    public class MicrophoneDemo : MonoBehaviour
    {
        public WhisperManager whisper;
        public MicrophoneRecord microphoneRecord;
        public bool streamSegments = true;
        private string _buffer;
        // public bool printLanguage = true;

        // Add Apps
        [Header("Apps")] 
        public DesktopManager desktopManager;

        [Header("UI")] 
        public Button button;
        // public Text buttonText;
        public TextMeshProUGUI outputText;
        public float outputTextDuration = 2.0f;
        
        // public Text timeText;
        // public Dropdown languageDropdown;
        // public Toggle translateToggle;
        // public Toggle vadToggle;
        // public ScrollRect scroll;

        [Header("Dictionary")] 
        private Dictionary<string, (Action function, string message)> commandDictionary;

        [Header("Voice Activity Detection (VAD)")]
        public Image vadIndicatorImage;
        public Color defaultIndicatorColor;
        public Color voiceDetectedColor;
        public Color voiceUndetectedColor;

        private void Awake()
        {
            whisper.OnNewSegment += OnNewSegment;
            // whisper.OnProgress += OnProgressHandler;
            
            microphoneRecord.OnRecordStop += OnRecordStop;
            
            button.onClick.AddListener(OnButtonPressed);

            InitCommandDictionary();
            // languageDropdown.value = languageDropdown.options
            //     .FindIndex(op => op.text == whisper.language);
            // languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            // translateToggle.isOn = whisper.translateToEnglish;
            // translateToggle.onValueChanged.AddListener(OnTranslateChanged);

            // vadToggle.isOn = microphoneRecord.vadStop;
            // vadToggle.onValueChanged.AddListener(OnVadChanged);
        }

        // Function to initialize the command dictionary
        private void InitCommandDictionary()
        {
            commandDictionary = new Dictionary<string, (Action function, string message)>
            {
                { "open text editor", ( () => desktopManager.OpenApp(0), "Opening Text Editor" ) },
                { "open file manager", ( () => desktopManager.OpenApp(1), "Opening File Manager" ) },
                { "open help", ( () => desktopManager.OpenApp(2), "Opening Help App" ) },
                { "close app", ( () => desktopManager.CloseCurrentApp(), "Closing app" ) },
                // Add more commands and their corresponding functions and messages as needed
            };
        }

        private void Update() {
            
            Color color; 
            
            if (microphoneRecord.IsRecording) 
                color = microphoneRecord.IsVoiceDetected ? voiceDetectedColor : voiceUndetectedColor;
            
            else
                color = defaultIndicatorColor;

            vadIndicatorImage.color = color;

        }

        private void OnVadChanged(bool vadStop)
        {
            microphoneRecord.vadStop = vadStop;
        }

        private void OnButtonPressed()
        {
            if (!microphoneRecord.IsRecording)
            {
                microphoneRecord.StartRecord();
                // buttonText.text = "Stop";
                UnityEngine.Debug.Log("Recording...");
            }
            else
            {
                microphoneRecord.StopRecord();
                // buttonText.text = "Record";
                UnityEngine.Debug.Log("Stop recording.");
            }
            
            string message = "Honey, what can I do for you?";
            DisplayOutputText(message);
            
        }
        
        private async void OnRecordStop(AudioChunk recordedAudio)
        {
            // buttonText.text = "Record";
            UnityEngine.Debug.Log("Stop recording.");
            _buffer = "";

            var sw = new Stopwatch();
            sw.Start();
            
            var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
            if (res == null || !outputText) 
                return;

            var time = sw.ElapsedMilliseconds;
            var rate = recordedAudio.Length / (time * 0.001f);
            // timeText.text = $"Time: {time} ms\nRate: {rate:F1}x";

            var text = res.Result;

            // if (printLanguage)
            //     text += $"\n\nLanguage: {res.Language}";
            
            RunCommand(text);
            // UiUtils.ScrollDown(scroll);
        }
        
        // private void OnLanguageChanged(int ind)
        // {
        //     var opt = languageDropdown.options[ind];
        //     whisper.language = opt.text;
        // }
        
        // private void OnTranslateChanged(bool translate)
        // {
        //     whisper.translateToEnglish = translate;
        // }

        // private void OnProgressHandler(int progress)
        // {
        //     if (!timeText)
        //         return;
        //     timeText.text = $"Progress: {progress}%";
        // }
        
        private void OnNewSegment(WhisperSegment segment)
        {
            if (!streamSegments || !outputText)
                return;

            _buffer += segment.Text;
            outputText.text = _buffer + "...";
            // UiUtils.ScrollDown(scroll);
        }

        private void DisplayOutputText(string message)
        {
            StartCoroutine(HandleDisplayOutputText(message));
        }

        // Define a coroutine to display output text for a specified duration
        private IEnumerator HandleDisplayOutputText(string message)
        {
            // Display the message
            outputText.text = message;
            
            // Wait for the specified duration
            yield return new WaitForSeconds(outputTextDuration);
            
            // Clear the output text
            outputText.text = "";
        }

        private void RunCommand(string command)
        {
            // Convert the command text to lower case for easier matching
            string normalizedCommand = command.Trim().ToLower();

            // Iterate over the dictionary to find a command that is contained in the input text
            foreach (var entry in commandDictionary)
            {
                // Check if a command contains a specific string rather than an exact match
                if (normalizedCommand.Contains(entry.Key))
                {
                    // Execute the corresponding function and display the output message
                    entry.Value.function();
                    DisplayOutputText(entry.Value.message); // Display message for 2 seconds
                    return; // Exit the loop after finding a match
                }
            }

            // Handle unknown commands (e.g., log a message or ignore)
            DisplayOutputText("Sorry, Sweetie. Command not found.");
            UnityEngine.Debug.Log($"Unknown command: {command}");
        }
    }
}