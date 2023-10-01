using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementButtonScript : MonoBehaviour
{
    public AnimationMode AnimMode;

    private bool _selected = false;
    [SerializeField] private bool _selectable = true;
    private static List<MovementButtonScript> buttons = new List<MovementButtonScript>();
    [SerializeField] private Color _selectionColor = Color.white;

    private void Awake()
    {
        buttons.Add(this);
    }

    public static void UnselectAllButton()
    {
        foreach (var button in buttons)
        {
            button.UnselectButton();
        }
    }

    public void ButtonClick() 
    {
        AnimEditEventSystem.Instance.AnimMenuButtonClick(AnimMode);
    }

    public static void SwitchToButton(AnimationMode mode)
    {
        foreach (MovementButtonScript button in buttons)
        {
            if (button._selected && mode == button.AnimMode)
            {
                button.UnselectButton();
            }
            else if (mode == button.AnimMode)
            {
                button.SelectButton();
            }
            else
            {
                button.UnselectButton();
            }
        }
    }

    public void SelectButton()
    {
        if (_selectable)
        { 
            GetComponent<Image>().color = _selectionColor;
            _selected = true;
        }
    }

    public void UnselectButton()
    {
        GetComponent<Image>().color = Color.white;
        _selected = false;
    }

    public static void ClearStatics()
    {
        buttons.Clear();
    }
}
