using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
	public Camera mainCamera;

	Vector3 screenTopLeft;
	Vector3 screenBottomRight;

	Vector3 miniMapTopLeft;
	Vector3 miniMapBottomRight;


	private void OnGUI()
	{
		//GUI.Box(new Rect(0, 0, 0, 0),);
	}
}
