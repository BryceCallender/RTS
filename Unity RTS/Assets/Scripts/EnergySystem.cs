using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public float energy;
    public float maxEnergy;

    public bool isShield;

    private float energyRegenTimer = 0f;
    public bool HasEnergy => energy > 0;

    private void Start()
    {
        energy = 0;
    }

    private void Update()
    {
        RegenEnergy();

        if(energy <= 0)
        {
            energy = 0;
        }
    }

    private void RegenEnergy()
    {
        //Incase we go over the amount 
        if(energy >= maxEnergy)
        {
            energy = maxEnergy;
            energyRegenTimer = 0f;
            return;
        }

        energyRegenTimer += Time.deltaTime;
        if (energyRegenTimer > 1f)
        {
            energy += 0.5625f;
            energyRegenTimer = 0f;
        }
    }

    public void ConsumeEnergy(float amount)
    {
        energy -= amount;
    }
}
