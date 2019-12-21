using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitManager: MonoBehaviour
{
    public static List<GameObject> units;
    public List<GameObject> allUnits;

    private void Start()
    {
        units = allUnits;
    }

    public static GameObject GetGameObjectFromUnit(Unit unit)
    {
        return units.Where(gameObj => gameObj.GetComponent<RTSObject>().name == unit.name).FirstOrDefault();
    }
}
