using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;

    private Camera mainCamera;
    [SerializeField] public NavMeshSurface navmesh;
    public Dictionary<int, Building> BuildingDictionary { get; private set; }
    //public Dictionary<int, Titanium> TitaniumResourceDictionary { get; private set; }
    //public Dictionary<int, Coal> CoalResourceDictionary { get; private set; }

    [SerializeField] public GameObject ForgeBuildingPrefab;
    [SerializeField] public GameObject TurretPrefab;
    [SerializeField] public GameObject MaterialBasePrefab;

    [SerializeField] public BuildingData buildingData;

    [SerializeField] public Material redMaterial;
    [SerializeField] public Material greenMaterial;

    [SerializeField] public Rigidbody bulletPrefab;

    public GameObject healthbarPrefab;
    public RectTransform healthbarCanvas;


    public delegate void ResourcesUsed(BuildingData.BuildingType type, int amount1, int amount2);
    public delegate void ResourcesUsed2(BuildingData.BuildingType type, int amount1, int amount2, int amount3);
    public event ResourcesUsed resourcesUsed; //Event called when a building uses 2 resources
    public event ResourcesUsed2 resourcesUsed2; //Event called when a building uses 3 types of resources
    

    public BuildingManager()
    {
        BuildingDictionary = new Dictionary<int, Building>();
        //TitaniumResourceDictionary = new Dictionary<int, Titanium>();
        //CoalResourceDictionary = new Dictionary<int, Coal>();
        instance = this;
    }

    void Awake()
    {
        Debug.Log("Resource Manager has started");
        BuildingDictionary = new Dictionary<int, Building>();
        //TitaniumResourceDictionary = new Dictionary<int, Titanium>();
        //CoalResourceDictionary = new Dictionary<int, Coal>();
        instance = this;
        mainCamera = Camera.main;



    }

    private void OnForgeDestroyed(int id)
    {
        ForgeBuilding forge = (ForgeBuilding)BuildingDictionary[id];

        forge.buildingDestruction -= OnForgeDestroyed;
        Destroy(forge.healthBar);
        BuildingDictionary.Remove(id);
        Destroy(forge.getClone());


    }

    private void OnMaterialBaseDestroyed(int id)
    {
        MaterialBase mb = (MaterialBase)BuildingDictionary[id];

        mb.buildingDestruction -= OnMaterialBaseDestroyed;
        Destroy(mb.healthBar);
        BuildingDictionary.Remove(id);
        Destroy(mb.getClone());


    }

    private void OnTurretDestroyed(int id)
    {
        Turret turret = (Turret)BuildingDictionary[id];

        turret.buildingDestruction += OnTurretDestroyed;
        Destroy(turret.healthBar);
        BuildingDictionary.Remove(id);
        Destroy(turret.getClone());


    }

    /*    private void OnCoalDepleted(int id, int yield)
        {
            Coal coal = CoalResourceDictionary[id];

            coal.resourceDepleted -= OnCoalDepleted;

            CoalResourceDictionary.Remove(id);
            Destroy(coal.getClone());


        }

        private void OnTitaniumDepleted(int id, int yield)
        {
            Titanium titanium = TitaniumResourceDictionary[id];

            titanium.resourceDepleted -= OnTitaniumDepleted;

            TitaniumResourceDictionary.Remove(id);
            Destroy(titanium.getClone());


        }*/


    //Instantiate building in a certain position in the scene and store them in the respective dictionary
    //For further documentation see UnitManager.createUnit
    public void createForge()
    {
        int id = 0;
        Vector3 initPos = Vector3.zero;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            initPos = hitInfo.point;
        }        

        initPos.y += 1f; //The instantiated building must hover above ground before it is placed

        //Create only if there are enough resources
        if(ResourcesManager.instance.scrap_metal >= buildingData.minForgeScraps && ResourcesManager.instance.coal >= buildingData.minForgeCoal)
        {
            resourcesUsed?.Invoke(BuildingData.BuildingType.forge, buildingData.minForgeScraps, buildingData.minForgeCoal);

            GameObject forgego = Instantiate(ForgeBuildingPrefab, initPos, Quaternion.identity) as GameObject; //Instantiate a building
            forgego.AddComponent<ForgeBuilding>(); //add the script with all the data to the building
            ForgeBuilding forge = forgego.GetComponent<ForgeBuilding>();
            forge.setClone(forgego); //Set the member variable of the class Building to the created gameobject
            forge.selectedArea = forgego.transform.Find("grid").gameObject; // assign the selected area to the Building class
            id = forgego.GetInstanceID(); //Get the id of the created gameobject 
            forge.setId(id);
            forge.redMaterial = this.redMaterial;
            forge.greenMaterial = this.greenMaterial;
            BuildingDictionary.Add(id, forge); //add the unit to the dictionary with key its id

            forge.buildingDestruction += OnForgeDestroyed; //subscribe to the unitDeath event of the Unit

            forge.runningCoroutine = forge.moveBuildingUntilPlaced();
            forge.StartCoroutine(forge.runningCoroutine);

            GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
            healthbar.GetComponent<HealthBar>().SetHealthBarData(forgego.transform, healthbarCanvas, 150f);
            healthbar.transform.SetParent(healthbarCanvas, false);
            forge.healthBar = healthbar;

            Debug.Log(id);
        }
        

        
    }


    //Instantiate building in a certain position in the scene and store them in the respective dictionary
    //For further documentation see UnitManager.createUnit
    public void createMaterialBase()
    {
        int id = 0;
        Vector3 initPos = Vector3.zero;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            initPos = hitInfo.point;
        }

        initPos.y += 1f; //The instantiated building must hover above ground before it is placed

        //Create only if there are enough resources
        if (ResourcesManager.instance.scrap_metal >= buildingData.minMaterialBaseScraps && ResourcesManager.instance.titanium >= buildingData.minMaterialBaseTitanium)
        {
            resourcesUsed?.Invoke(BuildingData.BuildingType.materialBase, buildingData.minMaterialBaseScraps, buildingData.minMaterialBaseTitanium);

            GameObject materialbasego = Instantiate(MaterialBasePrefab, initPos, Quaternion.identity) as GameObject; //Instantiate a building
            materialbasego.AddComponent<MaterialBase>(); //add the script with all the data to the building
            MaterialBase mb = materialbasego.GetComponent<MaterialBase>();
            mb.setClone(materialbasego); //Set the member variable of the class Building to the created gameobject
            mb.selectedArea = materialbasego.transform.Find("grid").gameObject; // assign the selected area to the Building class
            id = materialbasego.GetInstanceID(); //Get the id of the created gameobject 
            mb.setId(id);
            mb.buildingData = buildingData;
            mb.redMaterial = this.redMaterial;
            mb.greenMaterial = this.greenMaterial;
            BuildingDictionary.Add(id, mb); //add the unit to the dictionary with key its id

            mb.buildingDestruction += OnMaterialBaseDestroyed; //subscribe to the unitDeath event of the Unit

            mb.runningCoroutine = mb.moveBuildingUntilPlaced();
            mb.StartCoroutine(mb.runningCoroutine);

            GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
            healthbar.GetComponent<HealthBar>().SetHealthBarData(materialbasego.transform, healthbarCanvas, 150f);
            healthbar.transform.SetParent(healthbarCanvas, false);
            mb.healthBar = healthbar;

            Debug.Log(id);
        }
    }



    //Instantiate building in a certain position in the scene and store them in the respective dictionary
    //For further documentation see UnitManager.createUnit
    public void createTurret()
    {
        int id = 0;
        Vector3 initPos = Vector3.zero;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            initPos = hitInfo.point;
        }

        initPos.y += 1f; //The instantiated building must hover above ground before it is placed

        //Create only if there are enough resources
        if (ResourcesManager.instance.titanium >= buildingData.minTurretTitanium && ResourcesManager.instance.coal >= buildingData.minTurretCoal)
        {
            resourcesUsed?.Invoke(BuildingData.BuildingType.turret, buildingData.minTurretTitanium, buildingData.minTurretCoal);

            GameObject turretgo = Instantiate(TurretPrefab, initPos, Quaternion.identity) as GameObject; //Instantiate a building
            turretgo.AddComponent<Turret>(); //add the script with all the data to the building
            Turret turret = turretgo.GetComponent<Turret>();
            turret.setClone(turretgo); //Set the member variable of the class Building to the created gameobject
            turret.selectedArea = turretgo.transform.Find("grid").gameObject; // assign the selected area to the Building class
            id = turretgo.GetInstanceID(); //Get the id of the created gameobject 
            turret.setId(id);
            turret.buildingData = buildingData;
            turret.redMaterial = this.redMaterial;
            turret.greenMaterial = this.greenMaterial;
            turret.bulletPrefab = this.bulletPrefab;
            BuildingDictionary.Add(id, turret); //add the unit to the dictionary with key its id

            turret.buildingDestruction += OnTurretDestroyed; //subscribe to the unitDeath event of the Unit

            turret.runningCoroutine = turret.moveBuildingUntilPlaced();
            turret.StartCoroutine(turret.runningCoroutine);

            GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
            healthbar.GetComponent<HealthBar>().SetHealthBarData(turretgo.transform, healthbarCanvas, 150f);
            healthbar.transform.SetParent(healthbarCanvas, false);
            turret.healthBar = healthbar;

            Debug.Log(id);
        }



    }


}
