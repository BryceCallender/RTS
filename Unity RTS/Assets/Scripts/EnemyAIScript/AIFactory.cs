using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class AIFactory : MonoBehaviour 
{
    [Header("Factory Attributes")]
    public float health;
    public int resourceCost;
    private int team = (int)Team.RED;
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

    public GameObject QueueUnit()
    {
        float weight = Random.Range(0, 1);
        if(weight > 0.50)
        {
            //Spawn tank
        }
        else if(weight > 0.25 && weight < 0.50)
        {
            //spawn galaxy
        }
        else
        {
            //spawn laser
        }

        //just getting rid of error
        return null;
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
}
