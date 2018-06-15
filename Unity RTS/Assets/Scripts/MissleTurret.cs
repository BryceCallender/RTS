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

public class MissleTurret : Turret
{
    public List<MissleHead> missleRepresentations; // its did a missle fire and its transform name better?????

	// Use this for initialization
	void Start()
	{
		base.Start();
		missleRepresentations = new List<MissleHead>();
	}
}
