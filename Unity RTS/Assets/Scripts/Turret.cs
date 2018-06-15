using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class Turret : Building 
{
    public List<GameObject> targets;
    public GameObject bullet;
    public Transform turretEnd;
    public Transform turretRotator;
	public float health = 100f;
    public int range = 15;
    public int team;
    public int layerTeam;
	
	/// <summary>
	/// Y rotation speed while the turret is idle in degrees per second
	/// </summary>
	public float idleRotationSpeed = 39f;

	/// <summary>
	/// The time it takes for the tower to correct its x rotation on idle in seconds
	/// </summary>
	public float idleCorrectionTime = 2.0f;
	
	/// <summary>
	/// Counter used for x rotation correction
	/// </summary>
	protected float m_XRotationCorrectionTime;
	
	/// <summary>
	/// How fast this turret is spinning
	/// </summary>
	protected float m_CurrentRotationSpeed;
	
	/// <summary>
	/// The seconds until the tower starts spinning
	/// </summary>
	protected float m_WaitTimer = 0.0f;
	
	/// <summary>
	/// How long the turret waits in its idle form before spinning in seconds
	/// </summary>
	public float idleWaitTime = 2.0f;

	// Use this for initialization
	protected void Start () 
    {
        targets = new List<GameObject>();
	    m_WaitTimer = idleWaitTime;
    }

	protected void Update()
	{
		AimTurret();
	}
	
	/// <summary>
	/// Aims the turret at the current target
	/// </summary>
	protected virtual void AimTurret()
	{
		if (turretRotator == null)
		{
			return;
		}

		if (targets.Count == 0) // do idle rotation
		{
			if (m_WaitTimer > 0)
			{
				m_WaitTimer -= Time.deltaTime;
				if (m_WaitTimer <= 0)
				{
					m_CurrentRotationSpeed = (Random.value * 2 - 1) * idleRotationSpeed;
				}
			}
			else
			{
				Vector3 euler = turretRotator.rotation.eulerAngles;
				euler.x = Mathf.Lerp(Wrap180(euler.x), 0, m_XRotationCorrectionTime);
				m_XRotationCorrectionTime = Mathf.Clamp01((m_XRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
				euler.y += m_CurrentRotationSpeed * Time.deltaTime;

				turretRotator.eulerAngles = euler;
			}
		}
	}

	/// <summary>
	/// A simply function to convert an angle to a -180/180 wrap
	/// </summary>
	public static float Wrap180(float angle)
	{
		angle %= 360;
		if (angle < -180)
		{
			angle += 360;
		}
		else if (angle > 180)
		{
			angle -= 360;
		}
		return angle;
	}
}
