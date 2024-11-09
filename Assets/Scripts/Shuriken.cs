using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            Debug.Log("Player hit");
        }
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) >= 10 || Mathf.Abs(transform.position.y) >= 10)
        {
            Destroy(gameObject);
        }
        
    }
}
