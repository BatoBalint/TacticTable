using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    CircleCollider2D circleCollider;
    [SerializeField]
    private float onTouchGrowScale = 1.1f;
    [SerializeField]
    private bool ifGrabbedNoCollider = false;   // can drag over other players without moving them

    private static List<PlayerScript> pScripts = new List<PlayerScript>();
    private Touch myTouch;                     // the touch that is moving the player
    private bool touchIsActive = false;
    private Vector3 grabOffset = Vector3.zero;  // the player and the touch offset

    private Vector3 originalScale;              // for later developement if scale must be changed
    
    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        CheckInputs();

        /*if (touchIsActive) MoveFigure();*/
    }

    private void CheckInputs() 
    {
        foreach (Touch t in Input.touches)
        {
            CheckFingerInput(t);
        }
        if (touchIsActive && Input.GetTouch(myTouch.fingerId).phase == TouchPhase.Ended)
        {
            AdjustTouchIds(myTouch.fingerId);
            pScripts.Remove(this);
            touchIsActive = false;

            GrabEndEvents();
        }
    }

    private void MoveFigure() 
    {
        transform.position = GetWorldPos(Input.GetTouch(myTouch.fingerId).position) - grabOffset;
    }

    private void CheckFingerInput(Touch t)
    {
        if (t.phase == TouchPhase.Began && circleCollider.bounds.Contains(GetWorldPos(t.position)) && !touchIsActive)
        {
            grabOffset = GetWorldPos(t.position) - transform.position;
            myTouch = t;
            touchIsActive = true;
            pScripts.Add(this);

            GrabStartEvents();
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

    private void AdjustTouchIds(int removedIndex) 
    {
        foreach (var s in pScripts)
        {
            //TODO
        }
    }

    static String listToString(String prefix, List<int> list)
    {
        String result = prefix;
        String deli = "";
        foreach (int i in list)
        {
            result += deli + " " + i;
            deli = ",";
        }
        return result;
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
