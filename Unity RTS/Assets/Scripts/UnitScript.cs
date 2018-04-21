using UnityEngine;
using Unit;

public class UnitScript: MonoBehaviour
{
    private HyperbitProjectileScript hyperbitProjectileScript;
    [SerializeField]private HealthManager healthManager;

    public int team = (int)Team.BLUE;
    public int damage;
    public float health;
    public int range;
    public int cost;

    private bool isUnderAttack;

    private void Awake()
    {
        healthManager = gameObject.GetComponent<HealthManager>();
    }

    public void TakeDamage(float damage)
    {
        healthManager.SetHealthBar(true);
        isUnderAttack = true;
        health -= damage;
        healthManager.UpdateHealthBar(damage);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        hyperbitProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

        if (hyperbitProjectileScript.team.Equals(team))
        {
            return;
        }

        if (!hyperbitProjectileScript.owner.Contains("Blue")
            && !hyperbitProjectileScript.team.Equals(team))
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
