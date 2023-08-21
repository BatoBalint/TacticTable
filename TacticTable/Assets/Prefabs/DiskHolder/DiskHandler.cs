using System.Collections.Generic;
using UnityEngine;

public class DiskHandler : MonoBehaviour
{
    private int prevTouchCount = 0;
    private List<DiskScript> disks;
    private List<CircleCollider2D> diskColliders;
    private Dictionary<int, DiskScript> fingerIdDiskPairs;

    void Start()
    {
        fingerIdDiskPairs = new Dictionary<int, DiskScript>();

        disks = new List<DiskScript>();
        diskColliders = new List<CircleCollider2D>();

        for (int i = 0; i < transform.childCount; i++)
        { 
            disks.Add(transform.GetChild(i).GetComponent<DiskScript>());
            diskColliders.Add(disks[i].GetCollider());
        }
    }

    void Update()
    {
        CheckReleasedTouch();
        if (prevTouchCount != Input.touchCount)
        {
            if (prevTouchCount < Input.touchCount) CheckNewTouch();

            prevTouchCount = Input.touchCount;
        }
    }

    // Handles the new touch and notifies the grabbed disk
    private void CheckNewTouch()
    {
        bool found = false;
        
        for (int i = Input.touchCount - 1; i >= 0 && !found; --i)
        {
            Touch t = Input.touches[i];
            if (!fingerIdDiskPairs.ContainsKey(t.fingerId))
            { 
                for (int j = 0; j < diskColliders.Count && !found; ++j)
                {
                    if (diskColliders[j].bounds.Contains(GetWorldPos(t.position)))
                    {
                        found = true;
                        
                        disks[j].DiskGrabbed(t.fingerId);
                        if (!fingerIdDiskPairs.ContainsKey(t.fingerId)) fingerIdDiskPairs.Add(t.fingerId, disks[j]);
                        else fingerIdDiskPairs[t.fingerId] = disks[j];
                    }
                }
            }
        }
    }

    // Checks for touch releases and notifies the disk
    private void CheckReleasedTouch()
    {
        
        foreach (Touch t in Input.touches) 
        {
            if (t.phase == TouchPhase.Ended && fingerIdDiskPairs.ContainsKey(t.fingerId))
            {
                fingerIdDiskPairs[t.fingerId].DiskReleased();
                fingerIdDiskPairs.Remove(t.fingerId);
            }
        }
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
