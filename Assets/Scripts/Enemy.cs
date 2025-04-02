using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    private int id;
    private int hp, maxHp = 300;
    private int power;
    private int speed;
    public Rigidbody bulletPrefab;

    public GameObject healthBar;
    private IEnumerator runningCoroutine;
    private bool shooting = false;
    private GameObject target = null;


    private GameObject clone; //A reference to a unit gameobject is stored in the class
    public NavMeshAgent agent;
    public delegate void EnemyDeath(int id);
    public event EnemyDeath enemyDeath; // Event called when a unit dies

    public Enemy()
    {
        hp = 300;
        maxHp = 300;

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

    private void Update()
    {
        //moveRandomly();

        /*if (Input.GetMouseButtonUp(1))
        {
            takeDamage();
        }*/

        if (!shooting && target != null)
        {
            shooting = true;
            runningCoroutine = shoot(1f);
            StartCoroutine(runningCoroutine);
        }

        if (target)
        {
            this.transform.LookAt(target.transform, Vector3.up);
        }
    }



    void moveRandomly()
    {
        Debug.Log("ENEMY MOVING");
        float newPosX = Random.Range(-10f, 10f);
        float newPosZ = Random.Range(-10f, 10f);
        this.agent.SetDestination(new Vector3(newPosX, this.transform.position.y, newPosZ));
    }

    public void takeDamage()
    {
        hp -= 20;
        Debug.Log(hp);
        OnHealthChanged(hp);

        if (hp == 0)
        {
            enemyDeath?.Invoke(this.id);
        }
    }


    public void OnHealthChanged(int healthLeft)
    {
        healthBar.GetComponentsInChildren<Image>()[1].fillAmount = (float) healthLeft / maxHp;
    }


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("A UNIT ENTERED ENEMY");

        if (other.gameObject.CompareTag("Units"))
        {
            target = other.gameObject;
  
        }


        
    }

    private IEnumerator shoot(float delay)
    {
        while (shooting)
        {
            Rigidbody rb;
            

            if(target && target.GetComponent<Unit>().type == UnitData.UnitType.flying)
            {
                rb = Instantiate(bulletPrefab, this.transform.position + new Vector3(0f, 8f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;
                rb.AddForce(Quaternion.Euler(-45f, 0f, 0f) * this.transform.forward * 200f, ForceMode.Force);
            }      
            else
            {
                rb = Instantiate(bulletPrefab, this.transform.position + new Vector3(0f, 4f, 0f), Quaternion.Euler(0f, -90f, 0f)) as Rigidbody;
                rb.AddForce(this.transform.forward * 200f, ForceMode.Force);
            }
                

            yield return new WaitForSeconds(delay);
            shooting = false;
        }
    }
}
