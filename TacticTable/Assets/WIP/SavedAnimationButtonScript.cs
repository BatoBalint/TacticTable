using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedAnimationButtonScript : MonoBehaviour
{
    private PlaySceneManager _sceneManager;

    private TextMeshProUGUI _text;
    private string _timelineAsJsonString;
    public string AnimationName;

    private Color _baseColor;
    private Color _selectionColor = new Color(240f / 255, 110f / 255, 40f / 255);
    private bool _isSelected;

    private bool _cancelButtonClick = false;

    public void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _baseColor = GetComponent<Image>().color;
    }

    public void ButtonClick()
    {
        if (_cancelButtonClick)
        {
            _cancelButtonClick = false;
            return;
        }
        ApplicationStateManager.TimelineToLoad = _timelineAsJsonString;
        ApplicationStateManager.AnimationName = AnimationName;
        _sceneManager.ChangeToAnimationEditScene();
    }

    public void LongPress()
    {
        _cancelButtonClick = true;
        Select();
    }

    public void Select()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            GetComponent<Image>().color = _selectionColor;
        }
        _sceneManager.SelectAnimationButton(this);
    }

    public void Unselect()
    {
        if (_isSelected)
        {
            _isSelected = false;
            GetComponent<Image>().color = _baseColor;
        }
    }

    public void Init(string name, string timelineJson)
    {
        _timelineAsJsonString = timelineJson;
        AnimationName = name;
        _text.text = name;
    }

    public void SetSceneManager(PlaySceneManager sceneManager)
    {
        _sceneManager = sceneManager;
    }
}
