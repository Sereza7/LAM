using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
	//Snaps objects to a 3D discrete grid
{
    public Vector3 gridSize = new Vector3(0.5f, 0.5f, 0.5f); //the size of one voxel.


	void Update()
	{
		if (this.GetComponent<Rigidbody>().velocity == Vector3.zero) //Only snap when the object is moving
		{
			var newPosition = new Vector3(
			   Mathf.Round(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
			   Mathf.Round(this.transform.position.y / this.gridSize.y) * this.gridSize.y,
			   Mathf.Round(this.transform.position.z / this.gridSize.z) * this.gridSize.z
				);
			this.transform.position = newPosition;
		}
	}
}
