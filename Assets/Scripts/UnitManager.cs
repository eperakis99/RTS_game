using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


//This class stores the instances of the different gameObjects in the scene
public class UnitManager : MonoBehaviour 
{
    public static UnitManager instance;

    public Dictionary<int, Unit> UnitDictionary { get; private set; }
    public List<Unit> FightersList;
    public List<Unit> FlyingList;
    public List<Unit> MedicsList;
    public List<Unit> ScavangersList;

    [SerializeField] public GameObject fighterUnitPrefab;
    [SerializeField] public GameObject flyingUnitPrefab;
    [SerializeField] public GameObject scavangerUnitPrefab;
    [SerializeField] public GameObject medicUnitPrefab;

    [SerializeField] public Rigidbody bulletPrefab;

    [SerializeField] public UnitData unitData;

    public GameObject healthbarPrefab;
    public RectTransform healthbarCanvas;

    public delegate void ResourcesUsed(UnitData.UnitType type, int amount1, int amount2, int amount3);
    public event ResourcesUsed resourcesUsed; //Event called when a unit uses resources


    public UnitManager()
    {
        UnitDictionary = new Dictionary<int, Unit>();
        FightersList = new List<Unit>();
        FlyingList = new List<Unit>();
        MedicsList = new List<Unit>();
        ScavangersList = new List<Unit>();
        instance = this; 
    }

    void Awake()
    {
        Debug.Log("Unit Manager has started");
        UnitDictionary = new Dictionary<int, Unit>();
        FightersList = new List<Unit>();
        FlyingList = new List<Unit>();
        MedicsList = new List<Unit>();
        ScavangersList = new List<Unit>();
        instance = this;

        for (int i = 1; i <= 20; i++)
        {
            createUnit(i);
        }

    }

    private void OnUnitDeath(int id)
    {
        Unit unit = UnitDictionary[id];
        
        unit.unitDeath -= OnUnitDeath; //Unsubscribe from the unitDeath event of the Unit
        Destroy(unit.healthBar);

        if (unit.type == UnitData.UnitType.fighter)
            FightersList.Remove(unit);
        else if (unit.type == UnitData.UnitType.flying)
            FlyingList.Remove(unit);
        else if (unit.type == UnitData.UnitType.scavanger)
            ScavangersList.Remove(unit);
        else if (unit.type == UnitData.UnitType.medic)
            MedicsList.Remove(unit);
        else
            Debug.Log("NO UNIT FOUND");

        UnitDictionary.Remove(id); //Remove the dead Unit from the dictionary
        Destroy(unit.getClone()); //Also destroy the gameobject instance to free space 
        
    }





