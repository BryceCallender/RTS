using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

[System.Serializable]
public struct MissleHead
{
    public bool hasBeenFired;
    public Transform turretEnds;
}

public class MissleTurret : Building
{
	public List<GameObject> targets;
    public List<MissleHead> missleRepresentations;
	public GameObject bullet;
	public Transform turretRotator;
	public int range = 15;
	public int layerTeam;


	// Use this for initialization
	void Start()
	{
		//Since its turrents the only targets is the units so layers
		layerTeam = this.gameObject.layer;
		switch (layerTeam)
		{
			case 8:
                team = (int)Team.BLUE;
				break;
			case 9:
                team = (int)Team.RED;
				break;
		}
		targets = new List<GameObject>();
	}
}
