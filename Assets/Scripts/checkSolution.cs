using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSolution : MonoBehaviour
{
	//This script handles detection of the solution configuration for puzzles. Should be attached to a "solution" object 
	//which contains as transform children all the objects to check, with the same names as the puzzle object it's comparing it to.


	public Transform puzzle; //puzzle to compare this solution with
	public Material unsolvedMaterial; //material for the solution shadow when unsolved
	public Material solvedMaterial;//material for the solution shadow when solved
	public int nameDistinction = -1;//if set to -1, ignore. Else, when set to i>0 it allows to consider only the first i letters in the object name instead of the full name when comparing
									//useful when several objects are interchangeable (smallball0 can go where smallball1 is supposed to go and still be right)

	private float deltaTime = 0.5f;//time between two updates of the solution
	private float nextUpdate = 0; //time for the next update
	private int solutionFound = 0; //number of pieces found in the solution. This number is compared to the amount of children in the solution to see if the puzzle is solved.
	private float solvedTime = -1f; //Time since the puzzle was fully solved, useful to pop the interface after a second or two of stability.

	
	void Update()
    {
		//limit updates because quite expensive and not necessary to keep up to date
		if (Time.time >= nextUpdate)
		{
			nextUpdate = Time.time + deltaTime;
			//the children of this object are the solution parts.
			foreach (Transform child in transform)
			{
				string name = child.name;
				if (nameDistinction > -1 && name.Length > nameDistinction) //reduce the name if needed
				{
					name = name.Substring(0,nameDistinction);
				}
				Vector3 position = child.position;
				Material material = child.GetComponent<MeshRenderer>().material;
				bool occupied = false; //avoid tagging the same solution object as correct multiple times
				//check for every child of puzzle.
				foreach (Transform child2 in puzzle)
				{
					string name2 = child2.name;
					if (nameDistinction > -1 && name2.Length> nameDistinction)
					{
						name2 = name2.Substring(0, nameDistinction);
					}


					if (name2 == name && child2.gameObject.activeSelf && !occupied
						&& Vector3.Distance(child2.position, position) < child.lossyScale.x/8)//If any is in the same position and has a similar name
					{
						occupied = true;
						//change material and update solutionFound
						if (material.color != solvedMaterial.color) {
							child.GetComponent<MeshRenderer>().material = solvedMaterial;
							solutionFound++;
							Debug.Log(solutionFound.ToString()+"/"+ transform.childCount.ToString());
						}
					}
				}
				if (!occupied && material.color != unsolvedMaterial.color) {//when a solution is not occupied anymore
					child.GetComponent<MeshRenderer>().material = unsolvedMaterial;
					solutionFound--;
				}
			}

			// manage the full solution detection
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

	private void SolutionFound() //called when solution is found.
	{
		GameObject.Find("Canvas").transform.Find("SolutionFound").gameObject.SetActive(true);
		GameObject.Find("SolutionNotFound").SetActive(false);
	}
}
