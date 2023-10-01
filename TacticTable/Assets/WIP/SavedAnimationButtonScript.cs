using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SavedAnimationButtonScript : MonoBehaviour
{
    private PlaySceneManager _sceneManager;

    private TextMeshProUGUI _text;
    private string _timelineAsJsonString;
    private string _animationName;

    public void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void ButtonClick()
    {
        ApplicationStateManager.TimelineToLoad = _timelineAsJsonString;
        ApplicationStateManager.AnimationName = _animationName;
        _sceneManager.ChangeToAnimationEditScene();
    }

    public void Init(string name, string timelineJson)
    {
        _timelineAsJsonString = timelineJson;
        _animationName = name;
        _text.text = name;
    }

    public void SetSceneManager(PlaySceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }
}
