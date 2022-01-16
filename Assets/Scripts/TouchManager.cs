﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    GameObject gObj = null;
    Vector3 mO;
    Plane objPlane;

	private AudioSource audioSource;
	public AudioClip[] audioFiles;
	private float minVol = 0.3f;
	private float maxVol = 0.4f;

	//Camera rotation
	[SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 previousPosition;

    Ray GenerateMouseRay()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF-mousePosN);
        return mr;
    }

	private void Start()
	{
		//initialize audio
		if (this.transform.GetComponent<AudioSource>())
		{
			this.audioSource = this.transform.GetComponent<AudioSource>();
		}
		else
		{
			this.audioSource = this.gameObject.AddComponent<AudioSource>();
		}
		this.audioSource.mute = OptionManager.activeSounds;
	}

	void Update()
    {
        //Select the Object to move
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = GenerateMouseRay();
            RaycastHit hit;
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
            {
                gObj = hit.transform.gameObject;

                objPlane = new Plane(Camera.main.transform.forward*-1, gObj.transform.position);

                //Calculate mouse offset to smooth the movement
                Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                objPlane.Raycast(mRay, out rayDistance); //Determine where the mouse is hitting the plane
                mO = gObj.transform.position - mRay.GetPoint(rayDistance);
				//play sound
				AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
				audioSource.clip = randomAudio;
				audioSource.volume = Random.Range(minVol,maxVol);
				audioSource.Play();

			}
            else //camera rotation
            {
                previousPosition = Input.mousePosition;
            }
        }
        //Move the Object if selected
        else if (Input.GetMouseButton(0) && gObj)
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
                gObj.transform.position = mRay.GetPoint(rayDistance) + mO;
		}
        //Release the Object if screen untouched
        else if (Input.GetMouseButtonUp(0) && gObj)
        {
            gObj = null;
        }
        //Move the camera around a position
        else if (Input.GetMouseButton(0) && !gObj)
        {
            Vector3 direction = -(previousPosition - Input.mousePosition)/10;
			foreach(Camera cam in Camera.allCameras)
			{
				cam.transform.position = target.position;//new Vector3();
				cam.transform.Rotate(Vector3.right, -direction.y );
				cam.transform.Rotate(Vector3.up, direction.x , Space.World);
				cam.transform.Translate(new Vector3(0, 0, -10));
			}
            

            previousPosition = Input.mousePosition;
        }
    }
}
