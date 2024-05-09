// DAVE NOCEDA

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RRPolicy : Policy
{
    private List<Process> processQueue = new List<Process>();
    //  private HashSet<Process> queuedProcesses = new HashSet<Process>();
    public override Process GetRunningProcess(List<Process> processList)
    {
        if (processList.Count == 0)
            return null; // No process to run
        
        foreach (Process process in processList)
        {
            if (process.status == Status.Ready && !processQueue.Contains(process)) 
            // && !queuedProcesses.Contains(process))
            {
                processQueue.Add(process);
                // queuedProcesses.Add(process);
            }
        }

        if (processQueue.Count == 0)
            return null;
        
        //Gets the next process
        Process currentProcess = processQueue.First();
        currentProcess.DecQuantumTime();
        Debug.Log("quantume time: " + currentProcess.quantumTime);

        //Check if the quantum time is 0
        if (currentProcess.quantumTime <= 0 || currentProcess.burstTime <= 0){
            processQueue.RemoveAt(0);
            processQueue.Add(currentProcess);
        }
        
        return currentProcess;
    }
}