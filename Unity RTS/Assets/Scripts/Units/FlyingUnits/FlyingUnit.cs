using UnityEngine;

public class FlyingUnit : Unit
{
    public float flyHeight;

    private void Start()
    {
        unitsUnitIsStrongAgainst = UnitDamageStrength.Ground;

        //Make the unit on start be at the respective height to be flying
        transform.position = new Vector3(transform.position.x, flyHeight, transform.position.z);
    }

    //Define how to fly and how to move heights when going over height disturbances
    protected void Update()
    {
        
    }
}
