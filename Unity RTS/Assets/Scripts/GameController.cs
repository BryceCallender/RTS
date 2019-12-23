using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public Text resourcePanel;
    public TextMeshProUGUI timeText;

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

	public static bool hitEscape;
	public bool isPaused;
	public GameObject pauseMenuUI;

	[SerializeField]
	private GameObject[] players;

    private Timer time;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mineralErrorColor = mineralErrorText.color;
        gasErrorColor = mineralErrorColor;
        buildingErrorColor = mineralErrorColor;
        timeText = timeText.GetComponent<TextMeshProUGUI>();
		time = GetComponent<Timer>();
    }

    private void Update()
    {
		if(PressedEscape())
		{
			if(isPaused)
			{
				UnPauseGame();
			}
			else
			{
				PauseGame();
			}
		}

		timeText.SetText(time.DisplayTime());
        //resourcePanel.text = "Resource:" + currency;

        if(mineralErrorText.gameObject.activeSelf)
        {
            FadeAlpha(colorFadeTime, ref mineralErrorText, ref mineralErrorColor);
        }
        if(IsZeroAlpha(mineralErrorColor))
        {
            SetAlphaBack(ref mineralErrorText, ref mineralErrorColor);
        }

        if(buildingErrorText.gameObject.activeSelf)
        {
            FadeAlpha(colorFadeTime, ref buildingErrorText, ref buildingErrorColor);
        }
        if(IsZeroAlpha(buildingErrorColor))
        {
            SetAlphaBack(ref buildingErrorText, ref buildingErrorColor);
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
        return errorColor.a == 0;
    }

    public void SetAlphaBack(ref Text errorText, ref Color errorColor)
    {
        errorColor.a = 1.0f;
        errorText.gameObject.SetActive(false);
        errorText.color = errorColor;
        colorFadeTime = 0.5f;
    }

	public bool PressedEscape()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			hitEscape = true;
		}
		else
		{
			hitEscape = false;
		}
		return hitEscape;
	}

	public void PauseGame()
	{
		pauseMenuUI.gameObject.SetActive(true);
        isPaused = true;
		Time.timeScale = 0f;
	}

	public void UnPauseGame()
	{
		pauseMenuUI.gameObject.SetActive(false);
		isPaused = false;
		Time.timeScale = 1f;
	}

	public GameObject grabPlayer(string identifier)
	{
		for (int i = 0; i < players.Length; i++)
		{
			if (players[i].name.Equals(identifier))
			{
				return players[i];
			}
		}
		return null;
	}
}
