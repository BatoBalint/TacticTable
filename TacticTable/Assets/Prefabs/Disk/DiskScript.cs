using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using UnityEngine;

public class DiskScript : MonoBehaviour
{
    // Global variables
    public static List<DiskScript> DiskScripts = new List<DiskScript>();
    public static List<DiskScript> SelectedDisks = new List<DiskScript>();
    public static bool SelectOnMove = false;

    public bool Selectable = true;
    public bool IsBall = false;
    public bool IsBlue = false;
    public int Id = -1;

    private CircleCollider2D _circleCollider;

    // Selection related variables
    [SerializeField]
    private GameObject _diskSelectionMarker;
    public Vector3 PositionAtSelection { get; private set; }
    private bool _diskIsSelected = false;

    // Movement related variables
    private bool _followFinger = false;
    private int _fingerId = -1;
    private float _touchStart = 0;      // Stores the time at touch start
    private Vector3 _touchOffset;
    private float _moveSinceGrab = 0f;  // distance traveld since grab event

    public void Awake()
    {
        _circleCollider = transform.GetComponent<CircleCollider2D>();
        PositionAtSelection = Vector3.zero;

        DiskScripts.Add(this);
    }

    public void Update()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            Vector3 touchWorldPos = ConvertToWorldPosition(t.position);

            // Handling touch start
            if (_circleCollider.bounds.Contains(touchWorldPos) && t.phase == TouchPhase.Began)
            {
                _followFinger = true;
                _fingerId = t.fingerId;
                _touchStart = Time.time;
                _touchOffset = touchWorldPos - transform.position;
            }
            // Handling drag
            else if (_followFinger && t.fingerId == _fingerId && t.phase == TouchPhase.Moved)
            {
                if (!_diskIsSelected && SelectOnMove)
                {
                    SelectDisk();
                }
                _moveSinceGrab += Vector3.Distance(touchWorldPos, transform.position);
                transform.position = touchWorldPos - _touchOffset;
                if (AnimEditEventSystem.Instance != null)
                {
                    AnimEditEventSystem.Instance.DiskPositionChange(this);
                }
            }
            // Handling touch release
            else if (_followFinger && t.fingerId == _fingerId && t.phase == TouchPhase.Ended)
            {
                if (Time.time - _touchStart < 0.3 && _moveSinceGrab < 0.2f) TouchTap();
                _followFinger = false;
                _moveSinceGrab = 0;
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
        _circleCollider.isTrigger = true;
    }

    public void TurnOnCollisoin()
    { 
        _circleCollider.isTrigger = false;
    }

    private void ToggleDiskSelection()
    {
        if (_diskIsSelected)
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
            _diskSelectionMarker.SetActive(true);
            _diskIsSelected = true;

            PositionAtSelection = transform.position;
            SelectedDisks.Add(this);
        }
    }

    public void UnselectDisk()
    {
        _diskSelectionMarker.SetActive(false);
        _diskIsSelected = false;

        SelectedDisks.Remove(this);
    }

    public Dictionary<string, string> ConvertToDictionary()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        dic.Add("id", Id.ToString());
        dic.Add("color", IsBlue ? "blue" : "red");
        dic.Add("ball", IsBall.ToString());
        dic.Add("x", transform.position.x.ToString());
        dic.Add("y", transform.position.y.ToString());

        return dic;
    }

    private Vector3 ConvertToWorldPosition(Vector2 pos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos.z = 0;
        return worldPos;
    }
}
