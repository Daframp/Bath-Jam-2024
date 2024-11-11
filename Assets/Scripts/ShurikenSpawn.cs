using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ShurikenSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _shurikenPrefab ;
    [SerializeField]
    private float _shurikenSpeed;

    // Start is called before the first frame update

    private void Start()
    {
        //Spawn();
    }
    // Update is called once per frame
    void Update()
    {
        
        //Spawn();
    }
    private void Spawn() 
    {
        GameObject shuriken = Instantiate(_shurikenPrefab, transform.position,transform.rotation);
        Rigidbody2D rigidbody = shuriken.GetComponent<Rigidbody2D>();
        if (transform.position.x <= -5.5)
        {
            //make direction to the right
            rigidbody.velocity = _shurikenSpeed * Vector2.right;
        }
        else if (transform.position.x >= 5.5)
        {
            //make direction to left
            rigidbody.velocity = _shurikenSpeed * Vector2.left;

        }
        else if (transform.position.y >= 5.5)
        {
            //make direction downwards
            rigidbody.velocity = _shurikenSpeed * Vector2.down; ;
        }
        else if (transform.position.y <= -5.5)
        {
            //make direction upwards
            rigidbody.velocity = _shurikenSpeed * Vector2.up;
        }

    }

    public void NextWave(int waveNumber,bool dodgeMode)
    {
        // implement different waves here
    }
}
