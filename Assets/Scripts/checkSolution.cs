using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSolution : MonoBehaviour
{
	public Transform puzzle;
	public Material unsolvedMaterial;
	public Material solvedMaterial;
	public int nameDistinction = -1;

	private float deltaTime = 0.5f;
	private float nextUpdate = 0;
	private int solutionFound = 0;
	private float solvedTime = -1f;

	
	void Update()
    {
		if (Time.time >= nextUpdate)
		{
			nextUpdate = Time.time + deltaTime;
			foreach (Transform child in transform)
			{
				string name = child.name;
				if (nameDistinction > -1 && name.Length > nameDistinction)
				{
					name = name.Substring(0,nameDistinction);
				}
				Vector3 position = child.position;
				Material material = child.GetComponent<MeshRenderer>().material;
				bool occupied = false;
				foreach (Transform child2 in puzzle)
				{
					string name2 = child2.name;
					if (nameDistinction > -1 && name2.Length> nameDistinction)
					{
						name2 = name2.Substring(0, nameDistinction);
					}


					if (name2 == name && child2.gameObject.activeSelf && !occupied
						&& Vector3.Distance(child2.position, position) < child.lossyScale.x/8)
					{
						occupied = true;
						if (material.color != solvedMaterial.color) {
							child.GetComponent<MeshRenderer>().material = solvedMaterial;
							solutionFound++;
							Debug.Log(solutionFound.ToString()+"/"+ transform.childCount.ToString());
						}
					}
				}
				if (!occupied && material.color != unsolvedMaterial.color) {
					child.GetComponent<MeshRenderer>().material = unsolvedMaterial;
					solutionFound--;
				}
			}

			if (solutionFound == transform.childCount &&  solvedTime==-1f)
			{
				solvedTime = Time.time;
			}
			if (solutionFound < transform.childCount && solvedTime != -1f)
			{
				solvedTime = -1f;
			}
			if (solutionFound == transform.childCount && solvedTime!=-1f && Time.time - solvedTime > 1f)
			{
				this.SolutionFound();
				//dirty way to avoid triggering the event twice
				solutionFound += 2;
			}
		}
	}
	private void SolutionFound()
	{
		GameObject.Find("Canvas").transform.Find("SolutionFound").gameObject.SetActive(true);
		GameObject.Find("SolutionNotFound").SetActive(false);
	}
}
