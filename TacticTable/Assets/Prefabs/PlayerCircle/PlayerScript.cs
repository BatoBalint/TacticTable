using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    CircleCollider2D circleCollider;

    private bool followFinger = false;
    private Vector3 grabOffset = Vector3.zero;

    private Vector3 originalScale;
    [SerializeField]
    private float onTouchGrowScale = 1.1f;

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
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began && circleCollider.bounds.Contains(GetWorldPos(t.position)))
            {
                followFinger = true;
                grabOffset = GetWorldPos(t.position) - transform.position;
                transform.localScale = originalScale * onTouchGrowScale;
            }

            if (t.phase == TouchPhase.Ended)
            {
                followFinger = false;
                transform.localScale = originalScale;
            }

            if (followFinger)
            {
                transform.position = GetWorldPos(t.position) - grabOffset;
            }
        }
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos) + new Vector3(0f, 0f, 10f);
    }
}
