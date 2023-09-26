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
        if (_timeline == null)
        {
            Debug.Log("Couldnt find component");
        }
    }

    public void UpdateTimeline()
    {
        foreach (Transform t in _timelineItemHolder.transform) 
        {
            Destroy(t.gameObject);
        }
        foreach (Movement move in _timeline.moves)
        {
            if (move.GetType() != typeof(DisksState))
            { 
                GameObject newTimelineItem = Instantiate(_timelineItem, _timelineItemHolder.transform);

                string animationName = move.movementName;

                newTimelineItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = animationName;
            }
        }
    }
}
