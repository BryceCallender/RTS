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

    public int speed = 30;


    private bool hasCollided = false;
    private Vector3 direction;

	private float timer = 0;
	private float timeToWait = 1.0f;
 
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
		if(muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }

    void OnCollisionEnter(Collision hit)
    {
        //if (hit.gameObject.tag == "Enemy")
        //{
        //Debug.Log(hit.collider.gameObject.name);
        if(!Physics.GetIgnoreLayerCollision(8,10) || !Physics.GetIgnoreLayerCollision(9,10))
        {
            if (!hasCollided)
            {
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
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 3f);
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
            //}
            //else
            //{
            //    Destroy(gameObject, 5f);
            //}
        //}
    }

  //  public void HitEnemy(Vector3 enemyPosition)
  //  {
  //      direction = enemyPosition - this.transform.position;
  //      float distance = speed * Time.deltaTime;
  //      //Debug.Log(direction.ToString());
  //      Debug.Log(direction.magnitude);
  //      Debug.Log(distance);
		//if (direction.magnitude <= distance)
		//{
			
		//}
    }
}