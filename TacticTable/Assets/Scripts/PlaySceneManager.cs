using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField] private List<DiskSpawnerScript> _diskSpawners = new List<DiskSpawnerScript>();
    [SerializeField] private PositionScalerScript _positionScalerScript;
    [SerializeField] private Transform _savedAnimationsContainer;
    [SerializeField] private GameObject _savedAnimationButton;

    private StorageManager storageManager;

    public void Start()
    {
        storageManager = new StorageManager();
        
        InitializeDisks();

        storageManager.SaveAnimation("test", "TestAnimation");

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
        Dictionary<string, string> saves = storageManager.GetSavedAnimations();
        foreach (var anim in saves)
        {
            GameObject newButton = Instantiate(_savedAnimationButton);
            newButton.transform.SetParent(_savedAnimationsContainer, false);
            newButton.GetComponent<SavedAnimationButtonScript>().Init(anim.Key, anim.Value);
            newButton.GetComponent<SavedAnimationButtonScript>().SetSceneManager(this);
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
