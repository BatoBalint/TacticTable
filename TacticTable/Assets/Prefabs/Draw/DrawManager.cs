using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private LineScript _linePrefab;

    private LineScript _currentLine;

    public const float RESOLUTION = .1f;

    private List<LineScript> _lines;

    private BoxCollider2D boxCollider;

    private bool outOfBounds = false;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Awake()
    {
        _lines = new List<LineScript>();
        boxCollider= GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if(boxCollider.bounds.Contains(mousePos))
            {
                _currentLine = Instantiate(_linePrefab.gameObject, mousePos, Quaternion.identity, transform).GetComponent<LineScript>();
                _lines.Add(_currentLine);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (boxCollider.bounds.Contains(mousePos))
            {
                if (outOfBounds)
                {
                    _currentLine = Instantiate(_linePrefab.gameObject, mousePos, Quaternion.identity, transform).GetComponent<LineScript>();
                    outOfBounds = false;
                    _lines.Add(_currentLine);
                }

                if (_currentLine != null)
                {
                    _currentLine.SetPosition(mousePos);
                }
            }
            else
            {
                outOfBounds= true;
            }
        }
    }


    public void DeletLast()
    {
        int listLength = _lines.Count;
        LineScript delet = _lines[listLength-1];

        if (listLength>=2)
        {
            Destroy(_lines[listLength - 1].gameObject);
            _lines.Remove(delet);
            listLength--;
            delet = _lines[listLength - 1];
            Destroy(_lines[listLength - 1].gameObject);
            _lines.Remove(delet);
            listLength--;
        }
    }

    public void DeleteAll()
    {
        int listLength = _lines.Count-1;
        LineScript delet;
        for (int i = listLength; i >= 0; i--)
        {
            delet = _lines[i];
            if (_lines[i] != null)
            {
                Destroy(_lines[i].gameObject);
            }
            _lines.Remove(delet);

        }
    }
}
