using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEditor.Experimental.GraphView;
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

    private float x;
    private string direction;
    private GameObject shuriken;
    [SerializeField]
    private float _shurikenSpeed;

    private int numberOfEnemies;
    private int numberOfEmpty;
    private int numberOfDirections;
    private int rnd;
    private List<int> enemies;
    private float spawnerOffSet;


    // Start is called before the first frame update

    private void Start()
    {
        //Spawn(2);
        //NextWave(5);
    }
    // Update is called once per frame
    void Update()
    {

        //Spawn();
    }
    private void Spawn(int shurikenVariant, int waveNumber, bool invincible , float displacement)
    {
        if (shurikenVariant == 0)
        {
            return;
        }
        if (shurikenVariant == 1) { shuriken = Instantiate(Resources.Load("Shuriken") as GameObject, transform.position, transform.rotation); }
        else if (shurikenVariant == 2) { shuriken = Instantiate(Resources.Load("Shuriken2") as GameObject, transform.position, transform.rotation); }
        else if (shurikenVariant == 3) { shuriken = Instantiate(Resources.Load("Shuriken3") as GameObject, transform.position, transform.rotation); }
        else if (shurikenVariant == 4) { shuriken = Instantiate(Resources.Load("Shuriken4") as GameObject, transform.position, transform.rotation); }

        Rigidbody2D rigidbody = shuriken.GetComponent<Rigidbody2D>();
        if (transform.position.x <= -displacement)
        {
            //make direction to the right
            rigidbody.velocity = _shurikenSpeed * Vector2.right;
        }
        else if (transform.position.x >= displacement)
        {
            //make direction to left
            rigidbody.velocity = _shurikenSpeed * Vector2.left;

        }
        else if (transform.position.y >= displacement)
        {
            //make direction downwards
            rigidbody.velocity = _shurikenSpeed * Vector2.down; ;
        }
        else if (transform.position.y <= -displacement)
        {
            //make direction upwards
            rigidbody.velocity = _shurikenSpeed * Vector2.up;
        }
        if (invincible) { shuriken.GetComponent<Shuriken>().Health(1000); }
        else if (waveNumber >= 7)
        {
            if (UnityEngine.Random.Range(1,101) <= waveNumber) { shuriken.GetComponent<Shuriken>().Health(1); }
            else { shuriken.GetComponent<Shuriken>().Health(1); }
        }

        else { shuriken.GetComponent<Shuriken>().Health(1); }
        

    }
    public List<int> ShuffleListWithOrderBy(List<int> list)
    {
        System.Random random = new System.Random();
        return list.OrderBy(x => random.Next()).ToList();
    }
    public void NextWave(int waveNumber, bool dodgeMode, int boardWidth)
    {
        x = (8 * ((float)waveNumber / (float)(waveNumber + 15))) + 5 + UnityEngine.Random.Range(-3,4);
        numberOfEnemies = (int)Math.Round(x);
        rnd = UnityEngine.Random.Range(1, 40);
        numberOfDirections = 3;
        spawnerOffSet = (boardWidth / 2) + 0.5f;



        if (numberOfEnemies >= 15)
        {
            if (rnd <= waveNumber + 10)
            {
                numberOfDirections = 4;
            }
            else
            {
                numberOfDirections = 3;
            }
        }
        else if (numberOfEnemies >= 8)
        {
            if (rnd <= waveNumber + 7)
            {
                numberOfDirections = 4;
            }
            else if (rnd <= waveNumber + 10)
            {
                numberOfDirections = 3;
            }
            else
            {
                numberOfDirections = 2;
            }

        }
        else 
        {
            if (rnd <= waveNumber + 12)
            {
                numberOfDirections = 2;
            }
            else 
            {
                numberOfDirections = 1;
            }
        }
        numberOfEmpty = (numberOfDirections * boardWidth) - numberOfEnemies;

        enemies = new List<int>();
        for (int i = 1; i < numberOfEnemies+1; i++)
        {
            rnd = UnityEngine.Random.Range(1, 101);
            if (rnd == 1)
            {
                enemies.Add(4);
            }
            else if (rnd <= 2+waveNumber)
            {
                enemies.Add(2);
            }
            else if (rnd <= 15+waveNumber)
            {
                enemies.Add(3);
            }
            else 
            {
                enemies.Add(1);
            }
        }
        for (int i = 0; i< numberOfEmpty;i++)
        {
            enemies.Add(0);
        }
        enemies = ShuffleListWithOrderBy(enemies);
        enemies = ShuffleListWithOrderBy(enemies);
        enemies = ShuffleListWithOrderBy(enemies);
        transform.position = new Vector3(spawnerOffSet, -spawnerOffSet, 0.044f);

        if (dodgeMode) 
        {
            enemies.Clear();
            rnd = UnityEngine.Random.Range(1, boardWidth - 3);
            for (int i = 0;i < rnd;i++)
            {
                enemies.Add(1);
            }
            for (int i = 0; i < 3; i++)
            {
                enemies.Add(0);
            }
            for (int i = 0; i < boardWidth - 3 - rnd; i++) 
            { 
                enemies.Add(1);
            }
        }


        direction = "up";
        foreach (int i in enemies) 
        { 
        if (direction == "up") 
            { 
                transform.Translate(new Vector2(0, 1)); 
                if (transform.position.y == spawnerOffSet) {transform.Translate(new Vector2(-1, 0)); direction = "left"; }
            }
        else if (direction == "down") 
            {
                transform.Translate(new Vector2(0, -1));
                if (transform.position.x == -spawnerOffSet) { transform.Translate(new Vector2(1, 0)); direction = "right"; }

            }
        else if (direction == "right") 
            {
                transform.Translate(new Vector2(1, 0));
                if (transform.position.x == spawnerOffSet) { transform.Translate(new Vector2(0, 1)); direction = "up"; }
            }
        else if (direction == "left") 
            {
                transform.Translate(new Vector2(-1, 0));
                if (transform.position.x == -spawnerOffSet) { transform.Translate(new Vector2(0, -1)); direction = "down"; }
            }
            Spawn(i,waveNumber,false , (boardWidth/2) + 0.5f);
        }




        



    }

}
