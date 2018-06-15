﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Unit;

[RequireComponent(typeof(GameController))]
public class CommandBuilding : Building 
{
	#region variables
    public Text commandBuildText;
    public Text amountText;
    public GameObject harvesterGameObject;
    public GameObject harvesterPanel;
    public Slider harvesterSpawnerSlider;

    private GameObject harvesterSpawn;

    static GameController gameController;

    private Queue<GameObject> harvesterQueue;
    private float spawnTimer = 5.0f;
    private float spawnTimerCoolDown = 5.0f;

    private RaycastHit hitInfo;
    private bool clickedBuilding;
    private Transform rallyLocation;
	private UIManager uiManager;

	#endregion

	// Use this for initialization
	void Start ()
	{
		requirementStructures.building = gameObject;
		requirementStructures.requirementList = new List<GameObject>();
		
        gameController = FindObjectOfType<GameController>();
		uiManager = gameController.GetComponent<UIManager>();
        //commandBuildText = GameObject.Find("HarvesterStats").GetComponent<Text>();
        commandBuildText.text = "Harvester Cost: 10 resources";
        harvesterSpawn = GameObject.Find("HarvesterSpawner");
        harvesterQueue = new Queue<GameObject>();
        harvesterSpawnerSlider.maxValue = spawnTimer;
        harvesterSpawnerSlider.value = spawnTimer;
        rallyLocation = harvesterSpawn.transform;
	}
	
	// Update is called once per frame
	private void Update ()
	{
        //Call to see if we clicked the building or not to see if we can even 
        //make units or not 
        ClickedBuilding();

        //Only perform this if we have more than 0 in the queue 
		if (harvesterQueue.Count > 0)
		{
            //Show the green timer system 
			harvesterSpawnerSlider.gameObject.SetActive(true);
			spawnTimer -= Time.deltaTime;
			harvesterSpawnerSlider.value = spawnTimer;

            //Once timer hits 0 or lower we will spawn unit reset counter for 
            //next one and dequeue from the queue changing the amount being made
			if (spawnTimer < 0)
			{
				spawnTimer = spawnTimerCoolDown;
				Instantiate(harvesterGameObject, harvesterSpawn.transform.position, harvesterSpawn.transform.rotation);
                //Unit layer
                harvesterGameObject.layer = 8;
				Debug.Log(harvesterQueue.Dequeue());
				amountText.text = "x" + harvesterQueue.Count;
			}
		}
		else
		{
            //Dont show the animation since we have none in queue
			harvesterSpawnerSlider.gameObject.SetActive(false);
		}
	}

    public void ClickedBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.name.Contains("Command") && Input.GetMouseButtonDown(0))
            {
                clickedBuilding = true;
                harvesterPanel.SetActive(true);
				uiManager.SetAllOffBut(harvesterPanel);
			}
            //If panel is on then lets conisder if they want to have ui go away 
            //if we click away
            if(harvesterPanel.activeSelf)
            {
                if(!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(1))
                {
                    clickedBuilding = false;
                    harvesterPanel.SetActive(false);
                }
            }
        }
    }

    /*
     * Based upon button input we will add a harvester to the queue and take 
     * money away.
     */
    public void AddHarvesterToQueue()
    {
        if(gameController.currency >= harvesterGameObject.GetComponent<Harvester>().cost)
        {
            harvesterQueue.Enqueue(harvesterGameObject);
            gameController.currency -= harvesterGameObject.GetComponent<Harvester>().cost;
            Debug.Log(harvesterQueue.Count);
            amountText.text = "x" + harvesterQueue.Count;
        }
		else
		{
			gameController.mineralErrorText.gameObject.SetActive(true);
		}
    }

    /*
     * Based upon button input we will delete a harvester from the queue and
     * give the appropiate amount of money back for taking off the queue.
     */
	public void DeleteHarvesterFromQueue()
	{
        if (harvesterQueue.Count > 0)
		{
            harvesterQueue.Dequeue();
            gameController.currency += harvesterGameObject.GetComponent<Harvester>().cost;
			Debug.Log(harvesterQueue.Count);
			amountText.text = "x" + harvesterQueue.Count;
		}
	}

    /*
     * Like all rts each building has a rally point for when units spawn and 
     * this allows the user to move the rallylocation by rightclicking
     * TODO: add animation or a flag to show where it is.
     */
    public void MoveSpawnLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
		{
            if(clickedBuilding && Input.GetMouseButtonDown(1))
            {
                rallyLocation.transform.position = hitInfo.point;
                harvesterGameObject.GetComponent<NavMeshAgent>().destination = rallyLocation.transform.position;
            }
		}
    }
}
