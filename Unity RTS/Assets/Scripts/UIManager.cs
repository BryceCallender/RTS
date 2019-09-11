using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject[] panels;
	public GameObject selectedObjectPanel;
	public List<Sprite> images = new List<Sprite>();

	private Sprite spriteToShow;
	private Image image;
	private static UIManager instance;

	public static UIManager Instance
	{
		get
		{
			return instance;
		}
	}

	private void Awake()
	{
		instance = this;
		image = selectedObjectPanel.GetComponent<Image>();
	}

	public void SetPhoto(string name)
	{
		spriteToShow = FindSprite(name);
		image.sprite = spriteToShow;
	}

	public Sprite FindSprite(string name)
	{
		string[] nameSplit = name.Split('_');
		//Gets the thing between the _ like in Unit_Tank_Blue
		string actualTargetedName = nameSplit[1];
		Sprite foundSprite = null;

		for (int i = 0; i < images.Count; i++)
		{
			if(actualTargetedName.Equals(images[i].name))
			{
				foundSprite = images[i];
			}
		}

		return foundSprite;
	}

	public void SetAllPanelsOff()
	{
		foreach(GameObject panel in panels)
		{
			panel.gameObject.SetActive(false);
		}
	}

	public void SetAllOffBut(GameObject panel)
	{
		for (int i = 0; i < panels.Length; i++)
		{
			if(!panels[i].Equals(panel))
			{
				panels[i].gameObject.SetActive(false);
			}
		}
	}

	public void SetOnPanel(GameObject panel)
	{
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].Equals(panel))
			{
				panels[i].gameObject.SetActive(true);
			}
		}
	}

	public void SetOffPanel(GameObject panel)
	{
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].Equals(panel))
			{
				panels[i].gameObject.SetActive(false);
			}
		}
	}

	public bool MoreThanOnePanel()
	{
		int count = 0;
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].activeSelf)
			{
				count++;
			}
		}

		return count == 1;
	}
}
