using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal : Resource
{

    private int unitsEntered = 0;//Number of units that are trying to harvest this resource
    private IEnumerator runningCoroutine;//The coroutine that's running right now

    public Coal()
    {
        this.stamina = 100;
        this.yield = 10;
        this.collectionSpeed = 10;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("UNIT ENTERED COAL");

        isBusy = true;

        if (other.gameObject.CompareTag("Units") && (other.gameObject.GetComponent<Unit>().type == UnitData.UnitType.scavanger))
        {
            unitsEntered++;
            runningCoroutine = depleteStamina(1.0f);
            StartCoroutine(runningCoroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("UNIT EXITED COAL");

        if (other.gameObject.CompareTag("Units") && (other.gameObject.GetComponent<Unit>().type == UnitData.UnitType.scavanger))
        {
            unitsEntered--;
            StopCoroutine(runningCoroutine);
        }
    }

    IEnumerator depleteStamina(float delay)
    {

        while (stamina > 0)
        {
            stamina -= collectionSpeed;
            yield return new WaitForSeconds(delay); //Resources should be fully collected after a set amount of seconds
        }

        isBusy = false;
        this.depleteStamina(); //method called from parent to trigger the event


    }

}
