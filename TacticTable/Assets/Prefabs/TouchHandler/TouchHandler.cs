using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    private static int prevTouchCount = 0;
    private static List<int> availableTouchIds = new List<int>();

    void Start()
    {
        transform.SetAsFirstSibling();
    }

    void Update()
    {
        if (prevTouchCount < Input.touchCount)
        {
            availableTouchIds.Clear();
            for (int i = prevTouchCount; i <= Input.touchCount; ++i)
            {
                availableTouchIds.Add(i);
            }
        }

        int index = 0;
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Ended)
            {
                PlayerScript.AdjustTouchAllId(index);
                AdjustAvailableIds(index);
            }
            ++index;
        }

        prevTouchCount = Input.touchCount;
    }

    private static void AdjustAvailableIds(int index)
    {
        for (int i = 0; i < availableTouchIds.Count; ++i)
        {
            if (availableTouchIds[i] > index)
            {
                availableTouchIds[i]--;
            }
        }
    }

    public static int GetFingerId()
    {
        int id = availableTouchIds[0];
        availableTouchIds.RemoveAt(0);
        return id;
    }
}
