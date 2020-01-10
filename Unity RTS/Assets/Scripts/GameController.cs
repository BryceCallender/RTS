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

	public static bool hitEscape;
	public bool isPaused;
	public GameObject pauseMenuUI;

	[SerializeField]
	//private Player[] players;
	private Player player;

    private Timer time;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        timeText = timeText.GetComponent<TextMeshProUGUI>();
		time = GetComponent<Timer>();
		timeText.gameObject.SetActive(true);
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
    }

    

	public bool PressedEscape()
	{
		hitEscape = Input.GetKeyDown(KeyCode.Escape);
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

	//public Player GetPlayer(int player)
	//{
	//	return players[player];
	//}

	public Player GetPlayer()
	{
		return player;
	}
}
