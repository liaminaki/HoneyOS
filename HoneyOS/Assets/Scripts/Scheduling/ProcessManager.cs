// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public GameObject processesContainer;
    public GameObject processPrefab;

    public TMP_Text schedPolicyText;
  
    public 
    void Awake() {
        processCount = 0;
        processes = new List<Process>();
        isPlaying = false;
        time = 0;
        prevRunningProcess = null;

        SetSchedulingPolicy(SchedPolicy.SJF);

    }

    public enum SchedPolicy { FCFS, SJF, Prio, RR }
    public void SetSchedulingPolicy(SchedPolicy policy)
    {
        switch (policy)
        {
            case SchedPolicy.FCFS:
                schedulingPolicy = new FCFSPolicy();
                schedPolicyText.text = "First Come, First Serve";
                break;
            case SchedPolicy.SJF:
                schedulingPolicy = new SJFPolicy();
                schedPolicyText.text = "Shortest Job First";
                break;
            case SchedPolicy.Prio:
                schedulingPolicy = new PrioPolicy();
                schedPolicyText.text = "Priority";
                break;
            case SchedPolicy.RR:
                schedulingPolicy = new RRPolicy();
                schedPolicyText.text = "Round Robin";
                break;
            default:
                // Handle unknown scheduling policies
                break;
        }
    }

	
    public void Play() 
    {
        isPlaying = true;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while(isPlaying)
        {
            yield return new WaitForSeconds(1f);
            Next();
        }
    }
        
    public void Next()
    {
        // // Get running process from chosen scheduling policy
        // runningProcess = schedulingPolicy.GetRunningProcess(processes);

        // if (runningProcess != null)
        // {

        //     if (runningProcess != prevRunningProcess) 
        //     {   
        //         if (prevRunningProcess != null)
        //             prevRunningProcess.SetStatus(Status.Ready);
                
        //         runningProcess.SetStatus(Status.Running);
        //         prevRunningProcess = runningProcess;
        //     }            
            
        //     ++processCount;
        //     ++time;
            
        //     runningProcess.DecBurstTime();
            
        //     foreach (Process process in processes) 
        //     {
        //         process.DecWaitTime();  // Decrement wait time for all processes
        //         process.UpdateStatus(); // Update status of all process
        //         process.UpdateAttributes();
                
        //         if (process.status == Status.Terminated)
        //             processes.Remove(process);
                
        //     }
            
        // }

        ++time;

        UpdateProcesses();

        // Decide whether to add new process 
        if (UnityEngine.Random.Range(1, 5) == 1) 
            AddProcess(true); 
        
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

    public void UpdateProcesses()
    {
        // Get running process from chosen scheduling policy
        runningProcess = schedulingPolicy.GetRunningProcess(processes);

        if (runningProcess != null)
        {

            if (runningProcess != prevRunningProcess) 
            {   
                if (prevRunningProcess != null && prevRunningProcess.status != Status.Terminated) // Second part might not be necessary if delete lahos ang terminated
                    prevRunningProcess.SetStatus(Status.Ready);
                
                runningProcess.SetStatus(Status.Running);
                prevRunningProcess = runningProcess;
            }            
            
            runningProcess.DecBurstTime();
              
        }

        Process processToDestroy = null;

        foreach (Process process in processes) 
        {
            process.DecWaitTime();  // Decrement wait time for all processes
            process.UpdateStatus(); // Update status of all process
            process.UpdateAttributes();
            
            
            // Create new reference to terminated process since cant be done while iterating
            // Works since there is only one or no processes that will terminated at a time
            if (process.status == Status.Terminated) {
                processToDestroy = process;
            }    

        }

        // Remove terminated process if there is one
        if (processToDestroy != null)
        {
            processes.Remove(processToDestroy);
            Destroy(processToDestroy.objReference);
        }
            

    }
        
    public void Pause()
    {
        isPlaying = false;
    }
        
    public void Stop()
    {
        List<Process> processesToRemove = new List<Process>(processes); // Create a copy of the list

        foreach (Process process in processesToRemove)
        {
            processes.Remove(process); // Remove the process from the original list
            Destroy(process.objReference); // Destroy the associated GameObject
        }

        processes.Clear();

        isPlaying = false;
    }
        
    public void AddProcess(bool isFromNext)
    {
        // Instantiate a new process with processCount as its ID and a default time
        // Process newProcess = new Process(processCount, defaultProcessTime);
        
        // Add the new process to the processes list

        GameObject newProcess = Object.Instantiate(processPrefab, processesContainer.transform);
        
        // Sets "processHolder" as the new parent of the process GameObject.
        newProcess.transform.SetParent(processesContainer.transform); // Set position relative to parent
        // process.transform.SetParent(processHolder.transform, false); // Set position in global orientation

        Process process = newProcess.GetComponent<Process>();
        processes.Add(process);
        process.InitAttributes(newProcess, ++processCount, ++time);
        process.UpdateAttributes();

        if (!isFromNext)
            UpdateProcesses();
    
    }
    
    // private void Randomize(int min, int max) {
    //     return Randomizer.i.Randomize(min, max + 1); // Include min and max
    // }
    
    // private Inc
}