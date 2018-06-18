using UnityEngine;
using Unit;

public class UnitScript: RTSObject, IDamageable, ISelectable
{
    /// <summary>
    /// Gets the current health.
    /// </summary>
    [SerializeField]
    private float currentHealth;
	
    /// <summary>
    /// Gets whether this instance is dead.
    /// </summary>
    public bool isDead
    {
        get { return currentHealth <= 0f; }
    }
    
    [SerializeField]
    private HealthManager healthManager;

    public float health;
    public int cost;
    public int capacityAmount;
    
    private bool isUnderAttack;
    private bool canDamage;

    private void Awake()
    {
        healthManager = gameObject.GetComponent<HealthManager>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (isDead)
        {
            Die();
        }
    }

    /// <summary>
    /// Takes damage but checks if it can be damaged first
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageAlignment"></param>
    /// <returns></returns>
    public bool TakeDamage(float damage, IAlignmentProvider damageAlignment)
    {   
        canDamage = damageAlignment == null || alignmentProvider == null ||
                    damageAlignment.CanHarm(alignmentProvider);
		
        if (isDead || !canDamage)
        {
            return false;
        }

        currentHealth -= damage;
        return true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Typical take damage only when we get triggered
    /// </summary>
    /// <param name="collision"></param>
    protected void OnTriggerEnter(Collider collision)
    {
        //Bullet hit the unit so lets get the bullets allignment
        var hitAlignment = collision.gameObject.GetComponent<HyperbitProjectileScript>().alignmentProvider;
        
        if (collision.gameObject.name.Contains("Laser")
            && collision.gameObject.layer == 10)
        {
            TakeDamage(GameController.LASER_DAMAGE,hitAlignment);
        }
        else if (collision.gameObject.name.Contains("Cluster")
                 && collision.gameObject.layer == 10)
        {
            TakeDamage(GameController.CLUSTER_BOMB_DAMAGE,hitAlignment);
        }
        else if (collision.gameObject.name.Contains("Missle")
                 && collision.gameObject.layer == 10)
        {
            TakeDamage(GameController.MISSILE_DAMAGE,hitAlignment);
        }
    }
}
