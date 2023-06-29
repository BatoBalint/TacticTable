using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    CircleCollider2D circleCollider;
    [SerializeField]
    private float onTouchGrowScale = 1.1f;
    [SerializeField]
    private bool ifGrabbedNoCollider = false;   // can drag over other players without moving them

    private Touch myTouch = new Touch();        // the touch that is moving the player
    private bool followFinger = false;          // the player should follow the touch
    private Vector3 grabOffset = Vector3.zero;  // the player and the touch offset

    private Vector3 originalScale;              // for later developement if scale must be changed
    
    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs() 
    {
        foreach (Touch t in Input.touches)
        {
            CheckFingerInput(t);
        }
    }

    private void CheckFingerInput(Touch t)
    {
        if (t.phase == TouchPhase.Began && circleCollider.bounds.Contains(GetWorldPos(t.position)) && !followFinger)
        {
            grabOffset = GetWorldPos(t.position) - transform.position;
            followFinger = true;
            myTouch = t;

            GrabStartEvents();
        }

        if (t.fingerId == myTouch.fingerId && t.phase == TouchPhase.Ended)
        {
            followFinger = false;

            GrabEndEvents();
        }

        if (followFinger && t.fingerId == myTouch.fingerId)
        { 
            transform.position = GetWorldPos(t.position) - grabOffset;
        }
    }

    private void GrabStartEvents()
    {
        if (onTouchGrowScale != 1f) transform.localScale = originalScale * onTouchGrowScale;
        if (ifGrabbedNoCollider) circleCollider.enabled = false;
    }

    private void GrabEndEvents()
    {
        if (onTouchGrowScale != 1f) transform.localScale = originalScale;
        if (ifGrabbedNoCollider) circleCollider.enabled = true;
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
