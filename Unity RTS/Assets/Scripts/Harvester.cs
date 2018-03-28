using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unit;

public class Harvester : MonoBehaviour, IImageable
{
    [Header("Harvester Stats")]
    public int health = 100;
    public int resourceAmount = 0;
    public int maxResourceToCollect = 100;
    public int cost = 5;
    public int capacity = 1;
    public int team = (int)Team.BLUE;

    //Harvester Building Capabilites and what building to build if we want one
    public List<GameObject> buildableBuildings;
    private GameObject buildingToBuild;

    //Ui elements for the harvester to control
    [Header("UI Elements")]
	public GameObject harvesterPanel;
    public Button factoryButton;
    public Button supplyButton;
    public Slider healthBar;

    // Resources and collector for it to go to 
    [Header("Harvester Resource/Collector")]
    public Resource[] resources;
    public Resource nearestResource;
    public Transform resourceCollector;

    //Harvester Abilities Harvest amount and cooldowns
    private float harvestTime = 2.0f;
    private float harvestCoolDown = 3.0f;
    private int harvestAmount = 10;
    [SerializeField]
    private GameObject crystal;

    //Booleans
    private bool isTargetingResource = false;
    private bool isFull = false;
    private bool isTurnedIn = false;
    private bool isAtResource = false;

    //Timers
    private float timer = 0;
    private float timeToWait = 1.0f;

    //Other scripts and RayCast info holders
    private RaycastHit hitInfo;
    private UnitSelected unitSelected;
    private NavMeshAgent agent;
    [Header("Other Scripts")]
    public HyperbitProjectileScript hyperProjectileScript;
	public UIManager uiManager;
    static GameController gameController;
    static BuildingPlacement buildingPlacement;




    private void Start()
    {
		agent = GetComponent<NavMeshAgent>();
        resources = FindObjectsOfType<Resource>();
		uiManager = FindObjectOfType<UIManager>();

        gameController = FindObjectOfType<GameController>();
        buildingPlacement = FindObjectOfType<BuildingPlacement>();

        unitSelected = GetComponent<UnitSelected>();

        agent.stoppingDistance = 3;

        factoryButton.onClick.AddListener(BuildFactory);
        supplyButton.onClick.AddListener(BuildSupply);

        healthBar.gameObject.SetActive(false);
        healthBar.maxValue = health;
        healthBar.value = health;

        crystal.gameObject.SetActive(false);

        if(resourceCollector == null)
        {
            resourceCollector = transform.Find("SupplyBuilding");
        }

    }

    private void Update()
    {
        FindResource();

		//We have found a resource
        if(nearestResource != null)
        {
            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray2, out hitInfo, Mathf.Infinity))
            {
                WhereToGo();
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !isFull && isAtResource)
            {
                CollectResource();
            }

