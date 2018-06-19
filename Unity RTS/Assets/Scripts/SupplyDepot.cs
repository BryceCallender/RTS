using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyDepot : Building
{
    [SerializeField] 
    private int capacityAddOn;

    private GameController gameController;
    
    private void Start()
    {
        gameController = GameController.Instance;
        AddCapacity();
    }

    private void AddCapacity()
    {
        var playerReference = gameController.grabPlayer("Player");
        playerReference.GetComponent<Player>().capacityMax += capacityAddOn;
    }

    private void OnDestroy()
    {
        var playerReference = gameController.grabPlayer("Player");
        playerReference.GetComponent<Player>().capacityMax -= capacityAddOn;
    }
}
