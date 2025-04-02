using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_Camera : MonoBehaviour
{
    private Transform player;
    
    private void Start()
    {
        player = GameObject.FindWithTag("Units").transform;
        Debug.Assert(player != null);
    }

    private void LateUpdate()
    {
        //Debug.Log(player.position);
        Vector3 newPos = player.position;
        newPos.y  = transform.position.y;
        transform.position = newPos;
    }
}
