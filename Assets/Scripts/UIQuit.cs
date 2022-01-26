using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UIQuit : MonoBehaviour
	//Utility class for UI, function called to quit the app.
{
	public async void Quit()
	{
		await Task.Delay(System.TimeSpan.FromSeconds(0.25f));
		Application.Quit();
	}
}
