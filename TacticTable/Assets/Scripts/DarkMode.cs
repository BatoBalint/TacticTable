using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class DarkMode : MonoBehaviour
{
    public bool Dark = false;
    public GameObject DarkBackgroundSprite;
    public GameObject LightBackgroundSprite;

    public void darkMode()
    {
        Color c = new Color(36/255f, 59/255f, 110 / 255f);
        
        if (Dark)
        {
            Dark = false;
            DarkBackgroundSprite.SetActive(false);
            LightBackgroundSprite.SetActive(true);
            Camera.main.backgroundColor = Color.white;
        }
        else
        {
            Dark = true;
            LightBackgroundSprite.SetActive(false);
            DarkBackgroundSprite.SetActive(true);
            Camera.main.backgroundColor = c;
        }
        CustomEventSystem.Instance.ChangeDarkMode(this);
    }
}
