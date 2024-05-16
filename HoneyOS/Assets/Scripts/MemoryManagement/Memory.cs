// using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


class Memory : MonoBehaviour {
    public int totalSize = 1024;
    
    [Header("Segment 1: Data Segment")]
    public int s1Base;
    public int s1Limit;

    [Header("Segment 2: Stack Segment")]
    public int s2Base;
    public int s2Limit;

    public void HasMemory(Process process) {
        if (process.DataSegment <= s1Limit)
            s1Limit -= process.DataSegment;
    }

    // public void 

}
