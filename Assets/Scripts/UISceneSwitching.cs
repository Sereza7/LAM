using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneSwitching : MonoBehaviour
{
	public string sceneName;
	public void switchScene()
	{
		SceneManager.LoadSceneAsync(sceneName);
	}
}
