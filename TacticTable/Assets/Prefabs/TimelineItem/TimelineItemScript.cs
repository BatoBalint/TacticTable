using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineItemScript : MonoBehaviour
{
    [SerializeField] private float _whiteningIntensity = 0.75f;

    public bool IsHighlighted = false;
    private bool _isSelected = false;

    private Image _image;
    public Color BaseColor;     // #4DF3EE
    public Color HighlightColor;
    public Color SelectColor;

    public TimelineUI ParentTimelineUI;
    public int Index = -1;

    public void Awake()
    {
        _image = GetComponent<Image>();
        BaseColor = _image.color;
        HighlightColor = Color.Lerp(BaseColor, Color.white, _whiteningIntensity);
        SelectColor = new Color(240f / 255, 110f / 255, 40f / 255);
    }

    public void Highlight()
    {
        _image.color = HighlightColor;
    }

    public void CancelHighlight()
    {
        _image.color = BaseColor;
    }

    public void Click()
    {
        Select();
        ParentTimelineUI.SelectTimelineItem(this);
    }

    public void Select()
    {
        if (!_isSelected)
        { 
            _image.color = SelectColor;
            _isSelected = true;
        }
    }

    public void Unselect()
    {
        if (_isSelected)
        {
            _image.color = BaseColor;
            _isSelected = false;
        }
    }
}
