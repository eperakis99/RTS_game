using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{

    protected int id;
    protected int collectionSpeed;
    protected int stamina;
    protected int yield;
    protected bool isBusy = false;

    protected GameObject clone;
    public delegate void ResourceDepleted(int id, int yield);
    public event ResourceDepleted resourceDepleted; // Event called when a resource bank is depleted 

    public Resource()
    {

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

    public void depleteStamina()
    {

        if (stamina <= 0)
        {
            resourceDepleted?.Invoke(this.id, this.yield);
        }

    }
}
