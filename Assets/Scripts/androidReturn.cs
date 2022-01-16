using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class androidReturn : MonoBehaviour
{
	public string lastSceneName;
	public GameObject quitPrompt;

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey(KeyCode.Escape))
		{
			try
			{
				switchScene();
			}
			catch
			{
				quitPrompt.SetActive(!quitPrompt.activeSelf);
			}
		}

	}
	public async void switchScene()
	{
		await Task.Delay(System.TimeSpan.FromSeconds(0.25f));
		SceneManager.LoadSceneAsync(this.lastSceneName);
	}
}
