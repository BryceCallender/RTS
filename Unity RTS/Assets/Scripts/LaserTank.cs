using UnityEngine;
using Unit;

public class LaserTank : Tank
{
	#region variables

	[Header("Laser Effects")]
	public float damage;
    public bool isCharging;
    public bool isCharged;
    public bool isFiring;
    public GameObject chargeArea;
    private int tankTeam = (int)Team.BLUE;
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
    private Vector3 start;
    private Vector3 end;
    private UnitSelected unitSel;
    private bool enemySelected = false;
    private Transform turretPos;

    private float chargeTime = 0;
    private float timeToCharge = 4.0f;
	private Enemy enemy;
	#endregion

	// Use this for initialization
	void Start () 
    {
		damage = 10;
        chargeEffect = chargeArea.GetComponentInChildren<ParticleSystem>();
		chargeEffect.gameObject.SetActive(false);
        unitSel = GetComponent<UnitSelected>();
        turretPos = this.gameObject.transform.Find("Turret");
	}

	// Update is called once per frame
	void Update () 
    {
		if (unitSel.isFirst)
		{
			ShowImage();
		}

		if (nearestEnemy == null)
        {
            DestroyLaser();
        }
			
        //if(unitSel.selected)
        //{
        //    healthBar.gameObject.SetActive(true);
        //}

        //if (!unitSel.selected && healthBar.gameObject.activeSelf)
        //{
        //    HealthBarFadeAway();
        //}

	}

	private void FixedUpdate()
	{
		Fire();
	}

	private void Charge()
    {
        if(isCharging)
        {
            ChargeEffect();
        }
		else
		{
			chargeEffect.gameObject.SetActive(false);
			//chargeEffect.Stop();
		}
    }

    public void ChargeEffect()
    {
        chargeTime += Time.deltaTime;
        isCharging = true;
		//chargeEffect.Play();
		chargeEffect.gameObject.SetActive(true);
		if (chargeTime >= timeToCharge)
        {
            isCharged = true;
            isCharging = false;
            chargeTime = 0;
			chargeEffect.gameObject.SetActive(false);
			//chargeEffect.Stop();
            return;
        }
    }

    public void Fire()
    {
        if (unitSel.selected || enemySelected)
        {
            LockOn();
            if (nearestEnemy != null)
            {
                enemyDirection = nearestEnemy.transform.position - this.transform.position;
				enemy = nearestEnemy.GetComponent<Enemy>();
                enemySelected = true; 
                isCharging = true;
				if (!isCharged)
                    Charge();
                else
				{
					ShootBeamInDir(enemyDirection);
					enemy.TakeDamage(damage * Time.deltaTime);
				}
                    
            }
            else
            {
                isCharging = false;
                enemySelected = false;
                isFiring = false;
                isCharged = false;
				DestroyLaser();
            }

            if(!isFiring)
            {
                DestroyLaser();
            }
        }
    }

    new public void LockOn()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out rayHitInfo, Mathf.Infinity))
        {
            if (Input.GetMouseButtonDown(1) && unitSel.selected)
            {
                if (rayHitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    nearestEnemy = rayHitInfo.transform.gameObject;
					isCharged = false;
					isCharging = false;
					isFiring = false;
                }
                else if (rayHitInfo.collider.gameObject.name == "RTSTerrain")
                {
                    nearestEnemy = null;
                }
            }

            if (enemyDirection.magnitude <= range)
            {
                if (enemyDirection != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(enemyDirection);
                    turretPos.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
                }
            }
            if (nearestEnemy == null || enemyDirection.magnitude > range)
            {
                turretPos.rotation = Quaternion.Lerp(Quaternion.Euler(enemyDirection), gameObject.GetComponent<Transform>().rotation, 1.0f);
				chargeEffect.gameObject.SetActive(false);
            }
        }
    }

	private void ShootBeamInDir(Vector3 dir)
    {
        if(isFiring)
        {
            MoveLaserBeginning();
            RaycastHit findEnd;
            if (Physics.Raycast(chargeArea.transform.position, dir, out findEnd))
                end = findEnd.point - (dir.normalized * beamEndOffset);
            else
                end = transform.position + (dir * 10);
            MoveLaserEnd(chargeArea.transform.position,end);
			line.SetPosition(1, end);
		}

        if (!isFiring)
        {
            isFiring = true;
            start = chargeArea.transform.position;

            beam = Instantiate(beamLineRendererPrefab, chargeArea.transform.position, Quaternion.identity) as GameObject;
            line = beam.GetComponent<LineRenderer>();

            line.positionCount = 2;
            line.SetPosition(0, start);
            beamStart = Instantiate(beamStartPrefab, start, Quaternion.identity) as GameObject;
            beamStart.transform.position = start;

            end = Vector3.zero;
            beamEnd = Instantiate(beamEndPrefab, end, Quaternion.identity) as GameObject;
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
    }

	private void DestroyLaser()
    {
        Destroy(beam);
        Destroy(beamStart);
        Destroy(beamEnd);
        isFiring = false;
    }

    private void MoveLaserBeginning()
    {
        line = beam.GetComponent<LineRenderer>();
        line.SetPosition(0,chargeArea.transform.position);
    }

    private void MoveLaserEnd(Vector3 startPos, Vector3 endPos)
    {
        beamStart.transform.position = startPos;
        beamEnd.transform.position = endPos;
    }

	public void ShowImage()
	{
		UIManager.Instance.SetPhoto(this.gameObject.name);
	}
}
