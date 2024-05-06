// using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Process : MonoBehaviour
{   
    public TMP_Text idText;
    public TMP_Text priorityText;
    public TMP_Text arrivalTimeText;
    public TMP_Text burstTimeText;
    public TMP_Text memorySizeText;
    public TMP_Text statusText;

    private int id;
    public int priority { get; private set; }
    private int arrivalTime;
    public int burstTime { get; private set; }
    private int memorySize;
    public Status status { get; private set; }
    private float waitTime;
    public GameObject objReference { get; private set; }


    // Init attributes
    public void InitAttributes(GameObject objReference, int id, int arrivalTime)
    {   
        this.objReference = objReference;
        this.id = id;
        this.arrivalTime = arrivalTime;
        priority = Random.Range(1,101); // Generate a number from 1 to 100
        burstTime = Random.Range(1,10);
        memorySize = Random.Range(1,1000);
        SetStatus(Status.New);
        waitTime = Random.Range(0,10);
    }

    // void Awake()
    // {
        

    // }
  
    public void UpdateAttributes() 
    {
        idText.text = id.ToString();
        priorityText.text = priority.ToString();
        arrivalTimeText.text = arrivalTime.ToString();
        burstTimeText.text = burstTime.ToString();
        memorySizeText.text = memorySize.ToString();
        
        if (IsStatus(Status.Waiting))
            statusText.text = status.ToString() + "(" + waitTime + ")";
        else
            statusText.text = status.ToString();

    }

    public void UpdateStatus()
    {   
        if (status != Status.Terminated)
        {
            if (waitTime > 0) SetStatus(Status.Waiting);
            else if (waitTime <= 0 && !IsStatus(Status.Running)) SetStatus(Status.Ready);
            else if (burstTime <= 0) SetStatus(Status.Terminated);
        }

        // Change to Status.Running in ProcessManager
    }

    // public Status GetStatus()
    // {
    //     return status;
    // }

    public void SetStatus(Status newStatus) { status = newStatus; }

    private bool IsStatus(Status toCheckStatus) { return status == toCheckStatus; }

    public void DecBurstTime()	{ burstTime--; }
    public void IncBurstTime()	{ burstTime++; }
    public void DecWaitTime() 	{ waitTime--; }
    public void IncWaitTime()   { waitTime++; }

    // private int Randomize(int min, int max) 
    // {
    //     // Implementation of your Randomize method
    //     return new Random().Next(min, max);
    // }

}

public enum Status { New, Ready, Waiting, Running, Terminated}