using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimEditSceneManager : MonoBehaviour
{
    [SerializeField] private Timeline _timeline;

    public void ChangeScene(string sceneName)
    {
        ResetScene();
        SceneManager.LoadScene(sceneName);
    }

    private void ResetScene()
    {
        while (DiskScript.SelectedDisks.Count > 0) DiskScript.SelectedDisks[0].UnselectDisk();
        _timeline.ClearMoves();
    }
}
