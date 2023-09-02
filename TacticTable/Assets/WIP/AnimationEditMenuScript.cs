using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimationEditMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGroups;
    [SerializeField]
    private GameObject messageText;
    [SerializeField]
    private Timeline timeline;
    private TimelineUI timelineUIref;

    private AnimationMode animationMode = AnimationMode.None;
    private Func<int> animationModeUpdate;

    private string baseTopMessage = "Jelenlegi animáció:";


    private void Awake()
    {
        timelineUIref = timeline.GetComponent<TimelineUI>();
        if (timelineUIref == null)
        {
            Debug.Log("Couldnt get component!");
        }
    }

    private void Update()
    {
        if (animationModeUpdate != null)
        {
            animationModeUpdate();
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
        for (int i = 0; i < buttonGroups.transform.childCount; i++)
        {
            buttonGroups.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ResetAnimationMode()
    {
        animationMode = AnimationMode.None;
        animationModeUpdate = null;

        DiskScript.selectOnMove = false;

        messageText.GetComponent<TextMeshProUGUI>().text = baseTopMessage;
    }

    public void ButtonTestFunction()
    {
        Debug.Log("Button was pressed");
    }

    public void MoveButtonClick()
    {
        if (animationMode == AnimationMode.Move)
        {
            ResetAnimationMode();
        }
        else 
        {
            animationMode = AnimationMode.Move;
            animationModeUpdate = MoveUpdate;
            DiskScript.selectOnMove = true;

            messageText.GetComponent<TextMeshProUGUI>().text = baseTopMessage + " Mozgás";
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
            timelineUIref.UpdateTimeline();

            ResetAnimationMode();
            while (DiskScript.selectedDisks.Count > 0) DiskScript.selectedDisks[0].UnselectDisk();
        }
    }

    public void TestFunc()
    {
        timeline.Play();
    }
}

enum AnimationMode
{
    None,
    Move,
    Switch,
    Cross,
}
