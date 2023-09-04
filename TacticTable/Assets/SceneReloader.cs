using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Get the current scene's index
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Reload the current scene by its index
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
