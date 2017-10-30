using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
    public int capacityMax;
    public int currentCapacity;
    public Text capacityText;

    void Start()
    {
        capacityMax = 50;
        currentCapacity = 0;
        capacityText.text = currentCapacity + "/" + capacityMax;
    }
}
