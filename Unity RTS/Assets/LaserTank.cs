using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTank : Tank 
{
    [Header("Laser Effects")]
    public bool isCharging;
    public GameObject chargeArea;
    private int tankTeam = 0;
    private float chargeTimer = 2.0f;
    private ParticleSystem chargeEffect;

    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture

    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;
    private RaycastHit rayHitInfo;
    private Vector3 enemyDirection;
    private UnitSelected unitSel;
    private bool enemySelected = false;

    private WaitForSeconds chargeTime = new WaitForSeconds(2.0f);

	// Use this for initialization
	void Start () 
    {
        chargeEffect = chargeArea.GetComponent<ParticleSystem>();
        chargeEffect.Stop();
        beamStart = Instantiate(beamStartPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beamEnd = Instantiate(beamEndPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        beam = Instantiate(beamLineRendererPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        line = beam.GetComponent<LineRenderer>();
        unitSel = GetComponent<UnitSelected>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (unitSel.selected)
        //{
        //    healthBar.gameObject.SetActive(true);
        //}
        //Fire();
        StartCoroutine(ChargeEffect());
	}

    private void Charge(bool charging)
    {
        if(isCharging)
        {
            ChargeEffect();
        }
    }

    IEnumerator ChargeEffect()
    {
        chargeEffect.Play();
        yield return chargeTime;
        chargeEffect.Stop();
    }

    public override void Fire()
    {
        if (unitSel.selected || enemySelected)
        {
            base.LockOn();
            enemyDirection = nearestEnemy.transform.position - this.transform.position;
            enemySelected = true;
            if (nearestEnemy != null)
            {
                isCharging = true;
                Charge(isCharging);
                ShootBeamInDir(this.transform.position,enemyDirection);
            }
            else
            {
                isCharging = false;
                enemySelected = false;
            }
        }
    }

    void ShootBeamInDir(Vector3 start, Vector3 dir)
    {
        line.positionCount = 2;
        line.SetPosition(0, start);
        beamStart.transform.position = start;

        Vector3 end = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit))
            end = hit.point - (dir.normalized * beamEndOffset);
        else
            end = transform.position + (dir * 100);

        beamEnd.transform.position = end;
        line.SetPosition(1, end);

        beamStart.transform.LookAt(beamEnd.transform.position);
        beamEnd.transform.LookAt(beamStart.transform.position);

        float distance = Vector3.Distance(start, end);
        line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
        line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hyperProjectileScript = collision.gameObject.GetComponent<HyperbitProjectileScript>();

        if (hyperProjectileScript.team.Equals(tankTeam))
        {
            return;
        }

        if (!hyperProjectileScript.owner.Contains("Blue")
            && !hyperProjectileScript.team.Equals(tankTeam))
        {
            //Physics.IgnoreLayerCollision(9, 10, false);
            if (collision.gameObject.tag.Contains("Laser")
                && collision.gameObject.layer == 10)
            {
                TakeDamage(5);
            }
            else if (collision.gameObject.tag.Contains("Cluster")
                     && collision.gameObject.layer == 10)
            {
                TakeDamage(10);
            }
        }
    }
}
