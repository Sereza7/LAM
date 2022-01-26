using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
	//Manages touch inputs in the wooden puzzle.
{

    GameObject targetPiece = null;
    Vector3 mO;

	//movement processing
	private float length;
	private Vector3 plane;
	float speed = 10;
	float maxSpeed = 15;

	//Sound effects 
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
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))//Initialize piece movement if hit
			{

				if (!GameObject.ReferenceEquals(target.gameObject, hit.transform.gameObject))
				{
					targetPiece = hit.transform.gameObject;
					targetPiece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
					targetPiece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

					length = Vector3.Distance(Camera.main.transform.position, targetPiece.transform.position);

					//Select the movement plane according to the camera direction
					plane = Vector3.zero;
					Vector3 direction = Camera.main.transform.position - targetPiece.transform.position;
					if (Mathf.Abs(direction.x)>=Mathf.Abs(direction.y) && Mathf.Abs(direction.x) >= Mathf.Abs(direction.z)) { plane = new Vector3(0f, 1f, 1f); }
					else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x) && Mathf.Abs(direction.y) >= Mathf.Abs(direction.z)) { plane = new Vector3(1f, 0f, 1f); }
					else { plane = new Vector3(1f, 1f, 0f); }
				}
			}
			else //Camera rotation
			{
				previousPosition = Input.mousePosition;
			}
		}

		//Move the Object if selected
		else if (Input.GetMouseButton(0) && targetPiece)
		{
			Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 rayCastPosition = mRay.GetPoint(length);
			rayCastPosition = new Vector3(Mathf.Round(rayCastPosition.x / this.gridSize.x) * this.gridSize.x,
										Mathf.Round(rayCastPosition.y / this.gridSize.y) * this.gridSize.y,
										Mathf.Round(rayCastPosition.z / this.gridSize.z) * this.gridSize.z);
			// calc velocity necessary to follow the mouse pointer
			var vel = (rayCastPosition - targetPiece.transform.position);
			vel.Scale(plane);
			vel *= speed;
			// limit max velocity to avoid pass through objects
			if (vel.magnitude > maxSpeed) vel *= maxSpeed / vel.magnitude;

			// set object velocity

			targetPiece.GetComponent<Rigidbody>().velocity = vel;
		}
		//Release the Object if screen untouched
		else if (Input.GetMouseButtonUp(0) && targetPiece)
		{
			var snapPos = new Vector3(
			Mathf.Round(targetPiece.transform.position.x / this.gridSize.x) * this.gridSize.x,
			Mathf.Round(targetPiece.transform.position.y / this.gridSize.y) * this.gridSize.y,
			Mathf.Round(targetPiece.transform.position.z / this.gridSize.z) * this.gridSize.z
			);

			targetPiece.transform.position = snapPos;

			targetPiece.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			targetPiece.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
			targetPiece = null;
			//play sound on release of the piece
			AudioClip randomAudio = audioFiles[Random.Range(0, audioFiles.GetLength(0) - 1)];
			audioSource.clip = randomAudio;
			audioSource.volume = Random.Range(minVol, maxVol);
			audioSource.Play();
		}
		//Move the camera around a position
		else if (Input.GetMouseButton(0) && !targetPiece )
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
