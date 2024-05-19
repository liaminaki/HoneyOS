using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemSegmentsManager : MonoBehaviour
{
    public GameObject memSegmentPrefab;
    public GameObject memoryFrame;
    private float totalMem = 1024;

    public void AddSegments(int memSize, int basePosition){
        RectTransform memoryFrameRectTransform = memoryFrame.GetComponent<RectTransform>();
        if (memoryFrameRectTransform != null){
            float positionPercentage = basePosition / totalMem;
            float parentHeight = memoryFrameRectTransform.rect.height;

            // if (basePosition == 0){
            //     float yPosition = (parentHeight/2) - 5f;
            // }

            float heightPercentage = memSize / totalMem;
            float newMemSegmentHeight = memoryFrameRectTransform.rect.height * heightPercentage;


            GameObject newMemSegment = Object.Instantiate(memSegmentPrefab, memoryFrame.transform);
            RectTransform newMemSegmentRectTransform = newMemSegment.GetComponent<RectTransform>();
            if (newMemSegmentRectTransform != null){
                newMemSegmentRectTransform.sizeDelta = new Vector2(newMemSegmentRectTransform.sizeDelta.x - 16f, newMemSegmentHeight);
                float yPosition = parentHeight * (1 - positionPercentage) - (newMemSegmentHeight / 2);
                newMemSegmentRectTransform.anchoredPosition = new Vector2(newMemSegmentRectTransform.anchoredPosition.x, yPosition);
            }

        }
    }   
}
