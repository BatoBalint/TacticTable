using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGroups;

    private AnimationMode selectedMode = AnimationMode.None;

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

    public void ButtonTestFunction()
    {
        Debug.Log("Button was pressed");
    }

    public void MoveButtonClick()
    {
        if (selectedMode != AnimationMode.Move)
        {
            selectedMode = AnimationMode.Move;
        }
        else 
        {
            selectedMode = AnimationMode.None;
        }
    }

    private void Update()
    {
        switch (selectedMode)
        {
            case AnimationMode.None:
                MoveAnimationActions();
                break;
            case AnimationMode.Move:
                break;
            case AnimationMode.MoveWithBall:
                break;
            case AnimationMode.Switch:
                break;
            default:
                break;
        }
    }

    private void MoveAnimationActions()
    {
        
    }
}

enum AnimationMode
{
    None,
    Move,
    MoveWithBall,
    Switch,
}
