using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
	//Utility script for displaying a 3D object by randomish smooth and variable rotations.
{
	
	private float speed = 50f;
	private float pulseX;
	private float pulseY;
	private float pulseZ;
	private int tick = 0;
	private float shiftX;
	private float shiftY;
	private float shiftZ;

	private void Start()
	{
		pulseX = Random.Range(1/speed, 5/speed);
		pulseY = Random.Range(1 / speed, 5 / speed);
		pulseZ = Random.Range(1 / speed, 5 / speed);
		shiftX = Random.Range(0, 6.28f); // 2*pi
		shiftY = Random.Range(0, 6.28f);
		shiftZ = Random.Range(0, 6.28f);
	}

	void Update()
	{
		tick++;
		transform.Rotate(Time.deltaTime * speed * Mathf.Sin(shiftX + 0.01f * tick * pulseX), 0, 0, Space.Self);
		transform.Rotate(0, Time.deltaTime * speed * Mathf.Sin(shiftY + 0.01f * tick * pulseY), 0, Space.Self);
		transform.Rotate(0, 0, Time.deltaTime * speed * Mathf.Sin(shiftZ + 0.01f * tick * pulseZ), Space.Self);
	}
}