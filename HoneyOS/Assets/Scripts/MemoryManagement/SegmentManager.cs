using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SegmentManager : MonoBehaviour
{
    public GameObject memSegmentPrefab;
    public GameObject memoryFrame;

    private const float MAX= 1024;
    public void AddSegment(Segment segment){
        
        int baseAdr = segment.baseAdr;
        int memSize = segment.process.memorySize;

        Transform memoryFrameTransform = memoryFrame.GetComponent<Transform>();
        
        if (memoryFrameTransform != null){

            GameObject newSegment = Object.Instantiate(memSegmentPrefab, memoryFrame.transform);

            segment.SetObjectRef(newSegment);

            Transform tmProGameObject = newSegment.transform.Find ("SegmentID");

            if (tmProGameObject != null){
                TextMeshPro segmentIDComponent = tmProGameObject.GetComponent<TextMeshPro>();
                if (segmentIDComponent != null){
                    string id = segment.process.id.ToString();
                    segmentIDComponent.text = id;
                }
            }
            
            RectTransform newSegmentTransform = newSegment.GetComponent<RectTransform>();
            
            if (newSegmentTransform != null){

                // Set base address using Position Y
                // 0 to -1 (MAX)
                float newBaseAdr = (baseAdr / MAX) * -1;

                // Set size using scale Y
                // 0 to 1 (MAX)
                float newSize = (memSize / MAX);
                
                newSegmentTransform.localScale = new Vector2(newSegmentTransform.sizeDelta.x, newSize);
                // Randomize color
                SpriteRenderer segmentSprite = newSegment.GetComponent<SpriteRenderer>();
                if (segmentSprite != null)
                {
                    Color randomColor = new Color(Random.value, Random.value, Random.value);
                    segmentSprite.color = randomColor;
                }
                newSegmentTransform.anchoredPosition = new Vector2(newSegmentTransform.anchoredPosition.x, newBaseAdr);
            }
        }
    }

    public void DeleteSegment (Segment segment)
    {
        Destroy(segment.segmentObjRef);
    }
}