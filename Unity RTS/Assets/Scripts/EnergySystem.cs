using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    public float energy;
    public float maxEnergy;

    private float energyRegenTimer = 0f;

    private void Start()
    {
        energy = 0;
    }

    private void Update()
    {
        RegenEnergy();
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

    private void ConsumeEnergy(float amount)
    {
        energy -= amount;
    }
}
