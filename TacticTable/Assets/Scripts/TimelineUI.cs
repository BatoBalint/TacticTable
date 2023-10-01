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
            string animationName = move.movementName;
            newTimelineItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = animationName;
            
            if (index == _timeline.index && _timeline.timelinePlays)
                newTimelineItem.GetComponent<TimelineItemScript>().Highlight();
            ++index;
        }
    }
}
