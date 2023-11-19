using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDesign : MonoBehaviour
{
    public Sprite Darksprite;
    public Sprite Lightsprite;
    public TextMeshProUGUI buttonText;

    void Start()
    {
        CustomEventSystem.Instance.onDarkModeChange += ChangeDarkMode;
    }

    private void Awake()
    {
        ChangeDarkMode();
    }

    public void ChangeDarkMode()
    {
        if (ApplicationStateManager.Dark)
        {
            if(buttonText!= null)
            {
                buttonText.color = Color.white;
            }
            GetComponent<Image>().sprite = Darksprite;
        }
        else
        {
            if(buttonText!= null)
            {
                buttonText.color = Color.black;
            }
            GetComponent<Image>().sprite = Lightsprite;
        }
    }

    private void OnDestroy()
    {
        CustomEventSystem.Instance.onDarkModeChange -= ChangeDarkMode;
    }
}
