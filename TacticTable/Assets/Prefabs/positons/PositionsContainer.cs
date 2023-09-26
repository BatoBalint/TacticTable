using UnityEngine;

public class PositionsContainer : MonoBehaviour
{
    [SerializeField]
    private Transform BlueDisksHolder;
    [SerializeField]
    private Transform RedDisksHolder;

    private float slideDuration = 1f; // Time in seconds to complete the slide

    private System.Collections.IEnumerator Move(int index)
    {
        float t = 0f;

        Transform redPositions = transform.GetChild(index).GetChild(0);
        
        Transform bluePositions = transform.GetChild(index).GetChild(1);

        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            for (int i = 0; i < redPositions.childCount && i < RedDisksHolder.childCount && i < bluePositions.childCount && i < BlueDisksHolder.childCount; i++)
            {
                BlueDisksHolder.GetChild(i).position = Vector3.Lerp(BlueDisksHolder.GetChild(i).position, bluePositions.GetChild(i).position,t);
                
                RedDisksHolder.GetChild(i).position = Vector3.Lerp(RedDisksHolder.GetChild(i).position, redPositions.GetChild(i).position,t);
            }

            yield return null;
        }
    }


     public void StartSix()
     {
        StopAllCoroutines();
        StartCoroutine(Move(0));
     }
    public void StartFiveOne()
    {
        StopAllCoroutines();
        StartCoroutine(Move(1));
    }
    public void StartFourTwo()
    {
        StopAllCoroutines();
        StartCoroutine(Move(2));
    }
    public void StartThreeTwoOne()
    {
        StopAllCoroutines();
        StartCoroutine(Move(3));
    }
}
