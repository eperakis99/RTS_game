using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{

    public int velocity = 100;
    private bool mouseIsMovingHorizontally = false;
    private bool mouseIsMovingVertically = false;
    private Vector3 cameraForward = Vector3.zero;
    private float cameraAngle = 0.0f;
    public float zoom = 5f;
    private float zoomDelta = 0.1f;
    private Vector3 pos;

    public float maxZoom = 10.0f; //min y distance from scene 
    public float minZoom = 40.0f; //max y distance from scene 
    public float maxBoundaryX = 120f; //The camera should move within these limits
    public float minBoundaryX = -100f;
    public float maxBoundaryZ = 140f; 
    public float minBoundaryZ = -80f;

    [SerializeField] GameObject worldPlane;

    // Start is called before the first frame update
    void Start()
    {
        cameraForward = transform.forward;
        cameraAngle = -transform.localEulerAngles.y;

        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        /*if (Input.GetMouseButton(0))
        {
            //Debug.Log(mouseIsMovingHorizontally);
            //Debug.Log(Vector3.ProjectOnPlane(cameraForward, Vector3.up));

            

            if (Input.GetAxis("Mouse X") > 0 && (Mathf.Abs(Input.GetAxis("Mouse Y")) < Mathf.Abs(Input.GetAxis("Mouse X")))  && !mouseIsMovingVertically)
            {
                transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * velocity);
                //Debug.Log(cameraAngle);
            }
                
            else if (Input.GetAxis("Mouse X") < 0 && (Mathf.Abs(Input.GetAxis("Mouse Y")) < Mathf.Abs(Input.GetAxis("Mouse X")))  && !mouseIsMovingVertically)
            {
                transform.RotateAround(transform.position, Vector3.up, -(Time.deltaTime * velocity));
                mouseIsMovingHorizontally = true;
                //Debug.Log(cameraAngle);
            }


            if (Input.GetAxis("Mouse Y") > 0 && (Mathf.Abs(Input.GetAxis("Mouse Y")) > Mathf.Abs(Input.GetAxis("Mouse X"))) && !mouseIsMovingHorizontally)
            {
                mouseIsMovingVertically=true;
                transform.Translate(Vector3.ProjectOnPlane(transform.forward, Vector3.up) * Time.deltaTime * velocity, Space.World) ;
                //Debug.Log("rotated up");
            }

            else if (Input.GetAxis("Mouse Y") < 0 && (Mathf.Abs(Input.GetAxis("Mouse Y")) > Mathf.Abs(Input.GetAxis("Mouse X"))) && !mouseIsMovingHorizontally)
            {
                mouseIsMovingVertically = true;
                transform.Translate(-(Vector3.ProjectOnPlane(transform.forward, Vector3.up) * Time.deltaTime * velocity), Space.World);
                //Debug.Log("rotated down");
            }

            pos = transform.position;
            pos.x = Mathf.Clamp(transform.position.x, minBoundary, maxBoundary);
            pos.z = Mathf.Clamp(transform.position.z, minBoundary, maxBoundary);
            transform.position = pos;

        }
        else
        {
            //Debug.Log(mouseIsMovingHorizontally);
            mouseIsMovingHorizontally = false;
            mouseIsMovingVertically = false;
        }*/


        if(Input.mousePosition.y >= Screen.height - 10)
        {
            transform.Translate(Vector3.ProjectOnPlane(transform.forward, Vector3.up) * Time.smoothDeltaTime * velocity, Space.World);
        }
        else if(Input.mousePosition.y <= 10)
        {
            transform.Translate(-(Vector3.ProjectOnPlane(transform.forward, Vector3.up) * Time.smoothDeltaTime * velocity), Space.World);
        }
        else if(Input.mousePosition.x >= Screen.width - 10)
        {
            transform.Translate(transform.right * Time.smoothDeltaTime * velocity, Space.World);
        }
        else if(Input.mousePosition.x <= 10)
        {
            transform.Translate(-(transform.right * Time.smoothDeltaTime * velocity), Space.World);
        }

        pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, minBoundaryX, maxBoundaryX);
        pos.z = Mathf.Clamp(transform.position.z, minBoundaryZ, maxBoundaryZ);
        transform.position = pos;

        handleZoom();
    }


    
    private void handleZoom()
    {

        if (Input.mouseScrollDelta.y > 0)
        {
            if (transform.position.y > maxZoom)
            {
                zoom += zoomDelta * velocity * Time.smoothDeltaTime;
                transform.Translate(transform.forward * zoom, Space.World);
            }
                
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            if (transform.position.y < minZoom)
            {
                zoom -= zoomDelta * velocity * Time.smoothDeltaTime;
                transform.Translate(-transform.forward * zoom, Space.World);
            }
                
        }

    }

}
