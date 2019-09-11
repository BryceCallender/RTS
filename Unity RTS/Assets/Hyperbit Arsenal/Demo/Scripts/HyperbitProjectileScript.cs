using UnityEngine;
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
    public Team team;
    public int speed = 250;
    private bool hasCollided = false;

    private float timeToKill = 5.0f;
    private float timerToKill = 0;

    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (owner != hit.gameObject.name)
        {
            if (RTSObject.CanDamage(team, hit.gameObject.GetComponent<RTSObject>().team))
            {
                if (!hasCollided)
                {
                    hasCollided = true;
                    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

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
        if (timerToKill >= timeToKill)
        {
            Destroy(gameObject);
            timerToKill = 0;
        }
    }
}