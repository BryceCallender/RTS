using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    public int currency;
    public Text resourcePanel;
    public Text timeText;

    //Minerals
    public Text mineralErrorText;
    public Color mineralErrorColor;
   
    //Gas
    public Text gasErrorText;
    public Color gasErrorColor;

    //Building
    public Text buildingErrorText;
    public Color buildingErrorColor;

    public float colorFadeTime = 0.5f;

    private void Start()
    {
        mineralErrorColor = mineralErrorText.color;
        gasErrorColor = mineralErrorColor;
        buildingErrorColor = mineralErrorColor;
    }

    private void Update()
    {
        timeText.text =  Time.realtimeSinceStartup.ToString();
        resourcePanel.text = "Resource:" + currency;
        if(mineralErrorText.gameObject.activeSelf)
        {
            FadeAlpha(colorFadeTime, ref mineralErrorText, ref mineralErrorColor);
        }
        if(IsZeroAlpha(mineralErrorColor))
        {
            SetAlphaBack(ref mineralErrorText,ref mineralErrorColor);
        }
    }

    public void FadeAlpha(float fadeTime,ref Text errorText, ref Color errorColor)
    {
        if(errorColor.a >= 0)
        {
            fadeTime -= Time.deltaTime;
            errorColor.a -= 0.02f;
            errorText.color = errorColor;
        }
        else
        {
            errorColor.a = 0;
        }
    }

    public bool IsZeroAlpha(Color errorColor)
    {
        if(errorColor.a.Equals(0))
        {
            return true;
        }
        return false;
    }

    public void SetAlphaBack(ref Text errorText,ref Color errorColor)
    {
        errorColor.a = 1.0f;
        errorText.gameObject.SetActive(false);
        errorText.color = errorColor;
        colorFadeTime = 0.5f;
    }

}
