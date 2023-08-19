using UnityEngine;
using TMPro;

public class PrefabController : MonoBehaviour
{
    public GameObject prefabToControl;
    public TMP_Text buttonText; // Reference to the TextMeshPro Text component

    private bool isPrefabActive = false; // Track the active state

    private void Start()
    {
        UpdateButtonText();
    }

    public void TogglePrefab()
    {
        isPrefabActive = !isPrefabActive;
        prefabToControl.SetActive(isPrefabActive);
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        buttonText.text = isPrefabActive ? "Stop" : "Draw";
    }
}
