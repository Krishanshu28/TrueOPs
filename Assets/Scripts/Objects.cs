using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 120f;

    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;
        if(objectHealth <= 0f)
        {
            //destroy the gameobject
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
