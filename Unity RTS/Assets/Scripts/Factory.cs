using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unit;

[RequireComponent(typeof(GameController))]
public class Factory : Building
{
    [Header("Factory Attributes")]
    private GameObject unitGameObject;

    public Sprite tank;
    public Sprite galaxy;

    private float spawnTimer = 5.0f;
    private float spawnTimerCoolDown = 5.0f;

    [Header("Factory Queue's and Buidlable Units")]
    private Queue<UnitStruct> unitQueue;
    
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
	private UIManager uiManager;
    private HyperbitProjectileScript hyperProjectileScript;

    [SerializeField] 
    private Transform queueSlot;
    [SerializeField]
    private Image[] unitSpriteList;
    [SerializeField]
    private TextMeshProUGUI[] boxNumbers;

    // Use this for initialization
    void Start()
    {
        isSelected = false;
        gameController = GameController.Instance;
		uiManager = gameController.GetComponent<UIManager>();
        unitQueue = new Queue<UnitStruct>();
        unitSliderImage = unitSpawnSlider.GetComponentInChildren<Image>();
        //factoryPanel.SetActive(false);
        //factoryQueuePanel.SetActive(false);
        unitSpawnSlider.maxValue = spawnTimer;
        unitSpawnSlider.value = 0;
        isShowingNextUnit = false;

//        unitSpriteList = new Sprite[queueSlots.Length];
//        boxNumbers = new TextMeshProUGUI[queueSlots.Length];
//        
//        for (int i = 0; i < queueSlots.Length; i++)
//        {
//            unitSpriteList[i] = queueSlots[i].GetComponentInChildren<Image>().sprite;
//            boxNumbers[i] = queueSlots[i].GetComponentInChildren<TextMeshProUGUI>();
//        }
    }

    // Update is called once per frame
    void Update()
    {
        //Call to see if we clicked the building or not to see if we can even 
        //make units or not 
        if (ClickedBuilding())
        {
            if (unitQueue.Count > 0)
            {
                //Go through all 5 units and set images if we have a queue
                setImages();
                //Get progress of the unit we are making and update slider
                getProgress();
            } 
        }

        if(unitQueue.Count > 0)
        {
			//Show the green timer system 
			unitSpawnSlider.gameObject.SetActive(true);
            //factoryQueuePanel.SetActive(true);
			unitSpawnSlider.value += Time.deltaTime;

            //If we have more than one in our queue lets tell the player
            //what is next in the queue!
            if(HasMoreThanOneInQueue(unitQueue) && !isShowingNextUnit)
            {
                isShowingNextUnit = true;
            }

			//Once timer hits 0 or lower we will spawn unit reset counter for 
			//next one and dequeue from the queue changing the amount being made
			if (unitSpawnSlider.value > spawnTimer)
			{
			    MoveSprites();
			    ClearSprite();
			    //Move the queue images one down
			    
				spawnTimer = 0;
                Instantiate(unitQueue.Peek().unit, unitSpawn.transform.position, unitSpawn.transform.rotation);
                unitQueue.Dequeue();
                //Reset to change the next name of the next unit
                isShowingNextUnit = false;
			}
        }
        else
        {
			//Dont show the animation since we have none in queue
            //unitSpawnSlider.gameObject.SetActive(false);
            //factoryQueuePanel.SetActive(false);
        }    

    }

    public void AddTankToQueue()
    {
        if(isSelected)
        {
            SetUnit(buildableUnits.Find(x => x.gameObject.name.Contains("Tank")));
            AddUnitToQueue();
        }
    }

    public void AddGalaxyToQueue()
    {
        if(isSelected)
        {
            SetUnit(buildableUnits.Find(x => x.gameObject.name.Contains("Galaxy")));
            AddUnitToQueue();
        }
    }

    private void SetUnit(GameObject obj)
    {
        unitGameObject = obj;
    }

    private bool ClickedBuilding()
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
                //factoryPanel.SetActive(true);
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
                    //factoryPanel.SetActive(false);
                    isSelected = false;
                }

                //If we hit another factory lets keep the UI on since 
                //they all share a common UI panel but of course the queues
                //are different
                if(HitAnotherFactory(hitInfo))
                {
                    //factoryPanel.SetActive(true);
                }
            }

            //Shift to select multiple factories at once
            if(ShiftKeyDown())
            {
                isSelected = true;
                clickedBuilding = true; 
            }
		}

	    return isSelected;
	}

    private void ClearSprite()
    {
        int count = unitQueue.Count - 1;
        unitSpriteList[count].sprite = null;
        unitSpriteList[count].color = Color.black;
        boxNumbers[count].gameObject.SetActive(true);
    }

    private void setImages()
    {
        int count = unitQueue.Count - 1;
        unitSpriteList[count].sprite = unitGameObject.GetComponent<UnitScript>().sprite;
        unitSpriteList[count].color = Color.white;
        boxNumbers[count].gameObject.SetActive(false);
    }

    private void getProgress()
    {
        
    }

    private void MoveSprites()
    {
        for (int i = 0; i < unitQueue.Count - 1; i++)
        {
            unitSpriteList[i].sprite = unitSpriteList[i + 1].sprite;
        }
    }

    private bool HitAnotherFactory(RaycastHit hitInfo)
    {
        if(hitInfo.collider.name.Contains("Factory"))
        {
            return true;
        }
        return false;
    }

    private bool ShiftKeyDown()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        return false;
    }

    private string GetNextInQueue(Queue<UnitStruct> queue)
    {
        string unitName;
        UnitStruct[] queueArray = queue.ToArray();
        string[] nameSplit;
        if(queue.Count == 0 || queue.Count == 1)
        {
            return null;
        }

        UnitStruct unitFromQueue = queueArray[1];
        nameSplit = unitFromQueue.unit.name.Split('_');
        unitName = nameSplit[1];

        return unitName;
    }

    private void AddUnitToQueue()
    {
        if (unitQueue.Count > 5)
        {
            return;
        }
        
        int cost = unitGameObject.GetComponent<UnitScript>().cost;
        if (gameController.currency >= cost)
        {
            UnitStruct unitToQueue;
            unitToQueue.unit = unitGameObject;
            unitToQueue.cost = cost;
            unitToQueue.name = UnitName.GetNameOfUnit(unitGameObject);
            unitToQueue.sprite = unitGameObject.GetComponent<UnitScript>().sprite;

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

    private static bool HasMoreThanOneInQueue(Queue<UnitStruct> queue)
    {
        return queue.Count > 1;
    }
}
