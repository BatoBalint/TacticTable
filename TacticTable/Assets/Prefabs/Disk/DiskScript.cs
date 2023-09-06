using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class DiskScript : MonoBehaviour
{
    public static List<DiskScript> SelectedDisks = new List<DiskScript>();
    public static bool SelectOnMove = false;

    public bool Selectable = true;
    public bool IsBall = false;
    public bool IsBlue = false;
    public Vector3 PositionAtSelection = Vector3.zero;
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
                if (!diskIsSelected && SelectOnMove)
                {
                    SelectDisk();
                }
                transform.position = ConvertToWorldPosition(t.position) - touchOffset;
                if (AnimEditEventSystem.Instance != null)
                {
                    AnimEditEventSystem.Instance.DiskPositionChange(this);
                }
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
        if (Selectable)
        {
            diskSelectionMarker.SetActive(true);
            diskIsSelected = true;

            PositionAtSelection = transform.position;
            SelectedDisks.Add(this);
        }
    }

    public void UnselectDisk()
    {
        diskSelectionMarker.SetActive(false);
        diskIsSelected = false;

        SelectedDisks.Remove(this);
    }

    private Vector3 ConvertToWorldPosition(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
