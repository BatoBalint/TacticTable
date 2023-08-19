using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionsContainer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> korongok;
    private List<Vector3> targetPositions;

    private float slideDuration = 1f; // Time in seconds to complete the slide

    private void Start()
    {
        targetPositions = new List<Vector3>();

        // Populate the targetPositions with child GameObject positions
        for (int i = 0; i < transform.childCount; i++)
        {
            targetPositions.Add(new Vector3(transform.GetChild(i).position.x, transform.GetChild(i).position.y, 0f));

        }
    }

    public void StartSlideRings()
    {
        StartCoroutine(SlideRings());
    }

    private System.Collections.IEnumerator SlideRings()
    {
        float t = 0f;
        Vector3[] startPositions = new Vector3[korongok.Count];

        // Store the starting positions of all objects
        for (int i = 0; i < korongok.Count; i++)
        {
            startPositions[i] = korongok[i].transform.position;
        }

        // Move the objects simultaneously
        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            for (int i = 0; i < korongok.Count; i++)
            {
                korongok[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], t);
            }
            
            yield return null;
        }

        

    }
}
