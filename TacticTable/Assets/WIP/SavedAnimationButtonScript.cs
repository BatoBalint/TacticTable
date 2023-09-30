using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavedAnimationButtonScript : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private string _timelineAsJsonString;
    private string _animationName;

    public void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ButtonClick()
    {
        ApplicationStateManager._timelineToLoad = _timelineAsJsonString;
        ApplicationStateManager._animationName = _animationName;
    }

    public void Init(string name, string timelineJson)
    { 
        _timelineAsJsonString = timelineJson;
        _animationName = name;
        _text.text = name;
    }
}
