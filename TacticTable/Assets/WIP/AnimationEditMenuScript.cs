using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEditMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttonGroups;
    [SerializeField]
    private Timeline _timeline;
    private TimelineUI _timelineUIref;

    private AnimationMode _animationMode = AnimationMode.None;
    private Func<int> _animationModeUpdate;

    private void Awake()
    {
        _timelineUIref = _timeline.GetComponent<TimelineUI>();
        AnimEditEventSystem.Instance.onAnimMenuButtonClick += SetMode;
    }

    private void OnDestroy()
    {
        AnimEditEventSystem.Instance.onAnimMenuButtonClick -= SetMode;
    }

    private void Update()
    {
        if (_animationModeUpdate != null)
        {
            _animationModeUpdate();
        }
    }

    public void SwitchToPanel(GameObject panel)
    { 
        DisableAllPanels();
        panel.SetActive(true);
        panel.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
    }

    private void DisableAllPanels()
    {
        for (int i = 0; i < _buttonGroups.transform.childCount; i++)
        {
            _buttonGroups.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetMode(AnimationMode mode)
    {
        if (_animationMode == mode) mode = AnimationMode.None;
        _animationMode = mode;
        switch (mode)
        {
            case AnimationMode.Move:
                _animationModeUpdate = MoveUpdate;
                break;
            default:
                _animationModeUpdate = null;
                break;
        }
    }

    private int MoveUpdate()
    {
        if (DiskScript.selectedDisks.Count > 1)
        {
            DiskScript.selectedDisks[0].UnselectDisk();
        }
        return 0;
    }

    public void DoneButtonClick()
    {
        if (DiskScript.selectedDisks.Count > 0)
        {
            GameObject disk = DiskScript.selectedDisks[0].gameObject;
            Timeline.Add(new LinearMovement(disk, disk.GetComponent<DiskScript>().positionAtSelection, disk.transform.position));
            _timelineUIref.UpdateTimeline();
            SetMode(AnimationMode.None);

            while (DiskScript.selectedDisks.Count > 0) DiskScript.selectedDisks[0].UnselectDisk();
        }
    }

    public void TestFunc()
    {
        _timeline.Play();
    }
}

public enum AnimationMode
{
    None,
    Move,
    MoveWithBall,
    Pass,
    Switch,
    FrontCross,
    FrontCrossWithBall,
    RearCross,
    RearCrossWithBall,
}
