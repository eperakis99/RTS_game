using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionBox : MonoBehaviour
{

    public Image box;
    [SerializeField] public Camera mainCamera;
    private Vector2 startPos, endPos, currentPos;
    private float minX, minY, maxX, maxY;
    private Ray eyeRay;
    private float maxDistance = 200f;
    public List<Unit> unitsSelected = new List<Unit>();
    private Dictionary<int, Unit> dict;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Assert(box != null);
        box.gameObject.SetActive(false);
        dict = UnitManager.instance.UnitDictionary;
    }

    // Update is called once per frame
    void Update()
    {


        //Where you click a box is produced
        if (Input.GetMouseButtonDown(0))
        {
            box.gameObject.SetActive(true);
            startPos = Input.mousePosition;
            //unitsSelected.Clear();

        }

        if (Input.GetMouseButtonDown(1))
        {
            unitsSelected.Clear();
        }

        //while you are clicking the box scales with the movement of your mouse
        if (Input.GetMouseButton(0))
        {
            currentPos = Input.mousePosition;

            box.gameObject.transform.position = startPos;
            box.transform.localScale = currentPos - startPos; // The difference gives the direction along which the box is scaled
            minX = Mathf.Min(startPos.x, currentPos.x);
            maxX = Mathf.Max(startPos.x, currentPos.x);
            minY = Mathf.Min(startPos.y, currentPos.y);
            maxY = Mathf.Max(startPos.y, currentPos.y);

            

        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            box.gameObject.SetActive(false);
            minX = 0;
            minY = 0;
            maxX = 0;
            maxY = 0;
        }

        


    }

    private void FixedUpdate()
    {
        
        if (dict.Count == 0) return; //return if the dictionary is empty

        foreach(int i in dict.Keys)
        {

            Vector3 unitPos = mainCamera.WorldToScreenPoint(dict[i].getClone().transform.position);
            if(unitPos.x > minX && unitPos.x < maxX && unitPos.y > minY && unitPos.y < maxY)
            {
                unitsSelected.Add(dict[i]);
                dict[i].selected = true;
                dict[i].toggleSelectedVisual();

                Debug.Log("UNIT WITH ID " + i + " IS SELECTED: " + dict[i].selected);
            }
            


        }

       
    }
}
