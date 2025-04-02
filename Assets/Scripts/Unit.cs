using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IDamageable
{
    private string unitName;
    private int id;
    public int hp, maxHp = 100;
    private int power;
    private int speed;
    public bool selected = false;
    public GameObject selectedArea;
    private UnitData.UnitStates state;
    public UnitData.UnitType type;
    public GameObject healthBar;

    [SerializeField] public Rigidbody bullet;
    public bool shooting = false;
    public GameObject target = null;


    private GameObject clone; //A reference to a unit gameobject is stored in the class
    public NavMeshAgent agent;
    public delegate void UnitDeath(int id);
    public event UnitDeath unitDeath; // Event called when a unit dies

    public Unit()
    {
        hp = 100;
        maxHp = 100;
        unitName = "Unit" + (id%200);
        
    }

    private void Update()
    {
        //moveRandomly();

        /*if (Input.GetMouseButtonUp(1))
        {
            takeDamage();
        }*/

        if(target)
            this.transform.LookAt(target.transform, Vector3.up);
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

    private void Start()
    {
        this.agent = clone.GetComponent<NavMeshAgent>();
        //this.id = this.GetInstanceID();
        //UnitManager.instance.UnitDictionary.Add(this.id, this);
        //Debug.Log("New Unit created with id "+this.id);
    }

    //Show a green circle under the unit when it is selected
    public void toggleSelectedVisual()
    {
        if(selected) Debug.Log("TOGGLED SELECTED AREA");
        Debug.Log(selectedArea);
        if(selectedArea != null)
        {
            selectedArea.SetActive(this.selected);
        }
    }

    public void takeDamage()
    {
        hp -= 10;
        Debug.Log(hp);
        OnHealthChanged(hp);

        if (hp == 0)
        {
            unitDeath?.Invoke(this.id);
        }
    }


    public void OnHealthChanged(int healthLeft)
    {
        healthBar.GetComponentsInChildren<Image>()[1].fillAmount = (float)healthLeft / maxHp;
    }


    //Used by medics for healing
    void OnTriggerEnter(Collider other)
    {
        Unit unitEntered= other.gameObject.GetComponent<Unit>();
        int healAmount = 20;
        if (unitEntered != null && this.type == UnitData.UnitType.medic && unitEntered.type != UnitData.UnitType.medic)
        {
            if (unitEntered.maxHp - unitEntered.hp < healAmount)
            {
                unitEntered.hp += healAmount;
                unitEntered.OnHealthChanged(unitEntered.hp);
            }
            else if (unitEntered.maxHp - unitEntered.hp > healAmount)
            {
                unitEntered.hp = unitEntered.maxHp;
                unitEntered.OnHealthChanged(unitEntered.hp);
            }
            else
            {
                unitEntered.hp += 0;
                unitEntered.OnHealthChanged(unitEntered.hp);
            }
        }
    }



    public IEnumerator shoot(float delay)
    {
        while (shooting)
        {
            Rigidbody rb;
            rb = Instantiate(bullet, this.transform.position + new Vector3(0f, 4f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;

            if (this.type == UnitData.UnitType.fighter) {
                rb = Instantiate(bullet, this.transform.position + new Vector3(0f, 4f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;
                rb.AddForce(this.transform.forward * 200f, ForceMode.Force);
            }
            else if (this.type == UnitData.UnitType.flying)
            {
                rb = Instantiate(bullet, this.transform.position + new Vector3(1f, 2f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;
                rb.AddForce(Quaternion.Euler(15f, 0f, 0f) * this.transform.forward * 200f, ForceMode.Force);
            }   
            else
                Debug.Log("NO BULLET");

            yield return new WaitForSeconds(delay);
            shooting = false;
        }
    }

}
