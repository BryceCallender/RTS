using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBuilding : Building
{
    public int mineralCount;

    public GameObject leftMineralResource;
    public GameObject rightMineralResource;

    protected override void Start()
    {
        base.Start();

        mineralCount = 0;
        FillSupplyResources(false);
    }

    protected override void Update()
    {
        base.Update();

        if(mineralCount > 0)
        {
            FillSupplyResources(true);
        }
    }


    public void FillSupplyResources(bool enabled)
    {
        leftMineralResource.SetActive(enabled);
        rightMineralResource.SetActive(enabled);
    }
}
