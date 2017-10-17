using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    public int currency;
    public Text resourcePanel;
    public Text timeText;

    private void Update()
    {
        timeText.text = Time.realtimeSinceStartup.ToString();
        resourcePanel.text = "Resource:" + currency;
    }
}
