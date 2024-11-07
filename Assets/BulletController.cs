using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifetime = 5f;
    private bool piercing = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit an Enemy!");

            if (!piercing)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetPiercing(bool value)
    {
        piercing = value;
    }
}
