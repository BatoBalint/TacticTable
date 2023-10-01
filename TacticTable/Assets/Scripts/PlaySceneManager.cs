using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField] private List<DiskSpawnerScript> _diskSpawners = new List<DiskSpawnerScript>();
    [SerializeField] private PositionScalerScript _positionScalerScript;
    [SerializeField] private Transform _savedAnimationsContainer;
    [SerializeField] private GameObject _savedAnimationButtonPrefab;
    [SerializeField] private GameObject _savedAnimDeletButton;

    private SavedAnimationButtonScript _savedAnimationButtonScript;
    private bool _confirmationOffered = false;

    private StorageManager _storageManager;

    public void Start()
    {
        _storageManager = new StorageManager();
        
        InitializeDisks();

        LoadSavedAnimations();
    }

    private void InitializeDisks()
    {
        foreach (DiskSpawnerScript spawner in _diskSpawners)
        {
            spawner.SpawnDisks();
        }
        if (_positionScalerScript != null)
        {
            _positionScalerScript.ScalePositions();
            _positionScalerScript.MoveDisksToDefaults();
        }
    }

    private void LoadSavedAnimations()
    {
        foreach (Transform child in _savedAnimationsContainer)
        {
            Destroy(child.gameObject);
        }

        Dictionary<string, string> saves = _storageManager.GetSavedAnimations();
        foreach (var anim in saves)
        {
            GameObject newButton = Instantiate(_savedAnimationButtonPrefab);
            newButton.transform.SetParent(_savedAnimationsContainer, false);
            newButton.GetComponent<SavedAnimationButtonScript>().Init(anim.Key, anim.Value);
            newButton.GetComponent<SavedAnimationButtonScript>().SetSceneManager(this);
        }
    }

    public void DeleteSavedAnimation()
    {
        if (!_confirmationOffered)
        {
            _savedAnimDeletButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Biztos?";
            _confirmationOffered = true;
        }
        else
        {
            try
            {
                _storageManager.DeleteAnimation(_savedAnimationButtonScript.AnimationName);
            }
            catch (Exception) { }
            LoadSavedAnimations();
            ResetDeleteButton(false);
        }
    }

    private void ResetDeleteButton(bool active)
    {
        _savedAnimDeletButton.SetActive(active);
        _confirmationOffered = false;
    }

    public void SelectAnimationButton(SavedAnimationButtonScript script)
    {
        if (_savedAnimationButtonScript == null)
        {
            _savedAnimationButtonScript = script;
            ResetDeleteButton(true);
        }
        else if (_savedAnimationButtonScript == script)
        {
            _savedAnimationButtonScript.Unselect();
            _savedAnimationButtonScript = null;
            ResetDeleteButton(false);
        }
        else
        {
            _savedAnimationButtonScript.Unselect();
            _savedAnimationButtonScript = script;
            ResetDeleteButton(true);
        }
    }

    public void ChangeToAnimationEditScene()
    {
        ChangeScene("AnimationEditScene");
    }

    public void ChangeScene(string sceneName)
    {
        ResetScene();
        SceneManager.LoadScene(sceneName);
    }

    private void ResetScene()
    {
        while (DiskScript.SelectedDisks.Count > 0) DiskScript.SelectedDisks[0].UnselectDisk();
    }
}
