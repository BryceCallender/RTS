using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Factory : MonoBehaviour
{
    public int health;
    private GameObject unitGameObject;
    private GameObject factoryPanel;
    private GameObject nextUnitInQueue;
    private Transform unitSpawn;
    private Text unitStats;

    public Slider unitSpawnSlider;
    private Image unitSliderImage;

    public Sprite tank;
    public Sprite galaxy;

    private Queue<GameObject> unitQueue;
    private List<GameObject> nextInQueue;
    private List<Sprite> unitSpriteList;

    static GameController gameController;

    private float spawnTimer = 5.0f;
    private float spawnTimerCoolDown = 5.0f;

    private RaycastHit hitInfo;
    private bool clickedBuilding;
    private Transform rallyLocation;
    private bool isTank;

    // Use this for initialization
    void Start()
    {
        health = 300;
        gameController = FindObjectOfType<GameController>();
        unitQueue = new Queue<GameObject>();
        nextInQueue = new List<GameObject>();
        unitSpriteList = new List<Sprite>();
        factoryPanel = GameObject.Find("FactoryPanel");
        unitStats = GameObject.Find("UnitStats").GetComponent<Text>();
        nextUnitInQueue = GameObject.Find("nextUnit");
        unitSliderImage = unitSpawnSlider.GetComponentInChildren<Image>();
        unitSpawn = this.transform.Find("unitSpawn");
        factoryPanel.SetActive(false);
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
			spawnTimer -= Time.deltaTime;
            unitSpawnSlider.value = spawnTimer;
            unitSliderImage.sprite = unitSpriteList[0];

			//Once timer hits 0 or lower we will spawn unit reset counter for 
			//next one and dequeue from the queue changing the amount being made
			if (spawnTimer < 0)
			{
				spawnTimer = spawnTimerCoolDown;
                Instantiate(unitQueue.Peek(), unitSpawn.transform.position, unitSpawn.transform.rotation);
                Debug.Log(unitQueue.Dequeue());
                unitSpriteList.RemoveAt(0);
			}
        }
        else
        {
			//Dont show the animation since we have none in queue
            unitSpawnSlider.gameObject.SetActive(false);
        }

    }

    public void AddTankToQueue()
    {
        if (gameController.currency >= unitGameObject.GetComponent<Tank>().cost)
        {
            unitQueue.Enqueue(unitGameObject);
            gameController.currency -= unitGameObject.GetComponent<Tank>().cost;
            Debug.Log(unitQueue.Count);
        }

    }

    public void AddGalaxyToQueue()
    {
        if (gameController.currency >= unitGameObject.GetComponent<Galaxy>().cost)
        {
            unitQueue.Enqueue(unitGameObject);
            gameController.currency -= unitGameObject.GetComponent<Galaxy>().cost;
            Debug.Log(unitQueue.Count);
        }

    }

    public void DeleteTankFromQueue()
    {
        if (unitQueue.Count > 0)
        {
            unitQueue.Dequeue();
            gameController.currency += unitGameObject.GetComponent<Tank>().cost;
            Debug.Log(unitQueue.Count);
        }
    }

	public void DeleteGalaxyFromQueue()
	{
		if (unitQueue.Count > 0)
		{
			unitQueue.Dequeue();
            gameController.currency += unitGameObject.GetComponent<Galaxy>().cost;
			Debug.Log(unitQueue.Count);
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
			if (hitInfo.collider.name == "Building_Factory_Blue" && Input.GetMouseButtonDown(0))
			{
				clickedBuilding = true;
                factoryPanel.SetActive(true);
			}
			else
			{
				//if(hitInfo.collider.name == "RTSTerrain" && Input.GetMouseButtonDown(0))
				//{
				    clickedBuilding = false;
    //                factoryPanel.SetActive(false);
				//}

			}
		}
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Laser") && collision.gameObject.layer == 10)
        {
            TakeDamage(5);
        }
        else if (collision.gameObject.tag.Contains("Cluster") && collision.gameObject.layer == 10)
        {
            TakeDamage(10);
        }
    }


}
