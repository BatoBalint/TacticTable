using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int idAdjustment = 0;
    private bool touchIsActive = false;
    private int fingerId = -1;
    private Vector3 grabOffset = Vector3.zero;  // the player and the touch offset

    private Vector3 originalScale;              // for later developement if scale must be changed
    
    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        CheckInputs();

        if (touchIsActive) MoveFigure();
    }

    private void CheckInputs() 
    {
        foreach (Touch t in Input.touches)
        {
            CheckTouchInput(t);
        }

        if (touchIsActive && Input.touches[fingerId].phase == TouchPhase.Ended)
        {
            TouchEnded();
        }

        fingerId -= idAdjustment;
        idAdjustment = 0;
    }
    
    public void TouchEnded() 
    {
        pScripts.Remove(this);
        touchIsActive = false;
        fingerId = -1;

        GrabEndEvents();
    }

    private void MoveFigure() 
    {
        if (fingerId == -1) return;
        transform.position = GetWorldPos(Input.touches[fingerId].position) - grabOffset;
    }

    private void CheckTouchInput(Touch t)
    {
        if (t.phase == TouchPhase.Began && circleCollider.bounds.Contains(GetWorldPos(t.position)) && !touchIsActive)
        {
            fingerId = TouchHandler.GetFingerId();

            grabOffset = GetWorldPos(t.position) - transform.position;
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

    public static void AdjustTouchAllId(int removedIndex)
    {
        foreach (var s in pScripts)
        {
            s.AdjustOwnId(removedIndex);
        }
    }

    private void AdjustOwnId(int releasedId)
    {
        if (releasedId < fingerId)
        {
            idAdjustment++;
        }
    }

    // Debug
    /*static String listToString(String prefix, List<int> list)
    {
        String result = prefix;
        String deli = "";
        foreach (int i in list)
        {
            result += deli + " " + i;
            deli = ",";
        }
        return result;
    }*/

    private Vector3 GetWorldPos(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
