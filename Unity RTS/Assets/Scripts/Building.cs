using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, ISelectable, IDamageable
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
	
	/// <summary>
	/// The alignment of the damager
	/// </summary>
	public SerializableIAlignmentProvider alignment;
	
	/// <summary>
	/// Gets the <see cref="IAlignmentProvider"/> of this instance
	/// </summary>
	public IAlignmentProvider alignmentProvider
	{
		get
		{
			return alignment != null ? alignment.GetInterface() : null;
		}
	}

	protected RequirementStructure requirementStructures;

	private bool canDamage;

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
		if (canDamage)
		{
			var hitAlignment = collision.GetComponent<SerializableIAlignmentProvider>().GetInterface();
			//Physics.IgnoreLayerCollision(9, 10, false);
			if (collision.gameObject.tag.Contains("Laser")
			    && collision.gameObject.layer == 10)
			{
				TakeDamage(GameController.LASER_DAMAGE,hitAlignment);
			}
			else if (collision.gameObject.tag.Contains("Cluster")
			         && collision.gameObject.layer == 10)
			{
				TakeDamage(GameController.CLUSTER_BOMB_DAMAGE,hitAlignment);
			}
			else if (collision.gameObject.tag.Contains("Missle")
			         && collision.gameObject.layer == 10)
			{
				TakeDamage(GameController.MISSILE_DAMAGE,hitAlignment);
			}
		}
	}
}
