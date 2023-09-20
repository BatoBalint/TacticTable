using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }

    private void ResetScene()
    {
        foreach (var disk in DiskScript.SelectedDisks)
        {
            disk.UnselectDisk();
        }
    }
}
