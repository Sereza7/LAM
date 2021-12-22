using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSolution : MonoBehaviour
{
	public Transform puzzle;
	public Material unsolvedMaterial;
	public Material solvedMaterial;
	public GameObject solutionLabel;

	private float deltaTime = 0.5f;
	private float nextUpdate = 0;
	private int solutionFound = 0;

	
	void Update()
    {
		if (Time.time >= nextUpdate)
		{
			nextUpdate = Time.time + deltaTime;
			foreach (Transform child in transform)
			{
				string name = child.name;
				Vector3 position = child.position;
				Material material = child.GetComponent<MeshRenderer>().material;
				foreach (Transform child2 in puzzle)
				{
					if (child2.name == name && child2.gameObject.activeSelf
						&& Vector3.Distance(child2.position, position) < 0.025
						&& material.color != solvedMaterial.color)
					{
						child.GetComponent<MeshRenderer>().material = solvedMaterial;
						solutionFound++;
					}
					if (child2.name == name && child2.gameObject.activeSelf
						&& Vector3.Distance(child2.position, position) > 0.025
						&& material.color != unsolvedMaterial.color)
					{
						child.GetComponent<MeshRenderer>().material = unsolvedMaterial;
						solutionFound--;
					}
				}
			}
			if (solutionFound == puzzle.childCount)
			{
				this.SolutionFound();
				//dirty way to avoid triggering the event twice
				solutionFound += puzzle.childCount+2;
			}
		}
	}
	private void SolutionFound()
	{
		Debug.Log("Solved puzzle.");
		solutionLabel.GetComponent<TMPro.TMP_Text>().text="Congratulations!\nYou found the solution.";
	}
}
