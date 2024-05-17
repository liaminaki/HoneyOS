using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int baseAdr { get; private set; }
    public int endAdr  { get; private set; }

    public Segment(int baseAdr, int limit) {
        this.baseAdr = baseAdr;
        this.endAdr = baseAdr + limit;
    }

}