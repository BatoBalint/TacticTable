using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineUI : MonoBehaviour
{
    private Timeline _timeline;
    [SerializeField]
    private GameObject _timelineItem;
    [SerializeField]
    private GameObject _timelineItemHolder;

    public TimelineItemScript SelectedTimelineItem;

    private void Awake()
    {
        _timeline = GetComponent<Timeline>();
    }

    public void ReDrawUI()
    {
        foreach (Transform t in _timelineItemHolder.transform) 
        {
            Destroy(t.gameObject);
        }
        int index = 0;
        foreach (Movement move in _timeline.Moves)
        {
            GameObject newTimelineItem = Instantiate(_timelineItem, _timelineItemHolder.transform);
            TimelineItemScript timelineItemScript = newTimelineItem.GetComponent<TimelineItemScript>();

            timelineItemScript.ParentTimelineUI = this;
            timelineItemScript.Index = index;
            newTimelineItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = move.movementName;
            
            if (index == _timeline.Index && _timeline.TimelinePlays)
                timelineItemScript.Highlight();
            ++index;
        }
    }

    public void SelectTimelineItem(TimelineItemScript item)
    {
        // Makes it null proof
        if (SelectedTimelineItem == null)
        {
            SelectedTimelineItem = item;
        }
        // Toggles selection if same item clicked
        else if (SelectedTimelineItem == item)
        {
            SelectedTimelineItem.Unselect();
            SelectedTimelineItem = null;
        }
        else 
        { 
            SelectedTimelineItem.Unselect();
            SelectedTimelineItem = item;    
        }
        _timeline.SelectMovement(SelectedTimelineItem.Index);
    }
}
