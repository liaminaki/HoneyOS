// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    private int processCount;
    private List<Process> processes; 
    private Process runningProcess;
    private Process prevRunningProcess;
    private bool isPlaying;
    // private Stack stack;
    private Policy schedulingPolicy;
    private int time;

    private GameObject processHolder;
    public GameObject processPrefab;
  
    void Awake() {
        processCount = 0;
        processes = new List<Process>();
        isPlaying = false;
        // stack = new Stack();
        time = 0;
        prevRunningProcess = null;

        // Set this to GameObject that holds the processes or just grab reference
        processHolder = new GameObject("processHolder");  // Test holder / container for processes
    }
  
    public void SetSchedulingPolicy(string policy)
    {
        switch (policy)
        {
            case "FCFS":
                schedulingPolicy = new FCFSPolicy();
                break;
            case "SJF":
                schedulingPolicy = new SJFPolicy();
                break;
            case "Priority":
                schedulingPolicy = new PrioPolicy();
                break;
            case "RR":
                schedulingPolicy = new RRPolicy();
                break;
            default:
                // Handle unknown scheduling policies
                break;
        }
    }

	
    public void Play() 
    {
        isPlaying = true;
        while(isPlaying)
        {
            StartCoroutine(CountDown());
        
        }
    }
  
    private IEnumerator CountDown(){
        yield return new WaitForSeconds(1f);
        Next();
    }
        
    public void Next()
    {
        // Get running process from chosen scheduling policy
        runningProcess = schedulingPolicy.GetRunningProcess(processes);

        if (runningProcess != prevRunningProcess) 
        {   
            if (prevRunningProcess != null)
                prevRunningProcess.SetStatus(Status.Ready);
            
            runningProcess.SetStatus(Status.Running);
            prevRunningProcess = runningProcess;
        }            
        
        ++processCount;
        ++time;
        
        runningProcess.DecBurstTime();
        
        foreach (Process process in processes) 
        {
            process.DecWaitTime();  // Decrement wait time for all processes
            process.UpdateStatus(); // Update status of all process
            
            if (process.status == Status.Terminated)
                processes.Remove(process);
            
        }
        
        // Decide whether to add new process 
        if (UnityEngine.Random.Range(1, 9) == 1) 
            AddProcess();
        
    }
    // OPTIONAL
    // public void Previous()
    // {
    //     --processCount;
    //     --time;
        
    //     runningProcess.IncBurstTime();
        
    //     // Decrement wait time for all processes
    //     foreach (Process process in processes) 
    //     {
    //         process.IncWaitTime();
    //     }
    // }
        
    public void Pause()
    {
        isPlaying = false;
    }
        
    public void Stop()
    {
        // Decrement wait time for all processes
        foreach (Process process in processes) 
        {
            process.SetStatus(Status.Terminated);
        }
    }
        
    public void AddProcess()
    {
        // Instantiate a new process with processCount as its ID and a default time
        // Process newProcess = new Process(processCount, defaultProcessTime);
        
        // Add the new process to the processes list
        // processes.Add(newProcess);

        GameObject process = Object.Instantiate(processPrefab, processHolder.transform);
        // Sets "processHolder" as the new parent of the process GameObject.
        process.transform.SetParent(processHolder.transform); // Set position relative to parent
        // process.transform.SetParent(processHolder.transform, false); // Set position in global orientation

    
 
    }
    
    // private void Randomize(int min, int max) {
    //     return Randomizer.i.Randomize(min, max + 1); // Include min and max
    // }
    
    // private Inc
}