using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineUI : MonoBehaviour
{
    private Timeline timeline;
    [SerializeField]
    private GameObject timelineItem;
    [SerializeField]
    private GameObject timelineItemHolder;

    private void Awake()
    {
        timeline = GetComponent<Timeline>();
        if (timeline == null)
        {
            Debug.Log("Couldnt find component");
        }
    }

    public void UpdateTimeline()
    {
        foreach (Transform t in timelineItemHolder.transform) 
        {
            Destroy(t.gameObject);
        }
        foreach (Movement move in Timeline.moves)
        {
            if (move.GetType() != typeof(DisksState))
            { 
                GameObject newTimelineItem = Instantiate(timelineItem, timelineItemHolder.transform);

                string animationName = move.movementName;

                newTimelineItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = animationName;
            }
        }
    }
}
