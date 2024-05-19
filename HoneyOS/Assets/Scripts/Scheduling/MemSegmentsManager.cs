using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemSegmentsManager : MonoBehaviour
{
    public GameObject memSegmentPrefab;
    public GameObject memoryFrame;
    private float totalMem = 1024;
    private float yPosition;
    private float yOffset = 0;

    public void SampleAdd(){
        AddSegments (100, 0);
        AddSegments (200, 100);
        AddSegments (250, 300);
    }

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
                newMemSegmentRectTransform.sizeDelta = new Vector2(newMemSegmentRectTransform.sizeDelta.x, newMemSegmentHeight);
                // Randomize color
                Image image = newMemSegment.GetComponent<Image>();
                if (image != null)
                {
                    Color randomColor = new Color(Random.value, Random.value, Random.value);
                    image.color = randomColor;
                }
                // if (basePosition == 0){
                //     yPosition = parentHeight / 2 - (newMemSegmentHeight / 2);
                // }
                // else{
                    // yPosition = (parentHeight - (parentHeight / 2 * positionPercentage) - newMemSegmentHeight) / 2;
                    yPosition = parentHeight - (parentHeight / 2 * positionPercentage) - newMemSegmentHeight;
                // }
                newMemSegmentRectTransform.anchoredPosition = new Vector2(newMemSegmentRectTransform.anchoredPosition.x, yPosition);
            }

        }
    }   
}
