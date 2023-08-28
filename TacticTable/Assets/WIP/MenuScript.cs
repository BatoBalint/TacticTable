using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonGroups;

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
}
