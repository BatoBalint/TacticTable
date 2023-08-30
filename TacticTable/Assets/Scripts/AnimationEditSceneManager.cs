using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEditSceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
