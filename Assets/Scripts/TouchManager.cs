using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    GameObject gObj = null;
    Vector3 mO;
    Plane objPlane;

	private float length;
	private Vector3 plane;
	float speed = 10;
	float maxSpeed = 15;

	private AudioSource audioSource;
	public AudioClip[] audioFiles;
	private float minVol = 0.3f;
	private float maxVol = 0.4f;

	//Camera rotation
	[SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 previousPosition;

	private Vector3 gridSize = new Vector3(0.5f, 0.5f, 0.5f);

	//Zoom
	float zoomOutMin = 3f;
	float zoomOutMax = 10f;
	void zoom(float increment)
	{
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
	}

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
		//Zoom with pinch on mobile
		if (Input.touchCount >= 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			zoom(difference * 0.01f);
			if(Input.GetTouch(0).phase == TouchPhase.Ended) { previousPosition = Input.GetTouch(1).position; }
			if (Input.GetTouch(1).phase == TouchPhase.Ended) { previousPosition = Input.GetTouch(0).position; }
		}
		//Select the Object to move
		else if (Input.GetMouseButtonDown(0))
		{
			Ray mouseRay = GenerateMouseRay();
			RaycastHit hit;
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
			{

				if (!GameObject.ReferenceEquals(target.gameObject, hit.transform.gameObject))
				{
					gObj = hit.transform.gameObject;
					gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

					length = Vector3.Distance(Camera.main.transform.position, gObj.transform.position);
					plane = Vector3.zero;
					Vector3 direction = Camera.main.transform.position - gObj.transform.position;
					if (Mathf.Abs(direction.x)>=Mathf.Abs(direction.y) && Mathf.Abs(direction.x) >= Mathf.Abs(direction.z)) { plane = new Vector3(0f, 1f, 1f); }
					else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) >= Mathf.Abs(direction.z)) { plane = new Vector3(1f, 0f, 1f); }
					else { plane = new Vector3(1f, 1f, 0f); }
				}
				//objPlane = new Plane(Camera.main.transform.forward*-1, gObj.transform.position);
				//
				////Calculate mouse offset to smooth the movement
				//Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				//float rayDistance;
				//objPlane.Raycast(mRay, out rayDistance); //Determine where the mouse is hitting the plane
				//mO = gObj.transform.position - mRay.GetPoint(rayDistance);
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
			Vector3 rayCastPosition = mRay.GetPoint(length);
			rayCastPosition = new Vector3(Mathf.Round(rayCastPosition.x / this.gridSize.x) * this.gridSize.x,
										Mathf.Round(rayCastPosition.y / this.gridSize.y) * this.gridSize.y,
										Mathf.Round(rayCastPosition.z / this.gridSize.z) * this.gridSize.z);
			// calc velocity necessary to follow the mouse pointer
			var vel = (rayCastPosition - gObj.transform.position);
			vel.Scale(plane);
			vel *= speed;
			// limit max velocity to avoid pass through objects
			if (vel.magnitude > maxSpeed) vel *= maxSpeed / vel.magnitude;

			// set object velocity

			gObj.GetComponent<Rigidbody>().velocity = vel;
		}
		//Release the Object if screen untouched
		else if (Input.GetMouseButtonUp(0) && gObj)
		{
			var snapPos = new Vector3(
			Mathf.Round(gObj.transform.position.x / this.gridSize.x) * this.gridSize.x,
			Mathf.Round(gObj.transform.position.y / this.gridSize.y) * this.gridSize.y,
			Mathf.Round(gObj.transform.position.z / this.gridSize.z) * this.gridSize.z
			);

			gObj.transform.position = snapPos;

			gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			gObj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			gObj = null;
			//play sound
			AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
			audioSource.clip = randomAudio;
			audioSource.volume = Random.Range(minVol, maxVol);
			audioSource.Play();
		}
		//Move the camera around a position
		else if (Input.GetMouseButton(0) && !gObj )
		{
			Vector3 direction = -(previousPosition - Input.mousePosition) / 10;
			foreach (Camera cam in Camera.allCameras)
			{
				cam.transform.position = target.position;//new Vector3();
				cam.transform.Rotate(new Vector3(1, 0, 0), -direction.y);
				cam.transform.Rotate(new Vector3(0, 1, 0), direction.x, Space.World);
				cam.transform.Translate(new Vector3(0, 0, -10));
			}
			previousPosition = Input.mousePosition;
		}
	}
}
