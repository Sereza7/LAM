using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUnload : MonoBehaviour
	//Keeps the object alive through scene transitions.
	//Used on one object instanciated in the MenuStart scene
{
	
    void Awake()
    {
		GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag("noUnload");
		if (sceneObjects.Length > 1) //Make sure there's no multiple instances of these objects
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
    }
	
}
