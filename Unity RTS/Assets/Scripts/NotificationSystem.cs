using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum RTSEvent
{
    RallyPointSet,
    BuildingCompleted,
    UnitCompleted,
    UpgradeCompleted,
    AbilityActivated,
    AbilityDeactivated
}

public class NotificationSystem : MonoBehaviour
{
    //Just so we can set an instance in the editor
    public TextMeshProUGUI UIText; 
    public Color colorOfText = Color.red;

    //Actual members that will be affected
    public static TextMeshProUGUI text;
    public static Color textColor = Color.red;

    public static float fadeIncrement = 0.015f;  //Random number i found to look nice

    // Start is called before the first frame update
    void Start()
    {
        //Give static variables references to the items exposed to the editor
        text = UIText;
        textColor = colorOfText;
    }

    public static void NotifyMessage(string message, Color? messageColor = null)
    {
        text.gameObject.SetActive(true);

        text.SetText(message);
        text.color = messageColor ?? Color.red; //Is message color specified? Go red otherwise

        text.StartCoroutine(FadeText());
    }

    public static void NotifyEvent(RTSEvent rtsEvent)
    {

    }

    private static IEnumerator FadeText()
    {
        while(!text.color.IsZeroAlpha())
        {
            text.color = text.color.FadeAlpha(fadeIncrement);
            yield return null;
        }
    }
}
