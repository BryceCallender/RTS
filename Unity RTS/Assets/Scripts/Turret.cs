using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
    public List<GameObject> targets;
    public GameObject bullet;
    public Transform turretEnd;
    public Transform turretRotator;
    public int range = 15;
    public int team;
    public int layerTeam;

	// Use this for initialization
	void Start () 
    {
        layerTeam = this.gameObject.layer;
        switch(layerTeam)
        {
            case 8: team = 0;
                break;
            case 9: team = 1;
                break;
        }
        targets = new List<GameObject>();
	}
	
}
