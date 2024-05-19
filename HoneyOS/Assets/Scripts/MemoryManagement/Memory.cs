using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    private const int MIN = 0;
    private const int MAX = 1024;
    private List<Segment> SegmentTable;

    int minSpaceBaseAdr;

    public static Memory Instance { get; private set; }
    public SegmentManager segmentManager;

    void Awake() 
    {
        SegmentTable = new List<Segment>();
        
        // Ensure only one instance of Memory exists
        if (Instance == null)
        {
            Instance = this;
            

            // Optionally, make this object persistent across scenes
            // DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Destroy the duplicate instance
            Destroy(gameObject);
        }
    }

    // Optionally, handle cleanup on destroy
    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
    
    // Check if there is enough memory for the process and allocate
    public bool HasMemory(Process process) 
    {   
        // Assuming Segment Table is sorted based on ascending value of baseAdr

        minSpaceBaseAdr = MIN;
    
        if (SegmentTable.Count > 0)
        {   
            int space;

            // Get space before first segment 
            int minSpace = SegmentTable[0].baseAdr - MIN;
               
            // Get smallest space
            for(int i = 1; i < SegmentTable.Count; i++) 
            {
                // Check for space between two segments
                space = SegmentTable[i].baseAdr - SegmentTable[i - 1].endAdr;

                if (space >= process.memorySize  && space < minSpace)
                {
                    minSpace = space;
                    minSpaceBaseAdr = SegmentTable[i - 1].endAdr;
                }
            }

            // Check space after the last segment
            space = MAX - (SegmentTable[SegmentTable.Count - 1].endAdr);
            
            if (space >= process.memorySize  && space < minSpace)
            {
                minSpace = space;
                minSpaceBaseAdr = SegmentTable[SegmentTable.Count - 1].endAdr;
            }

            if (process.memorySize <= minSpace) {
                
                AllocateMemory(process);

                // AddInSegmentTable(minSpaceBaseAdr, process.memorySize);
                return true;
            }

        }

        else
        {
            // If SegmentTable is empty, check if the process can fit in the whole memory
            if (process.memorySize <= (MAX - MIN)) {
                
                // Add segment table 
                AllocateMemory(process);
                
                // AddInSegmentTable(MIN, process.memorySize);
                return true;
            }
        }
        
        return false;

    }
    // Use HasMemory() before using AllocateMemory() to check for memory before allocating directly
    public void AllocateMemory(Process process)
    {   

        Segment newSegment = new Segment(minSpaceBaseAdr, process.memorySize, process);
        SegmentTable.Add(newSegment);
        
        // Add in visualization implementation
        segmentManager.AddSegment(newSegment); 

        // Sort the table in ascending order based on the Base attribute
        SegmentTable.Sort((x, y) => x.baseAdr.CompareTo(y.baseAdr));

        Debug.Log("In memory.AllocateMemory(Process process)");
        
    }

    public void DeallocateMemory(Process process)
    {
        Debug.Log("baka");
        Debug.Log("processID: " + process.id);
        foreach (Segment segment in SegmentTable)
        {
            if (segment.process == process)
            {
                SegmentTable.Remove(segment);

                // Remove in visualization implementation
                segmentManager.DeleteSegment(segment);
            }
                
        }

    }

}