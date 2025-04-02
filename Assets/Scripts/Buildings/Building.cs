using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour, IDamageable
{

    protected int id;
    protected int hp, maxHp;
    protected bool isBusy = false;
    protected bool isOperational = false;
    public GameObject selectedArea;
    public IEnumerator runningCoroutine;

    protected GameObject clone;
    public GameObject healthBar;


    public delegate void BuildingDestroyed(int id);
    public event BuildingDestroyed buildingDestruction; // Event called when a building gets destroyed

    public delegate void ConstructionCompleted();
    public event ConstructionCompleted constructionCompleted; //Event called when a building has been fully constructed

    void Awake()
    {
        hp = 100;
        maxHp = 100;
        
    }



    public GameObject getClone()
    {
        return clone;
    }

    public void setClone(GameObject g)
    {
        clone = g;
    }

    public void setId(int id)
    {
        this.id = id;
    }

    public void takeDamage()
    {
        hp -= 10;
        Debug.Log(hp);
        OnHealthChanged(hp);

        if (hp == 0)
        {
            buildingDestruction?.Invoke(this.id);
        }
    }

    public void OnHealthChanged(int healthLeft)
    {
        
        healthBar.GetComponentsInChildren<Image>()[1].fillAmount = (float)healthLeft/maxHp;
    }


    public IEnumerator moveBuildingUntilPlaced()
    {
        while (!isOperational)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 200f, LayerMask.GetMask("Environment")))
            {
                Vector3 newPos = hitInfo.point;
                newPos.y += 1f; //Float a little over the ground
                this.clone.transform.position = newPos; 
            }

            yield return null;
        }

        
    }

}
