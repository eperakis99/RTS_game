using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShoot : MonoBehaviour
{
    
    private IEnumerator runningCoroutine;
    [SerializeField] GameObject unitparent;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("AN ENEMY ENTERED UNIT");
        Unit unit = unitparent.GetComponent<Unit>();
        if (other.gameObject.CompareTag("Enemy"))
        {  
            unit.target = other.gameObject;
         
        }

    }

    private void Update()
    {
        Unit unit = unitparent.GetComponent<Unit>();
        if (!unit.shooting && unit.target != null)
        {
            unit.shooting = true;
            runningCoroutine = unit.shoot(1.5f);
            StartCoroutine(runningCoroutine);
        }
    }


}
