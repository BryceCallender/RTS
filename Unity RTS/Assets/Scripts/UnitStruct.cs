using UnityEngine;

namespace Unit
{
    public struct UnitStruct
    {
        public GameObject unit;
        public string name;
        public int cost;
        public Sprite sprite;
    }

    public class UnitName
    {
        public static string GetNameOfUnit(GameObject unit)
        {
            string[] name = unit.gameObject.name.Split('_');
            return name[1];
        } 
    }
}
