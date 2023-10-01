using System;
using System.Collections;
using System.Net.Http.Headers;
using TMPro;
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
    private TimelineUI _timelineUI;

    // Save popup
    [SerializeField]
    private Transform _savePopUp;
    [SerializeField]
    private TextMeshProUGUI _saveNameInput;
    [SerializeField]
    private TextMeshProUGUI _feedbackText;

    [SerializeField]
    private bool _autoMoveOnDiskMove = true;
    private StorageManager _storageManager;

    // Animation management
    private delegate void NoParamVoidFunc();
    private NoParamVoidFunc _animationModeUpdate;
    private AnimationMode _animationMode = AnimationMode.None;

    private void Awake()
    {
        _timelineUI = _timeline.GetComponent<TimelineUI>();
        if (AnimEditEventSystem.Instance != null)
        { 
            AnimEditEventSystem.Instance.onAnimMenuButtonClick += ToggleMode;
            AnimEditEventSystem.Instance.onDiskPositionChange += DiskPositionChange;
        }
        _storageManager = new StorageManager();
    }

    private void Start()
    {
        _timeline.DiskStates.Add(new DisksState(DiskScript.DiskScripts));
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

    // Called every Time a disk is moved by the user
    private void DiskPositionChange(DiskScript disk)
    { 
        if (_timeline.DiskStates.Count == 1 && _animationMode == AnimationMode.None)
        {
            _timeline.DiskStates[0].SetDiskPosition(disk, disk.transform.position);
        }
        else if (_autoMoveOnDiskMove && _timeline.Moves.Count > 0 && _animationMode == AnimationMode.None)
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
                _timeline.Add(new LinearMovement(disk, disk.GetComponent<DiskScript>().PositionAtSelection, disk.transform.position));
                _timelineUI.ReDrawUI();

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

                _timeline.Add(swMovement);
                _timelineUI.ReDrawUI();

                _timeline.SetTimeSpeed(4);
                _timeline.AnimateAtIndex(_timeline.Moves.Count - 1);

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
        _timeline.RemoveAtSelection();
    }

    public void InitiateSave()
    {
        StartCoroutine(FadeSavePopup(false));
    }

    public void Save()
    {
        bool success = true;
        try
        {
            _storageManager.SaveAnimation(_timeline, _saveNameInput.text);
        }
        catch (Exception ex)
        {
            success = false;
            _feedbackText.text = ex.Message;
        }

        if (success)
        {
            StartCoroutine(FadeSavePopup(true));
        }
    }

    public void CancelSave()
    {
        StartCoroutine(FadeSavePopup(true));
    }

    private IEnumerator FadeSavePopup(bool backward)
    {
        CanvasGroup canvasGroup = _savePopUp.GetComponent<CanvasGroup>();
        float duration = 0.1f;
        float time = 0;

        if (!backward)
        {
            _savePopUp.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
        }

        while (time < 1)
        {
            canvasGroup.alpha = Math.Clamp(backward ? 1 - time : time, 0, 1);
            time += Time.deltaTime / duration;
            yield return null;
        }

        canvasGroup.alpha = backward ? 0 : 1;
        if (backward) _savePopUp.gameObject.SetActive(false);
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
