using System;
using System.Collections;
using System.Collections.Generic;

public class ProcessManager : MonoBehaviour
{
    private int ProcessCount;
    private List<Process> Processes; 
    private Process RunningProcess;
    private Process PrevRunningProcess;
    private bool isPlaying;
    // private Stack stack;
    private Policy SchedulingPolicy;
    private int time;

    private GameObject ProcessHolder;
    [SerializeField] ProcessPrefab;
  
    Awake() {
        ProcessCount = 0;
        Processes = new List<Process>();
        isPlaying = false;
        // stack = new Stack();
        time = 0;
        PrevRunningProcess = null;

        // Set this to GameObject that holds the processes or just grab reference
        ProcessHolder = new GameObject("ProcessHolder");  // Test holder / container for processes
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
                schedulingPolicy = new PriorityPolicy();
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
        RunningProcess = schedulingPolicy.GetRunningProcess(ProcessesList);

        if (RunningProcess != PrevRunningProcess) 
        {   
            if (PrevRunningProcess != null)
                PrevRunningProcess.SetStatus(Status.Ready);
            
            RunningProcess.SetStatus(Status.Running);
            PrevRunningProcess = RunningProcess;
        }            
        
        ++ProcessCount;
        ++time;
        
        RunningProcess.DecBurstTime();
        
        foreach (Process process in Processes) 
        {
            process.DecWaitTime();  // Decrement wait time for all processes
            process.UpdateStatus(); // Update status of all process
            
            if (process.GetStatus() == Status.Terminated)
                Processes.remove(process);
            
        }
        
        // Decide whether to add new process 
        if (Random.Range(1, 9) == 1) 
            AddProcess();
        
    }
    // OPTIONAL
    // public void Previous()
    // {
    //     --ProcessCount;
    //     --time;
        
    //     RunningProcess.IncBurstTime();
        
    //     // Decrement wait time for all processes
    //     foreach (Process process in Processes) 
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
        foreach (Process process in Processes) 
        {
            process.SetStatus(Status.Terminated)
        }
    }
        
    public void AddProcess()
    {
        // Instantiate a new process with ProcessCount as its ID and a default time
        Process newProcess = new Process(ProcessCount, defaultProcessTime);
        
        // Add the new process to the Processes list
        Processes.Add(newProcess);

        GameObject process = Object.Instantiate(ProcessPrefabs);
        // Sets "ProcessHolder" as the new parent of the process GameObject.
        process.transform.SetParent(ProcessHolder); // Set position relative to parent
        // process.transform.SetParent(ProcessHolder, false); // Set position in global orientation

        // If components not set in prefab
        process.AddComponent<Process>();
        newProcess.idText = 
    }
    
    // private void Randomize(int min, int max) {
    //     return Randomizer.i.Randomize(min, max + 1); // Include min and max
    // }
    
    private Inc
}