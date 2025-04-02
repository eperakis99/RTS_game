using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building
{

    public Rigidbody bulletPrefab;
    public GameObject turretHead;
    private bool shooting;
    public GameObject target;


    private int productionTime;
    //public GameObject healthBar;
    GameObject createUnitButton;

    public BuildingData buildingData;

    public Material redMaterial;
    public Material greenMaterial;



    private void Awake()
    {
        productionTime = 10;
        isOperational = false;
        isBusy = false;
        this.maxHp = 500;
        this.hp = maxHp;

    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("A UNIT ENTERED ENEMY");

        if (other.gameObject.CompareTag("Enemy"))
        {
            target = other.gameObject;
            
        }



    }

    private IEnumerator shoot(float delay)
    {
        while (shooting)
        {
            Rigidbody rb;
            rb = Instantiate(bulletPrefab, this.transform.position + new Vector3(0f, 2f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;
            rb.AddForce(this.transform.forward * 200f, ForceMode.Force);
            yield return new WaitForSeconds(delay);
            shooting = false;
        }
    }


    private void Start()
    {
        turretHead = GameObject.FindGameObjectWithTag("Spawner");
        //bulletPrefab.TryGetComponent<Rigidbody>(out bulletRb);
    }

    private void Update()
    {
        RaycastHit hitPoint;

        if (Physics.Raycast(this.clone.transform.position, Vector3.down, out hitPoint, 60f, LayerMask.GetMask("Environment")))
        {
            Debug.Log("Raycast Hit at" + hitPoint.point);
            if (hitPoint.point.y > 1f)
            {
                selectedArea.GetComponent<Renderer>().material = redMaterial;
            }
            else
            {
                selectedArea.GetComponent<Renderer>().material = greenMaterial;
            }


            if (Input.GetMouseButtonDown(0) && hitPoint.point.y <= 1f && !isOperational && !isBusy)
            {
                placeTurretOnGround();
            }

        }


        if (!shooting && target != null)
        {
            shooting = true;
            runningCoroutine = shoot(1.4f);
            StartCoroutine(runningCoroutine);
        }

        if (target)
            this.transform.LookAt(this.target.transform, Vector3.up);


        

    }

    public void placeTurretOnGround()
    {
        isBusy = true;

        //POPUP A UI ELEMENT THAT SHOWS PROGRESS BAR

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 60f, LayerMask.GetMask("Environment")))
        {
            this.transform.position = hitInfo.point; //Place the forge where the mouse is pointing
        }
        this.selectedArea.SetActive(false); //Deactivate the grid
        StopCoroutine(runningCoroutine); //Stop the coroutine that move the building and start the coroutine that builds the building
        runningCoroutine = awaitForTurretToBeBuilt(7.0f);
        StartCoroutine(runningCoroutine);

    }



    //Coroutine for delaying the construction of the building
    IEnumerator awaitForTurretToBeBuilt(float delay)
    {
        while (delay > 0)
        {
            delay -= Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("MATERIAL BASE IS READY");
        isBusy = false;
        isOperational = true;
    }





}
