using UnityEngine;

public enum Team
{
    Red,
    Blue
}

public enum ArmorClass
{
    Light,
    Normal,
    Heavy,
    Bio,
    Air,
    Building
}

[RequireComponent(typeof(Health))]
public class RTSObject : MonoBehaviour
{
    public string name;
    public Sprite uiSprite;

    [TextArea]
    public string unitDescription;

    [HideInInspector]
    public Health health;
    public int cost;
    public float speed;
    public float range;

    public ArmorClass armorClass = ArmorClass.Normal;
    public float productionDuration;
    public Team team;

    private int playerIndex;

    private void Start()
    {
        health = GetComponent<Health>();
    }
}
