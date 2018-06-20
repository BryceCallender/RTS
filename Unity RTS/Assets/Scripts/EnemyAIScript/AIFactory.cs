﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class AIFactory : Building 
{
    [Header("Factory Attributes")]
    public float health;
    public int resourceCost;
    private GameObject unitGameObject; 

    private float spawnTimer = 5.0f;
    private float spawnTimerCoolDown = 5.0f;

    [Header("Factory Queue's and Buildable Units")]
    public Queue<UnitStruct> unitQueue;
    [SerializeField]
    private List<GameObject> buildableUnits;

    [Header("Factory Spawner")]
    [SerializeField]
    private Transform unitSpawn;

    [Header("Booleans and Controllers")]
    private Transform rallyLocation;
    private HyperbitProjectileScript hyperProjectileScript;

    // Use this for initialization
    void Start()
    {
        health = 300;
        resourceCost = 50;
        unitQueue = new Queue<UnitStruct>();
        AI.Instance.buildings.Add("Factory", resourceCost);
    }

    // Update is called once per frame
    void Update()
    {
        if(unitQueue.Count > 0)
        {
            //Show the green timer system 
            spawnTimer -= Time.deltaTime;

            //Once timer hits 0 or lower we will spawn unit reset counter for 
            //next one and dequeue from the queue changing the amount being made
            if (spawnTimer < 0)
            {
                spawnTimer = spawnTimerCoolDown;
                Instantiate(unitQueue.Peek().unit, unitSpawn.transform.position, unitSpawn.transform.rotation);
                unitQueue.Dequeue();
            }
        }
    }

    public void QueueUnit()
    {
        UnitStruct unitToQueue;
        float weight = Random.Range(0, 1);
        if(weight > 0.50)
        {
            unitToQueue.unit = AI.Instance.FindUnit("Tank");
            unitToQueue.cost = AI.Instance.units["Tank"];
            unitToQueue.name = unitToQueue.unit.name;
            //AI.Instance.globalQueue.Enqueue(unitToQueue);
            
        }
        else if(weight > 0.25 && weight < 0.50)
        {
            unitToQueue.unit = AI.Instance.FindUnit("Galaxy");
            unitToQueue.cost = AI.Instance.units["Galaxy"];
            unitToQueue.name = unitToQueue.unit.name;
            //AI.Instance.globalQueue.Enqueue(unitToQueue);
        }
        else
        {
            unitToQueue.unit = AI.Instance.FindUnit("Laser");
            unitToQueue.cost = AI.Instance.units["Laser"];
            unitToQueue.name = unitToQueue.unit.name;
           // AI.Instance.globalQueue.Enqueue(unitToQueue);
        }

       // unitQueue.Enqueue(unitToQueue);
    }
}
