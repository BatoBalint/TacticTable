using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEditSceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        ResetScene();
        SceneManager.LoadScene(sceneName);
    }

    private void ResetScene()
    {
        while (DiskScript.selectedDisks.Count > 0) DiskScript.selectedDisks[0].UnselectDisk();
        Timeline.ClearMoves();
    }
}
