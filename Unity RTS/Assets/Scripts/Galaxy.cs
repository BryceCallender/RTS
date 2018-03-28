using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unit;

public class Galaxy : UnitStats, IImageable
{
    //TODO:: make galaxies fly in the air maybe?
	public int damage = 10;
	public float health = 150;
	public int range = 10;
    public int cost = 20;
    public int capacity = 10;

	public GameObject[] enemies;
	public GameObject nearestEnemy;
    public GameObject Thruster;
    public GameObject bulletPrefab;
    [SerializeField]
    private GameObject[] turrets;

    public Slider healthBar;
    private Quaternion keepUIAbove;
    public Canvas canvas; 

	private float fireCoolDown = 1.5f;
	private float fireCoolDownLeft = 0;
	private RaycastHit hitInfo;
	private Vector3 direction;
    private ParticleSystem thruster;
	private UnitSelected unitSelected;
	private Vector3 position;
    private NavMeshAgent agent;
    private int team = (int)Team.BLUE;

    GameObject projectile;
    private bool enemyHasBeenSelected = false;
    private float timerToStop = 0;
    private float timeToStopShowingHealth = 3.0f;

    public HyperbitProjectileScript hyperProjectileScript;

    private void Start()
    {
        //GameController.unitNames.Add(gameObject.name, cost);
        health = 150;
        projectile = bulletPrefab;
        keepUIAbove = canvas.GetComponent<RectTransform>().rotation;
        thruster = Thruster.GetComponentInChildren<ParticleSystem>();
        turrets = GameObject.FindGameObjectsWithTag("GalaxyTurrets");
        healthBar.gameObject.SetActive(false);
        healthBar.maxValue = health;
        healthBar.value = health;
        thruster.Stop();
        agent = GetComponent<NavMeshAgent>();
		unitSelected = GetComponent<UnitSelected>();
	}

    private void Update()
    {
        canvas.GetComponent<RectTransform>().rotation = keepUIAbove;
        if(agent.velocity != Vector3.zero)
        {
            ActivateThrusters();
        }
        else
        {
            DeActivateThrusters();
        }

		if (unitSelected.isFirst)
		{
			ShowImage();
		}

        if (unitSelected.selected)
        {
            healthBar.gameObject.SetActive(true);
        }

        if (!unitSelected.selected && healthBar.gameObject.activeSelf)
        {
            HealthBarFadeAway();
        }
	}

	private void FixedUpdate()
	{
		Fire();
	}

	public override void Fire()
    {
        if (unitSelected.selected || enemyHasBeenSelected)
        {
            LockOn();
            enemyHasBeenSelected = true;
            if (nearestEnemy != null)
            {
                GameObject turretToFire = turrets[RandomizeTurretSelection()];
                direction = nearestEnemy.transform.position - turretToFire.transform.position;
                fireCoolDownLeft -= Time.deltaTime;
                if (fireCoolDownLeft <= 0 && direction.magnitude <= range)
                {
                    fireCoolDownLeft = fireCoolDown;
                    projectile = (GameObject)Instantiate(bulletPrefab, turretToFire.transform.position, turretToFire.transform.rotation);
                    projectile.tag = "Cluster";
                    projectile.GetComponent<HyperbitProjectileScript>().owner = gameObject.name;
                    projectile.GetComponent<HyperbitProjectileScript>().team = team;
                    int speed = projectile.GetComponent<HyperbitProjectileScript>().speed;
                    projectile.GetComponent<Rigidbody>().AddForce(direction * speed);
                }
			}
            else
            {
                enemyHasBeenSelected = false;  
            }
        }
    }

	public void LockOn()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
		{
			if (Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<UnitSelected>().selected)
			{
				if (hitInfo.collider.gameObject.CompareTag("Enemy"))
				{
					nearestEnemy = hitInfo.transform.gameObject;
				}
				else if (hitInfo.collider.gameObject.name == "RTSTerrain")
				{
					nearestEnemy = null;
				}
                if(nearestEnemy != null)
                {
					for (int i = 0; i < turrets.Length; i++)
					{
						turrets[i].transform.LookAt(nearestEnemy.transform.position);
					}
                }
			}

			if (direction.magnitude <= range)
			{
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                }
			}
            if (nearestEnemy == null || direction.magnitude > range)
            {
                //foreach(GameObject turrentRotation in turrets)
                //{
                //    turrentRotation.transform.rotation = Quaternion.Lerp(Quaternion.Euler(direction), gameObject.GetComponent<Transform>().rotation, 1.0f);
                //}
            }
		}
	}

    public override void Die()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        healthBar.gameObject.SetActive(true);
        health -= damage;
        healthBar.value -= damage;
        if (health <= 0)
        {
            Die();
        }
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
        random = (int)Random.Range(0,turrets.Length);
        return random;
    }

	public void ShowImage()
	{
		UIManager.Instance.SetPhoto(this.gameObject.name);
	}

    public void HealthBarFadeAway()
    {
        timerToStop += Time.deltaTime;
        if (timerToStop >= timeToStopShowingHealth)
        {
            timerToStop = 0;
            healthBar.gameObject.SetActive(false);
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
            else if (collision.gameObject.tag.Contains("Missle")
                     && collision.gameObject.layer == 10)
            {
                TakeDamage(GameController.MISSILE_DAMAGE);
            }
        }
    }
}
