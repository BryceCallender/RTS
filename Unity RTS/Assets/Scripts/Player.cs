using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
    public int currency;
    public int capacityMax;
    public int currentCapacity;
    [SerializeField]
    private Text capacityText;

    [SerializeField]
    private bool isAI;

    private void Start()
    {
        capacityMax = 50;
        currentCapacity = 0;
        capacityText.text = currentCapacity + "/" + capacityMax;
    }
}
