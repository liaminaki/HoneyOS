// DAVE NOCEDA

using System;
using System.Collections;
using System.Collections.Generic;

public class RRPolicy : Policy
{
    public override Process GetRunningProcess(List<Process> processList)
    {
        if (processList.Count == 0)
            return null; // No process to run
        return null;
    }
}