            if (resourceAmount >= maxResourceToCollect || nearestResource.resourceLeft == 0)
			{
                isFull = true;
                crystal.gameObject.SetActive(true);
                if(resourceCollector != null)
                {
                    agent.destination = resourceCollector.position;
                }

                timer += Time.deltaTime;

                if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                {
                    if(resourceCollector != null)
                    {
                        isTurnedIn = true;
                        TurnInResource();
                        timer = 0;
                    }
                }
                isTurnedIn = false;
                isFull = false;
			}

        }
		//Need to find a resource
        else
        {
            //raycast to check where user clicked mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //TODO: Need to make harvester more effective with moving 
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                //If right clicked and unit is selected go to resource
                WhereToGo();
                if (nearestResource == null && resourceAmount > 0)
                {
                    if(resourceCollector != null)
                    {
                        agent.destination = resourceCollector.position;
                    }

                    timer += Time.deltaTime;
                    if (agent.remainingDistance <= agent.stoppingDistance && !isTurnedIn && timer >= timeToWait)
                    {
                        Debug.Log("We should turn in the stoff");
                        TurnInResource();
                        timer = 0;
                        isTurnedIn = true;
                    }
                    isTurnedIn = false;
                    isFull = false;
                }
            }
        }

        if (unitSelected.isFirst && unitSelected.selected)
		{
            
			ShowImage();
		}
	}

    /*
     * This function makes the harvester wait a bit to collect each resource 
     * and as it collects it takes from the resources resovoir 
     */
    public void CollectResource()
    {
        harvestCoolDown -= Time.deltaTime;
        if(harvestCoolDown < 0)
        {
            harvestCoolDown = harvestTime;
            resourceAmount += harvestAmount;
            nearestResource.resourceLeft -= harvestAmount;
        }
    }

	/*
     * This function makes the harvester turn in what it has and adds 
     * to the currency of the user 
     */
	public void TurnInResource()
    {
        gameController.currency += resourceAmount;
        resourceAmount = 0;
        crystal.gameObject.SetActive(false);
    }

	/*
     * This function looks at all the resources on map and picks closest one
     */
	public void FindResource()
    {
        float distance = Mathf.Infinity;

        foreach (Resource resource in resources)
        {
            if(resource != null)
            {
				float resourceDistance = Vector3.Distance(this.transform.position, resource.transform.position);
				if (nearestResource == null || resourceDistance < distance)
				{
					//nearestResource = resource;
					//distance = resourceDistance;
				}
            }

        }

        if (nearestResource == null)
        {
            //return; 
        }
    }

    /*
     * Tells the harvester where to go based on if it is selected and if 
     * the user has right clicked down on  the mouse. If it hits a resource the
     * harvester will go to the resource and start collecting and if its just
     * a random point on the map the harvester will go to that specific point
     */
    public void WhereToGo()
    {
		if (Input.GetMouseButtonDown(1) && unitSelected.selected)
		{
			if (hitInfo.transform.gameObject.CompareTag("Resource"))
			{
				GameObject resource = hitInfo.transform.gameObject;
				nearestResource = resource.GetComponent<Resource>();
				agent.destination = nearestResource.transform.position;
                isAtResource = true;
			}
			else
			{
				agent.destination = hitInfo.point;
                isAtResource = false;
			}

		}
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.value -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

	public void ShowImage()
	{
		UIManager.Instance.SetPhoto(this.gameObject.name);
	}

	private void OnTriggerEnter(Collider collision)
    {
        hyperProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

        if (hyperProjectileScript.team.Equals(team))
        {
            return;
        }

        if (!hyperProjectileScript.owner.Contains("Blue")
            && !hyperProjectileScript.team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(9, 10, false);
            if (collision.gameObject.tag.Contains("Laser")
                && collision.gameObject.layer == 10)
            {
                TakeDamage(GameController.LASER_DAMAGE);
            }
            else if (collision.gameObject.tag.Contains("Cluster")
                     && collision.gameObject.layer == 10)
            {
                TakeDamage(GameController.CLUSTER_BOMB_DAMAGE);
            }
        }
    }

    private void BuildFactory()
    {
        buildingPlacement.SetBuilding(buildableBuildings.Find(x => x.gameObject.name.Contains("Factory")));
        ResetBuildingPlacement();
        Debug.Log("Building Factory");
    }

    private void BuildSupply()
    {
        buildingPlacement.SetBuilding(buildableBuildings.Find(x => x.gameObject.name.Contains("Supply")));
        ResetBuildingPlacement();
        Debug.Log("Building Supply");
    }

    private GameObject FindBuilding(string buildingName)
    {
        GameObject building;
        building = buildableBuildings.Find(x => x.gameObject.name.Contains(buildingName));
        if(building == null)
        {
            return null;
        }
        return building;
    }

    private void ResetBuildingPlacement()
    {
        buildingPlacement.hasPlaced = false;
        //buildingPlacement.collisions.Clear();
    }


}
