using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unit;

[RequireComponent(typeof(GameController))]
public class Factory : MonoBehaviour
{
    [Header("Factory Attributes")]
    public float health;
    private int team = (int)Team.BLUE;
    private GameObject unitGameObject;

    public Sprite tank;
    public Sprite galaxy;

    private float spawnTimer = 5.0f;
    private float spawnTimerCoolDown = 5.0f;

    [Header("Factory Queue's and Buidlable Units")]
    private Queue<UnitStruct> unitQueue;
    private List<Sprite> unitSpriteList;
    [SerializeField]
    private List<GameObject> buildableUnits;


    [Header("Factory Spawner")]
    [SerializeField]
    private Transform unitSpawn;

    [Header("Factory UI Elements")]
    [SerializeField]
    private GameObject factoryPanel;
    [SerializeField]
    private GameObject factoryQueuePanel;
    [SerializeField]
    private Text unitStats;
    [SerializeField]
    private TextMeshProUGUI nextUnitNameText;
    public Slider unitSpawnSlider;
    private Image unitSliderImage;


    [Header("Booleans and Controllers")]
    static GameController gameController;
    private RaycastHit hitInfo;
    private bool clickedBuilding;
    private Transform rallyLocation;
    private bool isTank;
    [SerializeField]
    private bool isSelected;
    private bool isShowingNextUnit;
    private string selectedFactoryName;
	private UIManager uiManager;

    [Header("Unit Buttons")]
    [SerializeField]
    private Button tankButton;
    [SerializeField]
    private Button galaxyButton;


    // Use this for initialization
    void Start()
    {
        health = 300;
        isSelected = false;
        gameController = FindObjectOfType<GameController>();
		uiManager = gameController.GetComponent<UIManager>();
        unitQueue = new Queue<UnitStruct>();
        unitSpriteList = new List<Sprite>();
        unitSliderImage = unitSpawnSlider.GetComponentInChildren<Image>();
        factoryPanel.SetActive(false);
        factoryQueuePanel.SetActive(false);
        tankButton.onClick.AddListener(AddTankToQueue);
        galaxyButton.onClick.AddListener(AddGalaxyToQueue);
        unitSpawnSlider.maxValue = spawnTimer;
        unitSpawnSlider.value = spawnTimer;
        nextUnitNameText.gameObject.SetActive(false);
        isShowingNextUnit = false;
        //nextUnitInQueue.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Call to see if we clicked the building or not to see if we can even 
        //make units or not 
        ClickedBuilding();

        if(unitQueue.Count > 0)
        {
			//Show the green timer system 
			unitSpawnSlider.gameObject.SetActive(true);
            factoryQueuePanel.SetActive(true);
			spawnTimer -= Time.deltaTime;
            unitSpawnSlider.value = spawnTimer;
            unitSliderImage.sprite = unitSpriteList[0];

            //If we have more than one in our queue lets tell the player
            //what is next in the queue!
            if(HasMoreThanOneInQueue(unitQueue) && !isShowingNextUnit)
            {
                nextUnitNameText.gameObject.SetActive(true);
                nextUnitNameText.text = "Next Unit in Queue: ";
                nextUnitNameText.text += GetNextInQueue(unitQueue);
                isShowingNextUnit = true;
            }

            if(unitQueue.Count.Equals(1))
            {
                nextUnitNameText.gameObject.SetActive(false);
            }

			//Once timer hits 0 or lower we will spawn unit reset counter for 
			//next one and dequeue from the queue changing the amount being made
			if (spawnTimer < 0)
			{
				spawnTimer = spawnTimerCoolDown;
                Instantiate(unitQueue.Peek().unit, unitSpawn.transform.position, unitSpawn.transform.rotation);
                unitQueue.Dequeue();
                unitSpriteList.RemoveAt(0);
                //Reset to change the next name of the next unit
                isShowingNextUnit = false;
			}
        }
        else
        {
			//Dont show the animation since we have none in queue
            unitSpawnSlider.gameObject.SetActive(false);
            factoryQueuePanel.SetActive(false);
        }    

    }

    public void AddTankToQueue()
    {
        if(isSelected)
        {
            SetUnit(buildableUnits.Find(x => x.gameObject.name.Contains("Tank")));
            int cost = unitGameObject.GetComponent<Tank>().cost;
            if (gameController.currency >= cost)
            {
                UnitStruct unitToQueue;
                unitToQueue.unit = unitGameObject;
                unitToQueue.cost = cost;
                unitToQueue.name = UnitName.GetNameOfUnit(unitGameObject);

                unitQueue.Enqueue(unitToQueue);
                gameController.currency -= cost;
                Debug.Log(unitQueue.Count);
            }
            else
            {
                //Sets the error message
                gameController.mineralErrorText.gameObject.SetActive(true);
            }
        }
    }

