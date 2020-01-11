using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFacility : Building
{
    public int gasReceived;

    public Transform pump;

    protected override void Update()
    {
        base.Update();

        pump.position = new Vector3(pump.position.x, Mathf.PingPong(Time.time, 0.85f), pump.position.z);
    }
}
