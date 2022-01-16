using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchManager2 : MonoBehaviour
{
    GameObject gObj = null;
    Vector3 mO;
    Plane objPlane;

    float length;
    float speed = 10;
    float maxSpeed = 15;

    //Camera rotation
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    private Vector3 previousPosition;

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

    void Update()
    {
        //Zoom with pinch on mobile
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        
        //Select the Object to move
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = GenerateMouseRay();
            RaycastHit hit;
			if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
            {
                
                if(!GameObject.ReferenceEquals( target.gameObject, hit.transform.gameObject))
                {
                    gObj = hit.transform.gameObject;
                    gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                    length = hit.distance;
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
            // calc velocity necessary to follow the mouse pointer
            var vel = (mRay.GetPoint(length) - gObj.transform.position) * speed; 
            // limit max velocity to avoid pass through objects
            if (vel.magnitude > maxSpeed) vel *= maxSpeed / vel.magnitude;
            // set object velocity
            gObj.GetComponent<Rigidbody>().velocity = vel;
        
            
            
            //Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            //float rayDistance;
            //if (objPlane.Raycast(mRay, out rayDistance))
            //    gObj.transform.position = mRay.GetPoint(rayDistance) + mO;
		}
        //Release the Object if screen untouched
        else if (Input.GetMouseButtonUp(0) && gObj)
        {
            gObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gObj.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            gObj = null;
        }
        //Move the camera around a position
        else if (Input.GetMouseButton(0) && !gObj)
        {
            Vector3 direction = -(previousPosition - Input.mousePosition)/10;
			foreach(Camera cam in Camera.allCameras)
			{
				cam.transform.position = target.position;//new Vector3();
				cam.transform.Rotate(new Vector3(1, 0, 0), -direction.y );
				cam.transform.Rotate(new Vector3(0, 1, 0), direction.x , Space.World);
				cam.transform.Translate(new Vector3(0, 0, -10));
			}
            previousPosition = Input.mousePosition;
        }
    }
}