    public void AddGalaxyToQueue()
    {
        if(isSelected)
        {
            SetUnit(buildableUnits.Find(x => x.gameObject.name.Contains("Galaxy")));
            int cost = unitGameObject.GetComponent<Galaxy>().cost;
            if (gameController.currency >= cost)
            {
                UnitStruct unitToQueue;
                unitToQueue.unit = unitGameObject;
                unitToQueue.cost = cost;
                unitToQueue.name = UnitName.GetNameOfUnit(unitGameObject);

                unitQueue.Enqueue(unitToQueue);
                gameController.currency -= cost;
                Debug.Log(unitQueue.Count);
            }
            else
            {
                //Sets the error message
                gameController.mineralErrorText.gameObject.SetActive(true);
            }
        }
    }

    public void DeleteTankFromQueue()
    {
        if (isSelected)
        {
            if (unitQueue.Count > 0)
            {
                unitQueue.Dequeue();
                gameController.currency += unitGameObject.GetComponent<Tank>().cost;
                Debug.Log(unitQueue.Count);
            }
        }
    }

	public void DeleteGalaxyFromQueue()
	{
        if (isSelected)
        {
            if (unitQueue.Count > 0)
            {
                unitQueue.Dequeue();
                gameController.currency += unitGameObject.GetComponent<Galaxy>().cost;
                Debug.Log(unitQueue.Count);
            }
        }
	}

    public void SetUnit(GameObject obj)
    {
        unitGameObject = obj;
        if(unitGameObject.GetComponent<Tank>())
        {
            isTank = true;
            unitSpriteList.Add(tank);
            //unitQueue.Enqueue(unitGameObject);
        }
        else
        {
            isTank = false; 
            unitSpriteList.Add(galaxy);
            //unitQueue.Enqueue(unitGameObject);
        }
    }

	public void ClickedBuilding()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
		{
            if (!EventSystem.current.IsPointerOverGameObject() && 
                hitInfo.collider.name == gameObject.name && 
                Input.GetMouseButtonDown(0))
			{
                isSelected = true;
				clickedBuilding = true;
                factoryPanel.SetActive(true);
                selectedFactoryName = hitInfo.collider.name;
				uiManager.SetAllOffBut(factoryPanel);
			}
            //If panel is on then lets conisder if they want to have ui go away 
            //if we click away
            if (isSelected && hitInfo.collider.name != gameObject.name
                && !ShiftKeyDown())
            {
                if (!EventSystem.current.IsPointerOverGameObject() && 
                    Input.GetMouseButtonDown(0) && isSelected)
                {
                    clickedBuilding = false;
                    factoryPanel.SetActive(false);
                    isSelected = false;
                }

                //If we hit another factory lets keep the UI on since 
                //they all share a common UI panel but of course the queues
                //are different
                if(HitAnotherFactory(hitInfo))
                {
                    factoryPanel.SetActive(true);
                }
            }

            //Shift to select multiple factories at once
            if(ShiftKeyDown())
            {
                isSelected = true;
                clickedBuilding = true; 
            }
		}
	}

    public bool HitAnotherFactory(RaycastHit hitInfo)
    {
        if(hitInfo.collider.name.Contains("Factory"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ShiftKeyDown()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetNextInQueue(Queue<UnitStruct> queue)
    {
        string unitName;
        UnitStruct[] queueArray = queue.ToArray();
        string[] nameSplit;
        if(queue.Count == 0 || queue.Count == 1)
        {
            return null;
        }
        else
        {
            UnitStruct unitFromQueue = queueArray[1];
            nameSplit = unitFromQueue.unit.name.Split('_');
            unitName = nameSplit[1];
        }

        return unitName;
    }

    public void DeleteUnitFromQueue()
    {
        if (unitQueue.Count > 0)
        {
            UnitStruct unitToDelete = unitQueue.Peek();
            gameController.currency += unitToDelete.cost;
            unitQueue.Dequeue();
            Debug.Log("Unit Queue is now " + unitQueue.Count);
        }
    }

    public bool HasMoreThanOneInQueue(Queue<UnitStruct> queue)
    {
        return queue.Count > 1;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
            //Building dies so does the units its making and your money 
            unitQueue.Clear();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            return;
        }

        if (!collision.gameObject.GetComponent<HyperbitProjectileScript>().owner.Contains("Blue")
            && !collision.gameObject.GetComponent<HyperbitProjectileScript>().team.Equals(team))
        {
            //Physics.IgnoreLayerCollision(9, 10, false);
            if (collision.gameObject.tag.Contains("Laser") && collision.gameObject.layer == 10)
            {
                TakeDamage(GameController.LASER_DAMAGE);
            }
            else if (collision.gameObject.tag.Contains("Cluster") && collision.gameObject.layer == 10)
            {
                TakeDamage(GameController.CLUSTER_BOMB_DAMAGE);
            }
        }
    }
}
