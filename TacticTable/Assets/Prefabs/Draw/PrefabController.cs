using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PrefabController : MonoBehaviour
{
    public GameObject prefabToControl;
    //public TMP_Text buttonText; // Reference to the TextMeshPro Text component

    public GameObject extraButtons;

    public TextMeshProUGUI buttonText;

    private bool isPrefabActive = false; // Track the active state

    private void Start()
    {
        UpdateButtonText();
        extraButtons.SetActive(isPrefabActive);
    }

    public void TogglePrefab()
    {
        if(!isPrefabActive)
        {

        }
        isPrefabActive = !isPrefabActive;    
        extraButtons.SetActive(isPrefabActive);
        prefabToControl.SetActive(isPrefabActive);
        UpdateButtonText();

    }

    private void UpdateButtonText()
    {
        buttonText.text = isPrefabActive ? "Stop" : "Draw";
    }
}
