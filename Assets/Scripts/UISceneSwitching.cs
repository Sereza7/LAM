using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneSwitching : MonoBehaviour
{
	public string sceneName;
	public async void switchScene()
	{
		await Task.Delay(System.TimeSpan.FromSeconds(0.25f));
		SceneManager.LoadSceneAsync(sceneName);
	}

}
