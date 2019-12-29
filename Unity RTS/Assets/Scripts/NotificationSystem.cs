using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public static TextMeshProUGUI text;
    public static Color textColor;

    public float colorFadeTime = 0.5f;
    private static float currentColorFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        currentColorFadeTime = colorFadeTime;
    }

    public static void Notify(string message, Color messageColor)
    {
        text.SetText(message);
        text.color = messageColor;

        text.StartCoroutine(FadeText());
    }

    private static IEnumerator FadeText()
    {
        while(!text.color.IsZeroAlpha())
        {
            currentColorFadeTime -= Time.deltaTime;
            text.color = text.color.FadeAlpha(0.02f);
            yield return null;
        }
    }
}
