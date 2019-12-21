using UnityEngine;

public class FlyingUnit : Unit
{
    public float flyHeight;

    private GameObject selectionIndicator;

    protected override void Start()
    {
        base.Start();
        
        unitsUnitIsStrongAgainst = UnitDamageStrength.NormalArmor;

        //Make the unit on start be at the respective height to be flying
        agent.baseOffset = flyHeight;

        selectionIndicator = GetComponent<UnitSelected>().selectionIndicator;

        selectionIndicator.transform.position = new Vector3(selectionIndicator.transform.position.x, 
                                                            selectionIndicator.transform.position.y + flyHeight, 
                                                            selectionIndicator.transform.position.z);
    }

    //Define how to fly and how to move heights when going over height disturbances
    protected override void Update()
    {
        base.Update();
        
        //Raycast down to see how high we are
        //If we are not flyHeight distance from the group lerp until we are :)
        //Update the baseOffset too?
    }

    protected void RandomizeTurretSelection()
    {
        int random;
        random = Random.Range(0, turrets.Length);
        turretEnd = turrets[random];
    }

    protected override void Fire()
    {
        RandomizeTurretSelection();
        base.Fire();
    }
}
