using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreemodeMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject _savedAnimationsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnimationsButtonClick()
    {
        StartCoroutine(FadeCanvasGroup(_savedAnimationsPanel.GetComponent<CanvasGroup>(), 0, 1));
    }

    public void SavedAnimationsCloseButtonClick()
    {
        StartCoroutine(FadeCanvasGroup(_savedAnimationsPanel.GetComponent<CanvasGroup>(), 1, 0));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end)
    {
        float duration = 0.2f;
        float value = 0;
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        if (start == 0)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        while (value < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, curve.Evaluate(value / duration));
            value += Time.deltaTime;

            if (end == 0 && value >= duration)
            {
                canvasGroup.gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
