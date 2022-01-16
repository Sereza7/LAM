using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
	public bool startActiveBGM;
	public bool startActiveSounds;
	
	
	public static bool activeBGM;
	public static bool activeSounds;
    // Start is called before the first frame update
    void Start()
    {
		activeBGM = startActiveBGM;
		activeSounds = startActiveSounds;
		SceneManager.sceneLoaded += this.changeOptionsOnLoad;

	}
	void changeOptionsOnLoad(Scene scene, LoadSceneMode mode)
	{
		AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		for (int index = 0; index < sources.Length; ++index)
		{
			sources[index].mute = !OptionManager.activeSounds;
		}

		this.gameObject.GetComponent<AudioSource>().mute = !OptionManager.activeBGM;
	}

	public static void updateActiveBGM(Boolean b) { activeBGM = b; }
	public static void updateActiveSounds(Boolean b) { activeSounds = b; }
}
