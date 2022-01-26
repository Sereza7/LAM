using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
	//Controls the cube movement in physics puzzles
{
	public static float sensitivity = 1f;
	public static bool useGyro;
	private bool rotating = false; 


	void Start()
	{
		if (SystemInfo.supportsGyroscope) //get permission for gyroscope if possible
		{
			Input.gyro.enabled = true;
			Debug.Log("Gyro Activated!");
		}
	}

	void Update()
	{
		float horizontalMov = 0;
		float verticalMov = 0;

		//Controls with touch
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				rotating = Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit);
			}
			if (touch.phase == TouchPhase.Moved && this.rotating)
			{
				horizontalMov = sensitivity * touch.deltaPosition.x * touch.deltaTime;
				verticalMov = sensitivity * touch.deltaPosition.y * touch.deltaTime;
				this.transform.Rotate(Camera.main.transform.right, verticalMov, Space.World);
				this.transform.Rotate(Camera.main.transform.up, -horizontalMov, Space.World);
			}
		}
		//Controls with Gyroscope
		if (SystemInfo.supportsGyroscope && useGyro)
		{
			horizontalMov = sensitivity * Input.gyro.rotationRate.x ;
			verticalMov = sensitivity * Input.gyro.rotationRate.y;
			this.transform.Rotate(Camera.main.transform.right, -horizontalMov, Space.World);
			this.transform.Rotate(Camera.main.transform.up, -verticalMov, Space.World);
		}
	}
}
