using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingUnit : Unit
{

    private int offsetFromGround = 5;

    private void Awake()
    {
        Vector3 newPos = this.transform.position + new Vector3(0f, offsetFromGround, 0f);
        this.transform.position = newPos;
    }


    private void Update()
    {
        this.selectedArea.transform.position = this.transform.position + new Vector3(0f, 2f, 0f);
    }
}
