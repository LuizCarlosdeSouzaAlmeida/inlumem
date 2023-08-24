
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FadeText : MonoBehaviour
{
    public TMP_Text text;

    public void ShowText()
    {
        StartCoroutine(FadeTextToFullAlpha());
    }

    public void HideText()
    {
        StartCoroutine(FadeTextToZeroAlpha());
    }

    private IEnumerator FadeTextToFullAlpha()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator FadeTextToZeroAlpha()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime));
            yield return null;
        }
    }
}
