﻿using UnityEngine;
using System.Collections;
 
public class HyperbitProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
    public string owner;
    public int team;
    public int speed = 250;
    private bool hasCollided = false;

	private SphereCollider sphereCollider;
	private Rigidbody rb;

    private float timeToKill = 5.0f;
    private float timerToKill = 0;

	private int hitObjectTeam = 0;

	void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
		if(muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}

		sphereCollider = GetComponent<SphereCollider>();
		rb = GetComponent<Rigidbody>();
	}

  //  void OnCollisionEnter(Collision hit)
  //  {
		//if(hit.gameObject.name.Contains("Blue"))
		//{
		//	hitObjectTeam = 0;
		//}
		//else
		//{
		//	hitObjectTeam = 1;
		//}

  //      if (owner != hit.gameObject.name)
  //      {
  //          if(hitObjectTeam != team)
  //          {
		//		if (!hasCollided)
  //              {
  //                  //Debug.Log("Killed by " + hit.collider.gameObject.name);
  //                  //Debug.Log(hit.gameObject.name);
  //                  hasCollided = true;
  //                  //transform.DetachChildren();
  //                  impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
  //                  //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

  //                  //if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
  //                  //{
  //                  //  Destroy(hit.gameObject);
  //                  //}

  //                  //yield WaitForSeconds (0.05);
  //                  foreach (GameObject trail in trailParticles)
  //                  {
  //                      GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
  //                      curTrail.transform.parent = null;
  //                      Destroy(curTrail, 3f);
  //                  }
  //                  Destroy(projectileParticle, 1f);
  //                  Destroy(impactParticle, 1f);
  //                  Destroy(gameObject);
  //                  //projectileParticle.Stop();

  //                  ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
  //                  //Component at [0] is that of the parent i.e. this object (if there is any)
  //                  for (int i = 1; i < trails.Length; i++)
  //                  {
  //                      ParticleSystem trail = trails[i];
  //                      if (!trail.gameObject.name.Contains("Trail"))
  //                          continue;

  //                      trail.transform.SetParent(null);
  //                      Destroy(trail.gameObject, 2);
  //                  }
  //              } 
  //          }
		//	//else
		//	//{
		//	//	TurnOffCollisions(rb, sphereCollider);
		//	//}
  //      }
  //  }

	private void OnTriggerEnter(Collider hit)
	{
		if (hit.gameObject.name.Contains("Blue"))
		{
			hitObjectTeam = 0;
		}
		else
		{
			hitObjectTeam = 1;
		}

		if (owner != hit.gameObject.name)
		{
			if (hitObjectTeam != team)
			{
				if (!hasCollided)
				{
					//Debug.Log("Killed by " + hit.collider.gameObject.name);
					//Debug.Log(hit.gameObject.name);
					hasCollided = true;
					//transform.DetachChildren();
					impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
					//Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

					//if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
					//{
					//  Destroy(hit.gameObject);
					//}

					//yield WaitForSeconds (0.05);
					foreach (GameObject trail in trailParticles)
					{
						GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
						curTrail.transform.parent = null;
						Destroy(curTrail, 3f);
					}
					Destroy(projectileParticle, 1f);
					Destroy(impactParticle, 1f);
					Destroy(gameObject);
					//projectileParticle.Stop();

					ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
					//Component at [0] is that of the parent i.e. this object (if there is any)
					for (int i = 1; i < trails.Length; i++)
					{
						ParticleSystem trail = trails[i];
						if (!trail.gameObject.name.Contains("Trail"))
							continue;

						trail.transform.SetParent(null);
						Destroy(trail.gameObject, 2);
					}
				}
			}
		}
	}

	public void Update()
    {
        timerToKill += Time.deltaTime;
        if(timerToKill >= timeToKill)
        {
            Destroy(gameObject);
            timerToKill = 0;
        }
    }
}