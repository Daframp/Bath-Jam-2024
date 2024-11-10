using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ShurikenSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _shurikenPrefab1;
    [SerializeField]
    private GameObject _shurikenPrefab2;
    [SerializeField]
    private GameObject _shurikenPrefab3;
    [SerializeField]
    private GameObject _shurikenPrefab4;

    private GameObject shuriken;
    [SerializeField]
    private float _shurikenSpeed;

    // Start is called before the first frame update

    private void Start()
    {
        Spawn(2);
    }
    // Update is called once per frame
    void Update()
    {

        //Spawn();
    }
    private void Spawn(int shurikenVariant)
    {


        if (shurikenVariant == 1) { shuriken = Instantiate(_shurikenPrefab1, transform.position, transform.rotation); }
        else if (shurikenVariant == 2) { shuriken = Instantiate(_shurikenPrefab2, transform.position, transform.rotation); }
        else if (shurikenVariant == 3) { shuriken = Instantiate(_shurikenPrefab3, transform.position, transform.rotation); }
        else if (shurikenVariant == 4) { shuriken = Instantiate(_shurikenPrefab4, transform.position, transform.rotation); }

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
    public void NextWave(int waveNumber)
    {
        // implement different waves here
    }
}
