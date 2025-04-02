using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Collide_Check : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("A collision happened");
        Debug.Log(other.tag);
        /*if (other.gameObject.CompareTag("PersonalSpace"))
        {
            Debug.Log("A collider has entered this rigidbody with name: " + this.name);
            other.transform.parent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }*/
            
    }
}
