using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define the base class for all apps
public class App : MonoBehaviour
{   
    public AppIcon appIcon; // Reference to the associated app icon

    public virtual void Open()
    {
        gameObject.SetActive(true);
        UpdateIcon(AppState.Opened);
    }

    public virtual void Close()
    {   
        gameObject.SetActive(false);
        UpdateIcon(AppState.Closed);
        Reset();
    }

    public virtual void Minimize()
    {
        gameObject.SetActive(false);
        UpdateIcon(AppState.Minimized);
    }
    
    // Method to reset the app to its default state
    protected virtual void Reset() {}

    // Method to update the app icon indicator
    private void UpdateIcon(AppState state)
    {
        if (appIcon != null)
        {
            appIcon.UpdateIndicator(state);
        }
    }

}
