using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasFacility : Building
{
    public int gasReceived = 2;

    public Transform pump;

    private bool isMiningGas;

    protected override void Update()
    {
        base.Update();

        if(IsBuildingAvailableToUse())
        {
            pump.position = new Vector3(pump.position.x, Mathf.PingPong(Time.time, 0.85f), pump.position.z);

            if(!isMiningGas)
            {
                isMiningGas = true;
                StartCoroutine(MineGas());
            }
        }
    }

    private IEnumerator MineGas()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);

            Debug.Log("Mined some gas");

            GameController.Instance.GetPlayer().ChangeGasCurrency(-gasReceived);
        }
    }
}
