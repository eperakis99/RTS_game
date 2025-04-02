using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit_Movement : MonoBehaviour
{

    //cursor position
    private Vector3 mousePos;
    private Vector3 playerPos; 
    public Vector3 velocity;

    private Ray eyeRay;

    private float elapsedTime = 0f;
    private float duration = 2f;

    private bool first_click_flag = false;

    //public NavMeshAgent agent;
    public Mesh personalSpace; //Every unit needs its personal space, just like people
    private MeshCollider personalSpaceCollider; //Used to trigger an event when a unit touches another unit
    private static MeshGenerator generator;

    Unit unit;
    [SerializeField] private Camera mainCamera;




    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;

        mousePos = Vector3.zero;
        playerPos = transform.position;
        Debug.Log("player position:"+playerPos);
        velocity = Vector3.zero;

        //agent = this.GetComponent<NavMeshAgent>();
        generator = new MeshGenerator(32);
        personalSpace = generator.generateCircle();
        //Debug.Log(personalSpace);


        foreach (var meshFilter in this.GetComponentsInChildren<MeshFilter>(true))
        {
            if (meshFilter.name.Equals("PersonalSpace"))
            {
                meshFilter.mesh = personalSpace;
                
            }
        }

        foreach (var meshCollider in this.GetComponentsInChildren<MeshCollider>(true))
        {
            if (meshCollider.name.Equals("PersonalSpace"))
            {
                meshCollider.sharedMesh = personalSpace;
                personalSpaceCollider = meshCollider;


            }
        }

        
        unit = this.GetComponent<Unit>();


    }

    // Update is called once per frame
    void Update()
    {
        float percentageComplete = elapsedTime / duration;
        if (percentageComplete <= 1)
        {
            elapsedTime += Time.deltaTime;
        }
            
        

        if (Input.GetButtonDown("Fire2"))
        {
            first_click_flag=true;

            eyeRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(eyeRay, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Environment")) && unit.selected)
            {
                unit.selected = false;
                unit.toggleSelectedVisual();
                Debug.Log("MOVING TO POSITION");
                mousePos = hitInfo.point;
                moveAgent(hitInfo.point);
            }
            
           //Debug.Log(mousePos);


            playerPos = GetComponent<Transform>().position;
            //Debug.Log(playerPos);

            velocity = mousePos - playerPos;
            //Debug.Log(velocity.normalized);

            elapsedTime = 0f;

            
        }

        /*if (first_click_flag && !reachedTarget())
        {
            //transform.position = Vector3.Lerp(playerPos, mousePos, percentageComplete);
            moveAgent(mousePos);
        }*/
            


    }

    private void moveAgent(Vector3 pos)
    {
        unit.agent.SetDestination(pos);
    }

    private bool reachedTarget()
    {
        Vector3 currentDist = mousePos - GetComponent<Transform>().position;
        //Debug.Log(currentDist.magnitude);
        if (currentDist.magnitude < 0.5f)
        {
            personalSpaceCollider.isTrigger = true;
            return true;
        }    
        else
        {
            personalSpaceCollider.isTrigger = false;
            return false;
        }
            
    }



}
