using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;
	private float sensitivity = 1f;
	private bool rotating = false;

	int mod(int x, int m)
	{
		return (x % m + m) % m;
	}

	void Start()
	{
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

				this.transform.position = target.position;
				this.transform.Rotate(Vector3.right, -v);
				this.transform.Rotate(Vector3.up, h, Space.World);
				this.transform.Translate(new Vector3(0, 0, -10));
			}
		}
	}
}
