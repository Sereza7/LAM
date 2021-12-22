using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoUnload : MonoBehaviour
{
    void Awake()
    {
		GameObject[] sceneObjects = GameObject.FindGameObjectsWithTag("noUnload");
		if (sceneObjects.Length > 1)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
    }
	
}
