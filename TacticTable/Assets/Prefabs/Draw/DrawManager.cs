using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] private LineScript _linePrefab;

    private LineScript _currentLine;

    public const float RESOLUTION = .1f;

    private List<LineScript> _lines; 

    private RectTransform rect;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        _lines = new List<LineScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePos2 = _cam.ScreenToWorldPoint(Input.touch);

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(mousePos + ", " + _linePrefab);
            _currentLine = Instantiate(_linePrefab.gameObject, mousePos, Quaternion.identity,transform).GetComponent<LineScript>();
            _lines.Add(_currentLine);
        }

        if (Input.GetMouseButton(0))
        {
            if (_currentLine != null)
            {
                _currentLine.SetPosition(mousePos);
            }
        }
    }
    
    public void DeletLast()
    {
        //Debug.Log(_lines.Count + ", ");
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
        //Debug.Log(_lines.Count - 1 + ", ");
        int listLength = _lines.Count-1;
        LineScript delet;
        for (int i = listLength; i >= 0; i--)
        {
            delet = _lines[i];
            //Debug.Log(i + ", ");
            if (_lines[i] != null)
            {
                Destroy(_lines[i].gameObject);
            }
            _lines.Remove(delet);
            
        }
    }
}
