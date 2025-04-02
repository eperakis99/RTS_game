using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


//This class stores the instances of the different gameObjects in the scene
public class ResourcesManager : MonoBehaviour 
{
    public static ResourcesManager instance;

    public Dictionary<int, ScrapMetal> ScrapResourceDictionary { get; private set; }
    public Dictionary<int, Titanium> TitaniumResourceDictionary { get; private set; }
    public Dictionary<int, Coal> CoalResourceDictionary { get; private set; }

    [SerializeField] public GameObject ScrapMetalPrefab;
    [SerializeField] public GameObject TitaniumPrefab;
    [SerializeField] public GameObject CoalPrefab;

    public int scrap_metal = 0;
    public int coal = 0;
    public int titanium = 0;
    public int wood = 0;
    public int body_parts = 0;



    public ResourcesManager()
    {
        ScrapResourceDictionary = new Dictionary<int, ScrapMetal>();
        TitaniumResourceDictionary = new Dictionary<int, Titanium>();
        CoalResourceDictionary = new Dictionary<int, Coal>();
        instance = this; 
    }

    void Awake()
    {
        Debug.Log("Resource Manager has started");
        ScrapResourceDictionary = new Dictionary<int, ScrapMetal>();
        TitaniumResourceDictionary = new Dictionary<int, Titanium>();
        CoalResourceDictionary = new Dictionary<int, Coal>();
        instance = this;

        

    }

    private void OnScrapDepleted(int id, int yield)
    {
        ScrapMetal scrap = ScrapResourceDictionary[id];

        scrap.resourceDepleted -= OnScrapDepleted;

        ScrapResourceDictionary.Remove(id);
        Destroy(scrap.getClone());


    }

    private void OnCoalDepleted(int id, int yield)
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


    }


    //Instantiate resources in a certain position in the scene and store them in the respective dictionary
    //For further documentation see UnitManager.createUnit
    public int generateResources(int numOfResources, List<Vector3> possiblePositions)
    {
        int id = 0;
        int duplicates = 0;
        int randIndex = 0;


        //Temporary variables for instantiating resources
        GameObject scrapgo;
        GameObject coalgo;
        GameObject titaniumgo;

        System.Random random = new System.Random();

        for (int i = 0; i < numOfResources*5; i++)
        {
            
            if ((duplicates % 5) == 0)
                randIndex = random.Next(0, possiblePositions.Count - 1);

            if(i < (numOfResources*5) / 3)
            {
                scrapgo = Instantiate(ScrapMetalPrefab, possiblePositions[randIndex] + Quaternion.Euler(0f, (duplicates%5)*72, 0f)*(new Vector3(2f, 0f, 0f)), Quaternion.identity) as GameObject;
                ScrapMetal scrap = scrapgo.AddComponent<ScrapMetal>();
                scrap.setClone(scrapgo);
                id = scrapgo.GetInstanceID();
                scrap.setId(id);
                ScrapResourceDictionary.Add(id, scrap);

                scrap.resourceDepleted += OnScrapDepleted;
                duplicates++;
               //scrap.resourceDepleted += GUIManager.instance.OnScrapCollected;
            }
            else if (i >= (numOfResources*5) / 3 && i < 2*(numOfResources*5)/3)
            {
                coalgo = Instantiate(CoalPrefab, possiblePositions[randIndex] + Quaternion.Euler(0f, (duplicates % 5) * 72, 0f) * (new Vector3(2f, 0f, 0f)), Quaternion.identity) as GameObject;
                Coal coal = coalgo.AddComponent<Coal>();
                coal.setClone(coalgo);
                id = coalgo.GetInstanceID();
                coal.setId(id);
                CoalResourceDictionary.Add(id, coal);

                coal.resourceDepleted += OnCoalDepleted;
                duplicates++;
                //coal.resourceDepleted += GUIManager.instance.OnCoalCollected;
            }
            else
            {
                titaniumgo = Instantiate(TitaniumPrefab, possiblePositions[randIndex] + Quaternion.Euler(0f, (duplicates % 5) * 72, 0f) * (new Vector3(2f, 0f, 0f)), Quaternion.identity) as GameObject;
                Titanium titanium = titaniumgo.AddComponent<Titanium>();
                titanium.setClone(titaniumgo);
                id = titaniumgo.GetInstanceID();
                titanium.setId(id);
                TitaniumResourceDictionary.Add(id, titanium);

                titanium.resourceDepleted += OnTitaniumDepleted;
                duplicates++;
                //titanium.resourceDepleted += GUIManager.instance.OnTitaniumCollected;
            }


        }



        //Debug.Log(ScrapResourceDictionary.Count);
        return id;
    }


}

