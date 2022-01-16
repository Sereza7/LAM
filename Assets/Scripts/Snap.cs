using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    private Vector3 gridSize = new Vector3(0.5f, 0.5f, 0.5f);


	void Update()
	{
        SnapToGrid();

    }

    private void SnapToGrid()
    {
		if (this.GetComponent<Rigidbody>().velocity==Vector3.zero)
		{
			var position = new Vector3(
			   Mathf.Round(this.transform.position.x / this.gridSize.x) * this.gridSize.x,
			   Mathf.Round(this.transform.position.y / this.gridSize.y) * this.gridSize.y,
			   Mathf.Round(this.transform.position.z / this.gridSize.z) * this.gridSize.z
				);
			this.transform.position = position;
		}
        

        
    }
}
