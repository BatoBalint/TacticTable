using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class DiskScript : MonoBehaviour
{
    public static List<DiskScript> selectedDisks = new List<DiskScript>();
    public static bool selectOnMove = false;

    public bool selectable = true;
    public Vector3 positionAtSelection = Vector3.zero;
    [SerializeField]
    private GameObject diskSelectionMarker;
    private CircleCollider2D circleCollider;

    private bool diskIsSelected = false;
    private bool followFinger = false;
    private int fingerId = -1;
    private float touchStart = 0;
    private Vector3 touchOffset;

    public void Awake()
    {
        circleCollider = transform.GetComponent<CircleCollider2D>();
    }

    public void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);

            if (circleCollider.bounds.Contains(ConvertToWorldPosition(t.position)) && t.phase == TouchPhase.Began)
            {
                followFinger = true;
                fingerId = t.fingerId;
                touchStart = Time.time;
                touchOffset = ConvertToWorldPosition(t.position) - transform.position;
            }
            else if (followFinger && t.fingerId == fingerId && t.phase == TouchPhase.Moved)
            {
                if (!diskIsSelected && selectOnMove)
                {
                    SelectDisk();
                }
                transform.position = ConvertToWorldPosition(t.position) - touchOffset;
            }
            else if (followFinger && t.fingerId == fingerId && t.phase == TouchPhase.Ended)
            {
                if (Time.time - touchStart < 0.3) TouchTap();
                followFinger = false;
            }

            ++i;
        }
    }

    private void TouchTap()
    {
        ToggleDiskSelection();
    }

    public void TurnOffCollision()
    {
        circleCollider.isTrigger = true;
    }

    public void TurnOnCollisoin()
    { 
        circleCollider.isTrigger = false;
    }

    private void ToggleDiskSelection()
    {
        if (diskIsSelected)
        {
            UnselectDisk();
        }
        else
        {
            SelectDisk();
        }
    }

    public void SelectDisk()
    {
        if (selectable)
        {
            diskSelectionMarker.SetActive(true);
            diskIsSelected = true;

            positionAtSelection = transform.position;
            selectedDisks.Add(this);
        }
    }

    public void UnselectDisk()
    {
        diskSelectionMarker.SetActive(false);
        diskIsSelected = false;

        selectedDisks.Remove(this);
    }

    private Vector3 ConvertToWorldPosition(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
