using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBase : Building
{
    private int productionTime;
    //public GameObject healthBar;
    GameObject createUnitButton;

    public BuildingData buildingData;

    public Material redMaterial;
    public Material greenMaterial;

    public delegate void BodyPartProduced(int yield, int scrapsNeeded, int coalNeeded);
    public event BodyPartProduced bodyPartReady; // Event called when a body part is produced


    private void Awake()
    {
        productionTime = 10;
        isOperational = false;
        isBusy = false;
        this.maxHp = 500;
        this.hp = maxHp;
        

        this.bodyPartReady += GUIManager.instance.OnBodyPartsCreated; 

    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("A UNIT ENTERED THE MATERIAL BASE");
        int scraps = ResourcesManager.instance.scrap_metal;
        int coal = ResourcesManager.instance.coal;

        if (other.gameObject.CompareTag("Units") && this.isOperational && !this.isBusy)
        {
            
            if(scraps >= this.buildingData.minBodyPartsScraps && coal >= this.buildingData.minBodyPartsCoal)
            {
                isBusy = true;
                startProduction();
            }
        }
    }


    public void startProduction()
    {

        StopCoroutine(runningCoroutine);
        runningCoroutine = produceBodyParts(1, 1.0f);
        StartCoroutine(runningCoroutine);
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
                placeMaterialBaseOnGround();
            }
        }

    }

    public void placeMaterialBaseOnGround()
    {
        isBusy = true;

        //POPUP A UI ELEMENT THAT SHOWS PROGRESS BAR

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            this.transform.position = hitInfo.point; //Place the forge where the mouse is pointing
        }
        this.selectedArea.SetActive(false); //Deactivate the grid
        StopCoroutine(runningCoroutine); //Stop the coroutine that move the building and start the coroutine that builds the building
        runningCoroutine = awaitForMaterialBaseToBeBuilt(7.0f);
        StartCoroutine(runningCoroutine);

    }


    IEnumerator produceBodyParts(int numOfParts, float delay)
    {
        while (productionTime > 0)
        {
            productionTime--;
            yield return new WaitForSeconds(delay);
        }

        isBusy = false;
        Debug.Log("MATERIAL BASE HAS MADE A BODY PART");
        productionTime = 10; //Reset the timer
        bodyPartReady?.Invoke(numOfParts, buildingData.minBodyPartsScraps, buildingData.minBodyPartsCoal);
        
    }


    //Coroutine for delaying the construction of the building
    IEnumerator awaitForMaterialBaseToBeBuilt(float delay)
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
