using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;
	private float sensitivity = 1f;
	private Transform orbit;
	private bool rotating = false;

	int mod(int x, int m)
	{
		return (x % m + m) % m;
	}

	void Start()
	{
		orbit = new GameObject("CameraOrbit").transform;
		orbit.SetPositionAndRotation(target.position,Quaternion.identity);
		this.transform.SetParent(orbit);
	}

	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			
			if (touch.phase == TouchPhase.Began)
			{
				RaycastHit hit;
				rotating = !Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit);
			}
			if (touch.phase == TouchPhase.Moved && this.rotating)
			{
				float h = sensitivity * touch.deltaPosition.x * touch.deltaTime;
				float v = sensitivity * touch.deltaPosition.y * touch.deltaTime;

				if ((orbit.transform.eulerAngles.x + v + 90) % 360 <= 5.0f || (orbit.transform.eulerAngles.x + v + 90) % 360 >= 175.0f)
				{
					v = 0;
				}
				orbit.transform.eulerAngles = new Vector3(orbit.transform.eulerAngles.x + v, orbit.transform.eulerAngles.y + h, orbit.transform.eulerAngles.z);

				}
		}
		transform.LookAt(target.position);
	}
}
