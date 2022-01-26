using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
	//Manages sound for every marble.
{
	public AudioClip[] audioFiles; // files to be played on collision
	

	private float minVol = 0.01f; // min&max volumes of the sounds played. Between 0 and 1
	private float maxVol = 0.5f;

	private AudioSource audioSource; //the audio source associated to a marble
	
	void Start()
    {
		
        if (this.transform.GetComponent<AudioSource>()) //Get the audio source from the object (shouldn't happen, but just to be sure there's no duplication)
		{
			this.audioSource = this.transform.GetComponent<AudioSource>();
			this.audioSource.mute = !OptionManager.activeSounds; 
		}
		else //create and initialize an audiosource if not already there
		{
			this.audioSource = this.gameObject.AddComponent<AudioSource>();
			this.audioSource.mute = !OptionManager.activeSounds; //Since this audiosource is created after loading the scene, it wouldn't be muted by options
		}
    }
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.impulse.magnitude > 0.01f && !audioSource.isPlaying) //If the collision is strong enough and the audio source is not already playing a clip, then play one
		{
			//pick a random audio clip from the parameter list
			AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
			audioSource.clip = randomAudio;

			//Adjust the sound volume depending on the collision 'strength'= collision.impulse.magnitude
			audioSource.volume = this.minVol + (Mathf.Log(1 + collision.impulse.magnitude) / Mathf.Log(2 + collision.impulse.magnitude))*(this.maxVol-this.minVol);

			//Play sound
			audioSource.Play();
		}
	}
}
