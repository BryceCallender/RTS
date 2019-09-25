using UnityEngine;

public class FlyingUnit : Unit
{
    public float flyHeight;

    protected void Start()
    {
        base.Start();
        
        unitsUnitIsStrongAgainst = UnitDamageStrength.Ground;

        //Make the unit on start be at the respective height to be flying
        agent.baseOffset = flyHeight;
    }

    //Define how to fly and how to move heights when going over height disturbances
    protected void Update()
    {
        base.Update();
        
        //Raycast down to see how high we are
        //If we are not flyHeight distance from the group lerp until we are :)
        //Update the baseOffset too?
    }
}
