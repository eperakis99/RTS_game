using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    private Material prevUnitMaterial = null;
    private Material prevBuildingMaterial = null;
    private Material prevResourceMaterial = null;

    private Transform currentSelection = null;

    public Camera mainCamera;

    [SerializeField] public Material selectedMaterial = null;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (currentSelection)
        {
            if (currentSelection.CompareTag("Units"))
            {
                currentSelection.GetComponentInChildren<Renderer>().material = prevUnitMaterial;
            }
            else if (currentSelection.CompareTag("Resource"))
            {
                currentSelection.GetComponentInChildren<Renderer>().material = prevResourceMaterial;
            }
            else
            {
                currentSelection.GetComponentInChildren<Renderer>().material = prevBuildingMaterial;
            }

            currentSelection = null;

        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f, LayerMask.GetMask("Selectable")))
        {
            if (hitInfo.transform.CompareTag("Units"))
            {
                Renderer renderer = hitInfo.transform.GetComponentInChildren<Renderer>();
                if (renderer)
                {
                    prevUnitMaterial = renderer.material;
                    renderer.material = selectedMaterial;
                    currentSelection = hitInfo.transform;
                }
                

            }
            else if (hitInfo.transform.CompareTag("Resource"))
            {
                Renderer renderer = hitInfo.transform.GetComponentInChildren<Renderer>();
                if (renderer)
                {
                    prevResourceMaterial = renderer.material;
                    renderer.material = selectedMaterial;
                    currentSelection = hitInfo.transform;
                } 

            }
            else if (hitInfo.transform.CompareTag("Building"))
            {
                Renderer renderer = hitInfo.transform.GetComponentInChildren<Renderer>();
                if (renderer)
                {
                    prevBuildingMaterial = renderer.material;
                    renderer.material = selectedMaterial;
                    currentSelection = hitInfo.transform;
                } 
            }
            else
            {
                currentSelection = null;
            }

            
        }
    }
}
