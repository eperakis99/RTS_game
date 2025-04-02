using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;
    [SerializeField] public GameObject scrapTextObject;
    [SerializeField] public GameObject titaniumTextObject;
    [SerializeField] public GameObject coalTextObject;
    [SerializeField] public GameObject bodyPartsTextObject;
    [SerializeField] private GameObject forgeButtons;
    [SerializeField] public GameObject mainCanvas;

    private TextMeshProUGUI scrapText, titaniumText, coalText, bodyPartsText;
    private int scrapAmount, titaniumAmount, coalAmount, bodyPartsAmount = 0;

    private void Awake()
    {
        instance = this; 
    }

    public void initializeManager()
    {
        scrapText = scrapTextObject.GetComponent<TextMeshProUGUI>();
        titaniumText = titaniumTextObject.GetComponent<TextMeshProUGUI>();
        coalText = coalTextObject.GetComponent<TextMeshProUGUI>();
        bodyPartsText = bodyPartsTextObject.GetComponent<TextMeshProUGUI>();

        Dictionary<int, ScrapMetal> scraps = ResourcesManager.instance.ScrapResourceDictionary;
        Dictionary<int, Titanium> titanium = ResourcesManager.instance.TitaniumResourceDictionary;
        Dictionary<int, Coal> coal = ResourcesManager.instance.CoalResourceDictionary;
        Dictionary<int, Building> buildings = BuildingManager.instance.BuildingDictionary;


        foreach (int id in scraps.Keys)
        {
            scraps[id].resourceDepleted += OnScrapCollected;
        }

        foreach (int id in titanium.Keys)
        {
            titanium[id].resourceDepleted += OnTitaniumCollected;
        }

        foreach (int id in coal.Keys)
        {
            coal[id].resourceDepleted += OnCoalCollected;
        }

        BuildingManager.instance.resourcesUsed += OnResourcesUsed;

        UnitManager.instance.resourcesUsed += OnResourcesUsed;
    }

    //Event called when a chunk of scraps is collected
    private void OnScrapCollected(int id, int yield)
    {
        Debug.Log("SCRAPS CHANGED");
        scrapAmount = ResourcesManager.instance.scrap_metal;
        scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
        ResourcesManager.instance.scrap_metal += yield;
        scrapText.text += ResourcesManager.instance.scrap_metal;

    }

    //Event called when a chunk of titanium is collected
    private void OnTitaniumCollected(int id, int yield)
    {
        Debug.Log("TITANIUM CHANGED");
        titaniumAmount = ResourcesManager.instance.titanium;
        titaniumText.text = titaniumText.text.Remove(titaniumText.text.Length - titaniumAmount.ToString().Length, titaniumAmount.ToString().Length);
        ResourcesManager.instance.titanium += yield;
        titaniumText.text += ResourcesManager.instance.titanium;
    }

    //Event called when a chunk of coal is collected
    private void OnCoalCollected(int id, int yield)
    {
        Debug.Log("COAL CHANGED");
        coalAmount = ResourcesManager.instance.coal;
        coalText.text = coalText.text.Remove(coalText.text.Length - coalAmount.ToString().Length, coalAmount.ToString().Length);
        ResourcesManager.instance.coal += yield;
        coalText.text += ResourcesManager.instance.coal;
    }


    public void OnBodyPartsCreated(int yield, int amount1, int amount2)
    {
        bodyPartsAmount = ResourcesManager.instance.body_parts;
        bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
        ResourcesManager.instance.body_parts += yield;
        bodyPartsText.text += ResourcesManager.instance.body_parts;

        scrapAmount = ResourcesManager.instance.scrap_metal;
        scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
        ResourcesManager.instance.scrap_metal -= amount1;
        scrapText.text += ResourcesManager.instance.scrap_metal;

        coalAmount = ResourcesManager.instance.coal;
        coalText.text = coalText.text.Remove(coalText.text.Length - coalAmount.ToString().Length, coalAmount.ToString().Length);
        ResourcesManager.instance.coal -= amount2;
        coalText.text += ResourcesManager.instance.coal;
    }

    //Event called when a building uses resources
    private void OnResourcesUsed(BuildingData.BuildingType type, int amount1, int amount2)
    {
        if(type == BuildingData.BuildingType.forge)
        {
            scrapAmount = ResourcesManager.instance.scrap_metal;
            scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
            ResourcesManager.instance.scrap_metal -= amount1;
            scrapText.text += ResourcesManager.instance.scrap_metal;

            coalAmount = ResourcesManager.instance.coal;
            coalText.text = coalText.text.Remove(coalText.text.Length - coalAmount.ToString().Length, coalAmount.ToString().Length);
            ResourcesManager.instance.coal -= amount2;
            coalText.text += ResourcesManager.instance.coal;
        }
        else if (type == BuildingData.BuildingType.materialBase)
        {
            scrapAmount = ResourcesManager.instance.scrap_metal;
            scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
            ResourcesManager.instance.scrap_metal -= amount1;
            scrapText.text += ResourcesManager.instance.scrap_metal;

            titaniumAmount = ResourcesManager.instance.titanium;
            titaniumText.text = titaniumText.text.Remove(titaniumText.text.Length - titaniumAmount.ToString().Length, titaniumAmount.ToString().Length);
            ResourcesManager.instance.titanium -= amount2;
            titaniumText.text += ResourcesManager.instance.titanium;
        }
        else if (type == BuildingData.BuildingType.turret)
        {
            
        }
        else if (type == BuildingData.BuildingType.stronghold)
        {
            
        }
        else
        {
            
        }

    }



    //Event called when a Unit uses resources
    private void OnResourcesUsed(UnitData.UnitType type, int amount1, int amount2, int amount3)
    {
        if (type == UnitData.UnitType.fighter)
        {
            scrapAmount = ResourcesManager.instance.scrap_metal;
            scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
            ResourcesManager.instance.scrap_metal -= amount1;
            scrapText.text += ResourcesManager.instance.scrap_metal;

            bodyPartsAmount = ResourcesManager.instance.body_parts;
            bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
            ResourcesManager.instance.body_parts -= amount2;
            bodyPartsText.text += ResourcesManager.instance.body_parts;
        }
        else if (type == UnitData.UnitType.flying)
        {
            scrapAmount = ResourcesManager.instance.scrap_metal;
            scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
            ResourcesManager.instance.scrap_metal -= amount1;
            scrapText.text += ResourcesManager.instance.scrap_metal;

            coalAmount = ResourcesManager.instance.coal;
            coalText.text = coalText.text.Remove(coalText.text.Length - coalAmount.ToString().Length, coalAmount.ToString().Length);
            ResourcesManager.instance.coal -= amount2;
            coalText.text += ResourcesManager.instance.coal;

            bodyPartsAmount = ResourcesManager.instance.body_parts;
            bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
            ResourcesManager.instance.body_parts -= amount2;
            bodyPartsText.text += ResourcesManager.instance.body_parts;
        }
        else if (type == UnitData.UnitType.scavanger)
        {
            scrapAmount = ResourcesManager.instance.scrap_metal;
            scrapText.text = scrapText.text.Remove(scrapText.text.Length - scrapAmount.ToString().Length, scrapAmount.ToString().Length);
            ResourcesManager.instance.scrap_metal -= amount1;
            scrapText.text += ResourcesManager.instance.scrap_metal;

            titaniumAmount = ResourcesManager.instance.titanium;
            titaniumText.text = titaniumText.text.Remove(titaniumText.text.Length - titaniumAmount.ToString().Length, titaniumAmount.ToString().Length);
            ResourcesManager.instance.titanium -= amount2;
            titaniumText.text += ResourcesManager.instance.titanium;

            bodyPartsAmount = ResourcesManager.instance.body_parts;
            bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
            ResourcesManager.instance.body_parts -= amount2;
            bodyPartsText.text += ResourcesManager.instance.body_parts;

        }
        else if (type == UnitData.UnitType.medic)
        {
            titaniumAmount = ResourcesManager.instance.titanium;
            titaniumText.text = titaniumText.text.Remove(titaniumText.text.Length - titaniumAmount.ToString().Length, titaniumAmount.ToString().Length);
            ResourcesManager.instance.titanium -= amount2;
            titaniumText.text += ResourcesManager.instance.titanium;

            bodyPartsAmount = ResourcesManager.instance.body_parts;
            bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
            ResourcesManager.instance.body_parts -= amount2;
            bodyPartsText.text += ResourcesManager.instance.body_parts;
        }
        else
        {

        }

    }


    private void OnBodyPartsUsed(int amount)
    {
        bodyPartsAmount = ResourcesManager.instance.body_parts;
        bodyPartsText.text = bodyPartsText.text.Remove(bodyPartsText.text.Length - bodyPartsAmount.ToString().Length, bodyPartsAmount.ToString().Length);
        ResourcesManager.instance.body_parts -= amount;
        bodyPartsText.text += ResourcesManager.instance.body_parts;


    }



    public void selectAllFighters()
    {
        List<Unit> units = UnitManager.instance.FightersList; ;
        foreach (Unit unit in units)
        {
            unit.selected = true;
            unit.toggleSelectedVisual();
        }
    }

    public void selectAllFlying()
    {
        List<Unit> units = UnitManager.instance.FlyingList;
        foreach (Unit unit in units)
        {
            unit.selected = true;
            unit.toggleSelectedVisual();
        }
    }

    public void selectAllMedics()
    {
        List<Unit> units = UnitManager.instance.MedicsList;
        foreach (Unit unit in units)
        {
            unit.selected = true;
            unit.toggleSelectedVisual();
        }
    }

    public void selectAllScavangers()
    {
        List<Unit> units = UnitManager.instance.ScavangersList;
        foreach (Unit unit in units)
        {
            unit.selected = true;
            unit.toggleSelectedVisual();
        }
    }






    public GameObject showForgeButtons()
    {
        forgeButtons.SetActive(true);
        return forgeButtons;
    }

}
