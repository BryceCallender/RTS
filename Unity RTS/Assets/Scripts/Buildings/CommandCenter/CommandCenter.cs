using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnergySystem))]
public class CommandCenter : TrainingBuilding
{
    public int capacityIncrease;

    private EnergySystem energySystem;

    protected override void Start()
    {
        base.Start();

        energySystem = GetComponent<EnergySystem>();
    }
}
