using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimMenuButtonScript : MonoBehaviour
{
    public AnimationMode AnimMode;

    private bool _selected = false;
    [SerializeField] private bool _selectable = true;
    private static List<AnimMenuButtonScript> buttons = new List<AnimMenuButtonScript>();

    private void Awake()
    {
        buttons.Add(this);
    }

    public void ButtonClick() 
    {
        if (_selected)
        {
            UnselectButton();
        }
        else
        {
            foreach (var button in buttons)
            {
                button.UnselectButton();
            }
            SelectButton();
        }
        AnimEditEventSystem.Instance.AnimMenuButtonClick(AnimMode);
    }

    public void SelectButton()
    {
        if (_selectable)
        { 
            GetComponent<Image>().color = Color.green;
            _selected = true;
        }
    }

    public void UnselectButton()
    {
        GetComponent<Image>().color = Color.white;
        _selected = false;
    }
}
