using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    CircleCollider2D circleCollider;

    private bool followFinger = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began && circleCollider.bounds.Contains(GetWorldPos(t.position)))
            {
                followFinger = true;
                transform.localScale = new Vector3(1.1f, 1.1f, 0f);
            }

            if (t.phase == TouchPhase.Ended)
            {
                followFinger = false;
                transform.localScale = Vector3.one;
            }

            if (followFinger)
            {
                
                transform.position = GetWorldPos(t.position);
            }

        }
    }

    private Vector3 GetWorldPos(Vector2 pos)
    {
        return Camera.main.ScreenToWorldPoint(pos) + new Vector3(0f, 0f, 10f);
    }
}
