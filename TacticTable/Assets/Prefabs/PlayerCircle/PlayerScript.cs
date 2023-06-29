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
    private bool ifGrabbedNoCollider = true;

    private Touch myTouch = new Touch();
    private bool followFinger = false;
    private Vector3 grabOffset = Vector3.zero;

    private Vector3 originalScale;
    

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
        if (ifGrabbedNoCollider) circleCollider.isTrigger = true;
    }

    private void GrabEndEvents()
    {
        if (onTouchGrowScale != 1f) transform.localScale = originalScale;
        if (ifGrabbedNoCollider) circleCollider.isTrigger = false;
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos) + new Vector3(0f, 0f, 10f);
    }
}
