using UnityEngine;
using Unit;

public class UnitScript: MonoBehaviour
{
    private HyperbitProjectileScript hyperbitProjectileScript;

    public int team = (int)Team.BLUE;
    public int damage;
    public float health;
    public int range;
    public int cost;

    public void TakeDamage(float damage)
    {
        //healthBar.gameObject.SetActive(true);
        //isUnderAttack = true;
        health -= damage;
        //healthBar.value -= damage;
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
