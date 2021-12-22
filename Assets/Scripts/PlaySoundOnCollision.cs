using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
	private AudioSource audioSource;
	public AudioClip[] audioFiles;

	private float minVol = 0.01f;
	private float maxVol = 0.5f;

	// Start is called before the first frame update
	void Start()
    {
        if (this.transform.GetComponent<AudioSource>())
		{
			this.audioSource = this.transform.GetComponent<AudioSource>();
		}
		else
		{
			this.audioSource = this.gameObject.AddComponent<AudioSource>();
		}
    }
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.impulse.magnitude > 0.01f && !audioSource.isPlaying)
		{
			AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
			audioSource.clip = randomAudio;
			audioSource.volume = this.minVol + (Mathf.Log(1 + collision.impulse.magnitude) / Mathf.Log(2 + collision.impulse.magnitude))*(this.maxVol-this.minVol);
			audioSource.Play();
		}
	}
}
