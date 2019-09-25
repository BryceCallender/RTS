﻿using UnityEngine;

public class Galaxy : FlyingUnit
{
    public GameObject Thruster;
    private ParticleSystem thruster;

    private void Start()
    {
        base.Start();
        thruster = Thruster.GetComponentInChildren<ParticleSystem>();
        thruster.Stop();
    }


    public void ActivateThrusters()
    {
        thruster.Play();
    }

    public void DeActivateThrusters()
    {
        thruster.Stop();
    }

    public int RandomizeTurretSelection()
    {
        int random;
        random = Random.Range(0,turrets.Length);
        return random;
    }
}
