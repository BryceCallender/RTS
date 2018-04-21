using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    private UnitScript unit;
    [Header("Unit UI References")]
    public Slider healthBar;
    public Canvas canvas;

    private float timerToStop = 0;
    private float timeToStopShowingHealth = 3.0f;
    private Quaternion keepUIAbove;

    // Use this for initialization
    void Start ()
    {
        unit = gameObject.GetComponent<UnitScript>();
        healthBar.gameObject.SetActive(false);
        healthBar.maxValue = unit.health;
        healthBar.value = unit.health;
        keepUIAbove = canvas.GetComponent<RectTransform>().rotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //oof TODO::fix this gross getcomponent every frame BAD PERFORMANCE
        canvas.GetComponent<RectTransform>().rotation = keepUIAbove;
    }

    public void HealthBarFadeAway()
    {
        timerToStop += Time.deltaTime;
        if (timerToStop >= timeToStopShowingHealth)
        {
            timerToStop = 0;
            healthBar.gameObject.SetActive(false);
        }
    }

    public void SetHealthBar(bool status)
    {
        healthBar.gameObject.SetActive(status);
    }

    public void UpdateHealthBar(float damage)
    {
        Debug.Log(healthBar.value);
        healthBar.value -= damage;
    }
}
