using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ForgeBuilding : Building
{
    private int groundUnitsProduced;
    private int productionTime;
    private Camera cam;
    GameObject forgeButtons;
    Button[] createUnitButtons;

    public Material redMaterial;
    public Material greenMaterial;

    public ForgeBuilding()
    {
      
    }


    private void Awake()
    {
        groundUnitsProduced = 1;
        productionTime = 10;
        isOperational = false;
        isBusy = false;
        this.maxHp = 500;
        this.hp = maxHp;
        cam = Camera.main;

        forgeButtons = GUIManager.instance.showForgeButtons();
        forgeButtons.SetActive(false);
        createUnitButtons = forgeButtons.GetComponentsInChildren<Button>();

        createUnitButtons[0].onClick.AddListener(delegate { startProduction(UnitData.UnitType.fighter); });
        createUnitButtons[1].onClick.AddListener(delegate { startProduction(UnitData.UnitType.flying); });
        createUnitButtons[2].onClick.AddListener(delegate { startProduction(UnitData.UnitType.scavanger); });
        createUnitButtons[3].onClick.AddListener(delegate { startProduction(UnitData.UnitType.medic); });
    }

    private void OnTriggerEnter(Collider other)
    {

        //POPUP A UI ELEMENT THAT 
        

        if (other.gameObject.CompareTag("Units") && this.isOperational && !this.isBusy)
        {
            isBusy = true;
            forgeButtons.SetActive(true);
            Debug.Log("UNIT ENTERED FORGE");
            
            //startProduction();
        }
    }


    public void startProduction(UnitData.UnitType type)
    {
        forgeButtons.SetActive(false);
        StopCoroutine(runningCoroutine);
        runningCoroutine = produceUnit(type ,1.0f);
        StartCoroutine(runningCoroutine);
    }


    private void Update()
    {
        RaycastHit hitPoint;

        if(Physics.Raycast(this.clone.transform.position, Vector3.down, out hitPoint, 60f, LayerMask.GetMask("Environment")))
        {
            Debug.Log("Raycast Hit at"+hitPoint.point);
            if(hitPoint.point.y > 1f)
            {
                selectedArea.GetComponent<Renderer>().material = redMaterial;
            }
            else
            {
                selectedArea.GetComponent<Renderer>().material = greenMaterial;
            }


            if (Input.GetMouseButtonDown(0) && hitPoint.point.y <= 1f && !isOperational && !isBusy)
            {
                placeForgeOnGround();
            }
        }


        if (Input.GetMouseButtonUp(1))
        {
            this.takeDamage();
            OnHealthChanged(hp);
        }


    }

    public void placeForgeOnGround()
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
        runningCoroutine = awaitForForgeToBeBuilt(5.0f);
        StartCoroutine(runningCoroutine);

    }

    //Coroutine for delaying the production of a Unit
    IEnumerator produceUnit(UnitData.UnitType type, float delay)
    {
        while(productionTime > 0)
        {
            productionTime--;
            yield return new WaitForSeconds(delay);
        }

        //agent.enabled = false;
        int id = UnitManager.instance.createUnit(type, new Vector3(4f, 1.2f, 4f), Quaternion.identity); //Instantiate the new Unit next to the building
        isBusy = false;
        productionTime = 10; //Reset the timer
        Debug.Log("NO OF UNITS IS: "+UnitManager.instance.UnitDictionary.Count);
        //GameManager.instance.rebuildNavMesh();
    } 


    //Coroutine for delaying the construction of the building
    IEnumerator awaitForForgeToBeBuilt(float delay)
    {
        while(delay > 0)
        {
            delay -= Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }

        Debug.Log("FORGE IS READY");
        isBusy = false;
        isOperational = true;
    }

}
