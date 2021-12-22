using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UIQuit : MonoBehaviour
{
	public async void Quit()
	{
		await Task.Delay(System.TimeSpan.FromSeconds(0.25f));
		Application.Quit();
	}
}
