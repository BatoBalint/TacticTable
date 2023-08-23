using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private LineScript _linePrefab;

    private LineScript _currentLine;

    public const float RESOLUTION = .1f;

    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            // Edit made by Balint
            if (_currentLine != null)
            {
                _currentLine.Test();
            }
            // Edit end
            _currentLine = Instantiate(_linePrefab, mousePos, Quaternion.identity);
        }

        if (Input.GetMouseButton(0))
        {
            _currentLine.SetPosition(mousePos);
        }
    }
}
