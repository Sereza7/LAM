using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
	public static float sensitivity = 1f;
	public static bool useGyro;
	private bool rotating = false;


	void Start()
	{
		if (SystemInfo.supportsGyroscope)
		{
			Input.gyro.enabled = true;
			Debug.Log("Gyro Activated!");
		}
	}

	void Update()
	{
		float h = 0;
		float v = 0;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				rotating = Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit);
			}
			if (touch.phase == TouchPhase.Moved && this.rotating)
			{
				h = sensitivity * touch.deltaPosition.x * touch.deltaTime;
				v = sensitivity * touch.deltaPosition.y * touch.deltaTime;
				this.transform.Rotate(Camera.main.transform.right, v, Space.World);
				this.transform.Rotate(Camera.main.transform.up, -h, Space.World);
			}
		}
		if (SystemInfo.supportsGyroscope && useGyro)
		{
			h = sensitivity * Input.gyro.rotationRate.x ;
			v = sensitivity * Input.gyro.rotationRate.y;
			//Debug.Log(Input.gyro.updateInterval);
			this.transform.Rotate(Camera.main.transform.right, -h, Space.World);
			this.transform.Rotate(Camera.main.transform.up, -v, Space.World);
		}
	}
}