    //Instantiate a unit in a certain position in the scene and store it in the dictionary
    public int createUnit(UnitData.UnitType type, Vector3 position, Quaternion rotation)
    {
        int id = 0;
        GameObject unitgo = null;
        bool isEnough = false;
        int scrapAmount = ResourcesManager.instance.scrap_metal;
        int coalAmount = ResourcesManager.instance.coal;
        int titaniumAmount = ResourcesManager.instance.titanium;
        int bodyPartsAmount = ResourcesManager.instance.body_parts;


        switch (type)
        {
            case UnitData.UnitType.fighter:  
                if(scrapAmount >= unitData.minFighterScraps && bodyPartsAmount >= unitData.minFighterBodyParts)
                {
                    resourcesUsed?.Invoke(type, unitData.minFighterScraps, unitData.minFighterBodyParts, 0) ;
                    unitgo = Instantiate(fighterUnitPrefab, position, Quaternion.Euler(0f, -90f, 0f)*rotation) as GameObject; //Instantiate a unit
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            case UnitData.UnitType.flying:
                if (scrapAmount >= unitData.minFlyingScraps && bodyPartsAmount >= unitData.minFlyingBodyParts && coalAmount >= unitData.minFlyingCoal)
                {
                    resourcesUsed?.Invoke(type, unitData.minFlyingScraps, unitData.minFlyingCoal, unitData.minFlyingBodyParts);
                    unitgo = Instantiate(flyingUnitPrefab, position, Quaternion.Euler(0f, -90f, 0f)*rotation) as GameObject; //Instantiate a unit
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            case UnitData.UnitType.scavanger:
                if (scrapAmount >= unitData.minScavangerScraps && bodyPartsAmount >= unitData.minScavangerBodyParts && titaniumAmount >= unitData.minScavangerTitanium)
                {
                    resourcesUsed?.Invoke(type, unitData.minScavangerScraps, unitData.minScavangerTitanium, unitData.minScavangerBodyParts);
                    unitgo = Instantiate(scavangerUnitPrefab, position, rotation) as GameObject; //Instantiate a unit
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            case UnitData.UnitType.medic:
                if (titaniumAmount >= unitData.minMedicTitanium && bodyPartsAmount >= unitData.minMedicBodyParts)
                {
                    resourcesUsed?.Invoke(type, unitData.minMedicTitanium, unitData.minMedicBodyParts, 0);
                    unitgo = Instantiate(medicUnitPrefab, position, Quaternion.Euler(0f, 90f, 0f)*rotation) as GameObject; //Instantiate a unit
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            default:
                throw new UnityException();
        }

        if (!isEnough) return 0;

        
        
        Unit unit = unitgo.AddComponent<Unit>(); //add the script with all the data to the unit
        unit.type = type;
        unit.bullet = this.bulletPrefab;
        addToListByType(unit.type, unit);
        unit.setClone(unitgo); //Set the member variable of the class Unit to the created gameobject
        unit.selectedArea = unitgo.transform.Find("PersonalSpace").gameObject; // assign the selected area to the Unit class
        id = unitgo.GetInstanceID(); //Get the id of the created gameobject 
        unit.setId(id);
        UnitDictionary.Add(id, unit); //add the unit to the dictionary with key its id

        unit.unitDeath += OnUnitDeath; //subscribe to the unitDeath event of the Unit

        unitgo.AddComponent<Unit_Movement>();

        Debug.Log(id);

        GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
        healthbar.GetComponent<HealthBar>().SetHealthBarData(unitgo.transform, healthbarCanvas, 80f);
        healthbar.transform.SetParent(healthbarCanvas, false);
        unit.healthBar = healthbar;

        return id;
    }



    //Instantiate a unit in a certain position in the scene and store it in the dictionary
    //Overloaded method for instantiation at the beggining of the game
    public int createUnit(int i)
    {
        int id = 0;
        GameObject unitgo = null;
        Unit unit = null;
        //Instantiate a unit
        if (i <= 5)
        {
            unitgo = Instantiate(fighterUnitPrefab, new Vector3((i % 10) * 3f, 1.2f, (i / 10) * 3f), Quaternion.Euler(0f, -90f, 0f));
            unit = unitgo.AddComponent<Unit>(); //add the script with all the data to the unit
            unit.type = UnitData.UnitType.fighter;
            unit.bullet = this.bulletPrefab;
            
        }       
        else if (i <= 10)
        {
            unitgo = Instantiate(flyingUnitPrefab, new Vector3((i % 10) * 3f, 1.2f, (i / 10) * 3f), Quaternion.Euler(0f, -90f, 0f));
            unit = unitgo.AddComponent<Unit>(); //add the script with all the data to the unit
            unit.type = UnitData.UnitType.flying;
            unit.bullet = this.bulletPrefab;
        }           
        else if (i <= 15)
        {
            unitgo = Instantiate(scavangerUnitPrefab, new Vector3((i % 10) * 3f, 1.2f, (i / 10) * 3f), Quaternion.identity);
            unit = unitgo.AddComponent<Unit>(); //add the script with all the data to the unit
            unit.type = UnitData.UnitType.scavanger;
        }
        else
        {
            unitgo = Instantiate(medicUnitPrefab, new Vector3((i % 10) * 3f, 1.2f, (i / 10) * 3f), Quaternion.Euler(0f, 90f, 0f));
            unit = unitgo.AddComponent<Unit>(); //add the script with all the data to the unit
            unit.type = UnitData.UnitType.medic;
        }


        addToListByType(unit.type, unit);
        unit.setClone(unitgo); //Set the member variable of the class Unit to the created gameobject
        unit.selectedArea = unitgo.transform.Find("PersonalSpace").gameObject; // assign the selected area to the Unit class
        id = unitgo.GetInstanceID(); //Get the id of the created gameobject 
        unit.setId(id);
        UnitDictionary.Add(id, unit); //add the unit to the dictionary with key its id

        unit.unitDeath += OnUnitDeath; //subscribe to the unitDeath event of the Unit

        unitgo.AddComponent<Unit_Movement>();

        GameObject healthbar = Instantiate(healthbarPrefab) as GameObject;
        healthbar.GetComponent<HealthBar>().SetHealthBarData(unitgo.transform, healthbarCanvas, 80f);
        healthbar.transform.SetParent(healthbarCanvas, false);
        unit.healthBar = healthbar;

        Debug.Log(id);

        return id;
    }


    private void addToListByType(UnitData.UnitType type, Unit unit){

        switch (type)
        {
            case UnitData.UnitType.fighter:
                FightersList.Add(unit);
                break;
            case UnitData.UnitType.flying:
                FlyingList.Add(unit);
                break;
            case UnitData.UnitType.scavanger:
                ScavangersList.Add(unit);
                break;
            case UnitData.UnitType.medic:
                MedicsList.Add(unit);
                break;
            default:
                Debug.Log("NO TYPE");
                break;
        }

    }

}

