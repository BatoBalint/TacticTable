using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class DiskScriptSecond : MonoBehaviour
{
    public static List<DiskScriptSecond> selectedDisks = new List<DiskScriptSecond>();

    [SerializeField]
    private GameObject diskSelectionMarker;
    private new CircleCollider2D collider;

    private bool diskIsSelected = false;
    private bool followFinger = false;
    private int fingerId = -1;
    private float touchStart = 0;
    private Vector3 touchOffset;


    public void Awake()
    {
        collider = transform.GetComponent<CircleCollider2D>();
    }

    public void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);

            if (collider.bounds.Contains(GetWorldPos(t.position)) && t.phase == TouchPhase.Began)
            {
                followFinger = true;
                fingerId = t.fingerId;
                touchStart = Time.time;
                touchOffset = GetWorldPos(t.position) - transform.position;
            }
            else if (followFinger && t.fingerId == fingerId && t.phase == TouchPhase.Moved)
            {
                transform.position = GetWorldPos(t.position) - touchOffset;
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

    private void ToggleDiskSelection()
    {
        if (diskIsSelected)
        {
            diskSelectionMarker.SetActive(false);
            diskIsSelected = false;
        }
        else
        {
            diskSelectionMarker.SetActive(true);
            diskIsSelected = true;
        }
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
