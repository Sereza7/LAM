using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneSwitching : MonoBehaviour
	//Utility class for UI, function called to change scenes.
{
	public string sceneName;//Name of the next scene

	public async void switchScene()
	{
		await Task.Delay(System.TimeSpan.FromSeconds(0.25f));//Gives time for the button sound to play, and avoids an 'instantaneous' interface.
		SceneManager.LoadSceneAsync(sceneName);
	}

}
