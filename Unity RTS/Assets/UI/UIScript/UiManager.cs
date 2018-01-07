using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UiManager : MonoBehaviour
{
	public AudioMixer audioMix;
	public Dropdown resolutionDropDown;
	Resolution[] resolutions;
	

	private void Start()
	{
		resolutions = Screen.resolutions;
		resolutionDropDown.ClearOptions();

		List<string> res = new List<string>();

		int currentResolutionIndex = 0;
		
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			res.Add(option);

			if(resolutions[i].width == Screen.currentResolution.width &&
			   resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropDown.AddOptions(res);
		resolutionDropDown.value = currentResolutionIndex;
		resolutionDropDown.RefreshShownValue();
	}

	public void SetMasterVolume(float volume)
	{
		audioMix.SetFloat("volume", volume);
	}

	public void SetQuality(int quality)
	{
		QualitySettings.SetQualityLevel(quality);
	}

	public void SetFullScreen(bool isFullScreen)
	{
		Screen.fullScreen = isFullScreen;
	}

	public void SetResolution(int resolution)
	{
		Screen.SetResolution(resolutions[resolution].width, resolutions[resolution].height, Screen.fullScreen);
	}
}
