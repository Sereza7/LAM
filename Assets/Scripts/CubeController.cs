using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
	private float sensitivity = 1f;
	private bool rotating = false;


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
				rotating = Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out RaycastHit hit);
			}
			if (touch.phase == TouchPhase.Moved && this.rotating)
			{
				float h = sensitivity * touch.deltaPosition.x * touch.deltaTime;
				float v = sensitivity * touch.deltaPosition.y * touch.deltaTime;

				
				this.transform.Rotate(Vector3.up, v);
				this.transform.Rotate(Camera.main.transform.forward	, h);

			}
		}
	}
}
