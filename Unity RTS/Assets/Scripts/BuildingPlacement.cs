﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour 
{
    static GameController gameController;
    private PlaceableBuilding placeableBuilding;
    private Transform currentBuilding;
    public bool hasPlaced;

    private Color notPlacable;



	// Use this for initialization
	void Start () 
    {
        gameController = FindObjectOfType<GameController>();
        notPlacable = Color.red;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(currentBuilding != null)
        {
            MoveToMousePosition();
            hasPlaced |= (Input.GetMouseButtonDown(0) && IsValidSpot());
            if(!IsValidSpot())
            {
                gameController.buildingErrorText.gameObject.SetActive(true);
                gameController.FadeAlpha(gameController.colorFadeTime,
                                         ref gameController.buildingErrorText,
                                         ref gameController.buildingErrorColor);
                if(gameController.IsZeroAlpha(gameController.buildingErrorColor))
                {
                    gameController.SetAlphaBack(ref gameController.buildingErrorText,
                                                ref gameController.buildingErrorColor);
                }

            }
        }

    }

    public void SetBuilding(GameObject building)
    {
        currentBuilding = ((GameObject)Instantiate(building)).transform;
        placeableBuilding = currentBuilding.GetComponent<PlaceableBuilding>();
    }

    private bool IsValidSpot()
    {
        if(placeableBuilding.collisions.Count > 0)
        {
            return false;
        }
        return true;
    }

    private void MoveToMousePosition()
    {
        if(!hasPlaced)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                currentBuilding.position = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
            }   
        }
    }
}