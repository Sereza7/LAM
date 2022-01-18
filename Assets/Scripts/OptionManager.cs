using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
	public bool startActiveBGM;
	public bool startActiveSounds;
	public bool startActiveGyro;
	public float startSensitivity;


	public static bool activeBGM;
	public static bool activeSounds;
	public static bool activeGyro;
	public static float sensitivity;

	// Start is called before the first frame update
	void Start()
    {
		//Initialize static variables
		activeBGM = startActiveBGM;
		activeSounds = startActiveSounds;
		activeGyro = startActiveGyro;
		sensitivity = startSensitivity;

		//Add the listener to the scenemanager event
		SceneManager.sceneLoaded += this.changeOptionsOnLoad;

	}
	void changeOptionsOnLoad(Scene scene, LoadSceneMode mode)
	{
		
		//Change the option scene accordingly when loaded
		if (scene.name == "MenuOptions")
		{
			GameObject.Find("ToggleBGM").GetComponent<Toggle>().isOn=OptionManager.activeBGM;
			GameObject.Find("ToggleSounds").GetComponent<Toggle>().isOn = OptionManager.activeSounds;
			GameObject.Find("ToggleGyro").GetComponent<Toggle>().isOn = OptionManager.activeGyro;
			GameObject.Find("SensitivitySlider").GetComponent<Slider>().value = Mathf.Log(OptionManager.sensitivity);
		}

		//Apply the different options to the scene.
		//Those two lines could be run only once when getting out of the option menu.
		CubeController.sensitivity = OptionManager.sensitivity;
		CubeController.useGyro = OptionManager.activeGyro;

		//Those lines need to be run at every scene
		AudioSource[] sources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		for (int index = 0; index < sources.Length; ++index)
		{
			sources[index].mute = !OptionManager.activeSounds;
		}

		this.gameObject.GetComponent<AudioSource>().mute = !OptionManager.activeBGM;
	}

	public static void updateActiveBGM(Boolean b) { activeBGM = b; }
	public static void updateActiveSounds(Boolean b) { activeSounds = b; }
	public static void updateActiveGyro(Boolean b) { activeGyro = b; }
	public static void updateSensitivity(float f) { sensitivity = Mathf.Exp(f); }
}
