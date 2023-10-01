using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineItemScript : MonoBehaviour
{
    [SerializeField] private float _whiteningIntensity = 0.75f;

    public bool IsHighlighted = false;

    private Image _image;
    public Color BaseColor;
    public Color HighlightColor;

    public void Awake()
    {
        _image = GetComponent<Image>();
        BaseColor = _image.color;
        HighlightColor = Color.Lerp(BaseColor, Color.white, _whiteningIntensity);
    }

    public void Highlight()
    {
        _image.color = HighlightColor;
    }

    public void CancelHighlight()
    {
        _image.color = BaseColor;
    }
}
