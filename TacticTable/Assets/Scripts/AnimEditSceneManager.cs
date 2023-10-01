using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimEditSceneManager : MonoBehaviour
{
    [SerializeField] private Timeline _timeline;
    [SerializeField] private List<DiskSpawnerScript> _diskSpawners = new List<DiskSpawnerScript>();
    [SerializeField] private PositionScalerScript _positionScalerScript;

    public void Start()
    {
        InitializeDisks();

        if (ApplicationStateManager.TimelineToLoad != null)
        {
            _timeline.LoadFromJSON(ApplicationStateManager.TimelineToLoad);
        }
    }

    private void InitializeDisks()
    {
        foreach (var spawner in _diskSpawners)
        {
            spawner.SpawnDisks();
        }
        if (_positionScalerScript != null)
        { 
            _positionScalerScript.ScalePositions();
            _positionScalerScript.MoveDisksToDefaults();
        }
    }

    public void ChangeScene(string sceneName)
    {
        ResetScene();
        SceneManager.LoadScene(sceneName);
    }

    private void ResetScene()
    {
        while (DiskScript.SelectedDisks.Count > 0) DiskScript.SelectedDisks[0].UnselectDisk();
        DiskScript.DiskScripts.Clear();
        _timeline.ClearMoves();
        MovementButtonScript.ClearStatics();
        ApplicationStateManager.TimelineToLoad = null;
        ApplicationStateManager.AnimationName = "";
    }
}
