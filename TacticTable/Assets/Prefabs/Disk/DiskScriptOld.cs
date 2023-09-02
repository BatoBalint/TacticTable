using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class DiskScriptOld : MonoBehaviour
{ 
    private bool followFinger = false;
    private int touchId = -1;
    private Vector3 touchOffset = Vector2.zero;

    private Vector3 originalScale;
    [SerializeField]
    private float scaleGrowthModifier = 1.1f;
    [SerializeField]
    private bool growWhenGrabbed = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        MoveToTouch();
    }

    // Moves the disk to the touch
    private void MoveToTouch()
    {
        if (followFinger)
        {
            if (Input.touchCount == 0)
            {
                DiskReleased();
                return;
            }

            int i = 0;
            while (i < Input.touchCount && Input.touches[i].fingerId != touchId) ++i;

            transform.position = GetWorldPos(Input.touches[i].position) - touchOffset;
        }
    }

    // Sets the disk to follow the given touch
    public void DiskGrabbed(int touchId)
    {
        this.touchId = touchId;
        touchOffset = GetWorldPos(Input.touches[touchId].position) - transform.position;
        followFinger = true;

        if (growWhenGrabbed) transform.localScale *= scaleGrowthModifier;
    }

    // Releases the touch and sets things back to the original values
    public void DiskReleased()
    {
        touchId = -1;
        followFinger = false;

        transform.localScale = originalScale;
    }

    public CircleCollider2D GetCollider()
    {
        return transform.GetComponent<CircleCollider2D>();
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
