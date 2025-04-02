using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetime = 5f;

    public delegate void HealthChanged(int healthLeft);
    public event HealthChanged healthChanged;

    private void Start()
    {
        Debug.Log("Bullet Fired");
    }
    void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        Debug.Log(target);
        //Check if the bullet hit a target
        if (target != null && (collision.gameObject.CompareTag("Units") || collision.gameObject.CompareTag("Building")))
        {
            target.takeDamage();
            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
