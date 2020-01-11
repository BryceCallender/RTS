using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public int hour;
	public int minutes;
	public int seconds;
	public float secondHelper;

	private void Start()
	{
		hour = 0;
		minutes = 0;
		seconds = 0;
		secondHelper = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		secondHelper += Time.deltaTime;
		if(secondHelper > 1)
		{
			seconds++;
			secondHelper = 0;
		}

		if(seconds == 60)
		{
			seconds = 0;
			minutes++;
		}

		if(minutes == 60)
		{
			hour++;
			minutes = 0;
		}
	}

	public string DisplayTime()
	{
        string time = "";

        if(hour > 0)
        {
            time += hour.ToString() + ":";
        }


        time += minutes.ToString() + ":";

        time += seconds.ToString();

		return time;
	}
}
