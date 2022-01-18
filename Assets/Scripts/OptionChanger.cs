using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionChanger : MonoBehaviour
{
	AudioSource BGMmanager = null;
	AudioSource Soundmanager = null;
	void Start()
	{
		if (GameObject.Find("GlobalTracker") != null)
		{
			BGMmanager = GameObject.Find("GlobalTracker").GetComponent<AudioSource>();
		}
		if (GameObject.Find("Canvas") != null)
		{
			Soundmanager = GameObject.Find("Canvas").GetComponent<AudioSource>();
		}
	}

	public void updateActiveBGM(bool b) {
		OptionManager.updateActiveBGM(b);
		if (BGMmanager != null)
		{
			BGMmanager.mute= !b;
		}
	}
	public void updateActiveSounds(bool b) {
		OptionManager.updateActiveSounds(b);
		if (Soundmanager != null)
		{
			Soundmanager.mute = !b;
		}
	}
	public void updateActiveGyro(bool b)
	{
		OptionManager.updateActiveGyro(b);
	}

	public void updateSensitivity(float f)
	{
		OptionManager.updateSensitivity(f);
	}
}
