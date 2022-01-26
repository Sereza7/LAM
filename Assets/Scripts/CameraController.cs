using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	//Controls the camera movements on physics puzzles

	public Transform target; //The transform at which the camera will be pointing towards
	private float sensitivity = 1f; //Sensitivity of camera controls
	private bool rotating = false; //state of the camera

	int mod(int x, int m) //Utility function
	{
		return (x % m + m) % m;
	}


	void Update()
	{
		if (Input.touchCount > 0)//if there's at least one touch
		{
			Touch touch = Input.GetTouch(0);//select the first one
			
			if (touch.phase == TouchPhase.Began)//if this is the start of the touch
			{
				//cast a ray from the camera and set the state "rotating" to True if the ray doesn't hit anything. 
				RaycastHit hit;
				rotating = !Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit);
			}
			if (touch.phase == TouchPhase.Moved && this.rotating)//if this is during the touch and the touch moved
			{
				//then compute the movement on both axis of the screen
				float horizontalMov = sensitivity * touch.deltaPosition.x * touch.deltaTime;
				float verticalMov = sensitivity * touch.deltaPosition.y * touch.deltaTime;

				//recalculate the position of the camera
				this.transform.position = target.position;
				this.transform.Rotate(Vector3.right, -verticalMov);
				this.transform.Rotate(Vector3.up, horizontalMov, Space.World);
				this.transform.Translate(new Vector3(0, 0, -10));
			}
		}
	}
}
