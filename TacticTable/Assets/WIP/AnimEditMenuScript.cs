using System;
using UnityEngine;
using UnityEngine.UI;

public class AnimEditMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttonGroups;
    [SerializeField]
    private Transform _diskHolder;
    [SerializeField]
    private Timeline _timeline;
    private TimelineUI _timelineUIref;

    [SerializeField]
    private bool _autoMoveOnDiskMove = true;

    private delegate void NoParamVoidFunc();
    private NoParamVoidFunc _animationModeUpdate;
    private AnimationMode _animationMode = AnimationMode.None;

    private void Awake()
    {
        _timelineUIref = _timeline.GetComponent<TimelineUI>();
        if (AnimEditEventSystem.Instance != null)
        { 
            AnimEditEventSystem.Instance.onAnimMenuButtonClick += ToggleMode;
            AnimEditEventSystem.Instance.onDiskPositionChange += DiskPositionChange;
        }
    }

    private void Start()
    {
        _timeline.SaveDiskPositions(_diskHolder);
    }

    private void OnDestroy()
    {
        if (AnimEditEventSystem.Instance != null)
        { 
            AnimEditEventSystem.Instance.onAnimMenuButtonClick -= ToggleMode;
            AnimEditEventSystem.Instance.onDiskPositionChange -= DiskPositionChange;
        }
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

    private void ToggleMode(AnimationMode mode)
    {
        if (_animationMode == mode) mode = AnimationMode.None;
        SetMode(mode);
    }

    private void SetMode(AnimationMode mode)
    {
        _animationMode = mode;
        switch (mode)
        {
            case AnimationMode.Move:
                _animationModeUpdate = MoveUpdate;
                MovementButtonScript.SwitchToButton(mode);
                DiskScript.SelectOnMove = true;
                break;
            case AnimationMode.Switch:
                _animationModeUpdate = SwitchUpdate;
                MovementButtonScript.SwitchToButton(mode);
                break;
            default:
                _animationModeUpdate = null;
                DiskScript.SelectOnMove = false;
                MovementButtonScript.UnselectAllButton();
                break;
        }
    }

    private void MoveUpdate()
    {
        if (DiskScript.SelectedDisks.Count > 1)
        {
            DiskScript.SelectedDisks[0].UnselectDisk();
        }
    }

    private void SwitchUpdate()
    {
        if (DiskScript.SelectedDisks.Count > 2)
        {
            DiskScript.SelectedDisks[0].transform.position = DiskScript.SelectedDisks[0].PositionAtSelection;
            DiskScript.SelectedDisks[0].UnselectDisk();
        }
    }

    // Called every time a disk is moved by the user
    private void DiskPositionChange(DiskScript disk)
    {
        if (_timeline.disksStates.Count == 1 && _animationMode == AnimationMode.None)
        {
            _timeline.disksStates[0].SetDiskPosition(disk, disk.transform.position);
        }
        else if (_autoMoveOnDiskMove && _timeline.moves.Count > 0 && _animationMode == AnimationMode.None)
        {
            SetMode(AnimationMode.Move);
        }
    }

    public void AddButtonClick()
    {
        if (_animationMode == AnimationMode.Move)
        {
            // Linear Movement
            if (DiskScript.SelectedDisks.Count > 0)
            {
                GameObject disk = DiskScript.SelectedDisks[0].gameObject;
                _timeline.Add(new LinearMovement(disk, disk.GetComponent<DiskScript>().PositionAtSelection, disk.transform.position), _diskHolder);
                _timelineUIref.UpdateTimeline();

                ResetAfterAddingMovement();
            }
        }
        else if (_animationMode == AnimationMode.Switch)
        {
            // Switch Movement
            if (DiskScript.SelectedDisks.Count > 1)
            { 
                GameObject disk1 = DiskScript.SelectedDisks[0].gameObject;
                GameObject disk2 = DiskScript.SelectedDisks[1].gameObject;

                DiskScript disk1Script = disk1.GetComponent<DiskScript>();
                DiskScript disk2Script = disk2.GetComponent<DiskScript>();


                SwitchMovement swMovement = new SwitchMovement(disk1, disk2, disk1Script.PositionAtSelection, disk2Script.PositionAtSelection);

               


                _timeline.Add(swMovement, _diskHolder);
                _timelineUIref.UpdateTimeline();

                _timeline.SetTimeSpeed(4);
                _timeline.AnimateAtIndex(_timeline.moves.Count - 1);

                ResetAfterAddingMovement();
            }
        }
    }

    private void ResetAfterAddingMovement()
    {
        while (DiskScript.SelectedDisks.Count > 0) DiskScript.SelectedDisks[0].UnselectDisk();
        SetMode(AnimationMode.None);
    }

    public void PlayButtonClick()
    {
        _timeline.Play();
    }

    public void DeleteButtonClick()
    {
        _timeline.RemoveAt(_timeline.moves.Count - 1);
        _timelineUIref.UpdateTimeline();
    }

    public void SaveButtonClick()
    { 
        
    }

    public void TestFunc()
    {
        _timeline.Play();
    }
}

// "None" have to be the first in the enum
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
