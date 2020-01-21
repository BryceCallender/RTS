using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : Building
{
    public List<Upgrade> unitUpgrades;
    public List<Upgrade> buildingUpgrades;


    public void ResearchUpgrade(Upgrade upgrade)
    {
        if(unitUpgrades.Contains(upgrade) || buildingUpgrades.Contains(upgrade))
        {
            ActivateResearchEffect();
        }
        
    }

    private void ActivateResearchEffect()
    {

    }
}
