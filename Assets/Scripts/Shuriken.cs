using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [SerializeField]
    private int shurikenType;
    [SerializeField]
    private float shuriken2Frequency;

    private float counter;

    private Rigidbody2D rigidbody;

    private Transform _player;
    private Vector2 directionToPlayer;
    [SerializeField]
    public float health = 1f;


    private void Awake()
    {
        counter = 0;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<PlayerController>().transform;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            collision.GetComponent<PlayerController>().PlayerHit();
            Debug.Log("Player hit");
        }
    }


    private void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, directionToPlayer);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 100);
        rigidbody.SetRotation(rotation);
    }

    private void setVelocity()
    {
        rigidbody.velocity = transform.up * 2;

    }

    private void FixedUpdate()
    {

        if (Mathf.Abs(transform.position.x) >= 10 || Mathf.Abs(transform.position.y) >= 10)
        {
            Destroy(gameObject);
        }

        if (shurikenType == 2)
        {
            if (rigidbody.velocity.normalized == new Vector2(1, 0) || rigidbody.velocity.normalized == new Vector2(-1, 0))
            {
                float displace = Mathf.Sin(counter + shuriken2Frequency) - Mathf.Sin(counter);
                transform.Translate(new Vector2(0, displace));
                counter += shuriken2Frequency;
            }
            else if (rigidbody.velocity.normalized == new Vector2(0, 1) || rigidbody.velocity.normalized == new Vector2(0, -1))
            {
                float displace = Mathf.Sin(counter + shuriken2Frequency) - Mathf.Sin(counter);
                transform.Translate(new Vector2(displace, 0));
                counter += shuriken2Frequency;
            }
        }

        else if (shurikenType == 3)
        {
            if (Mathf.Abs(transform.position.x) >= 6.3 || Mathf.Abs(transform.position.y) >= 6.3)
            {
                if (counter != 1)
                {
                    rigidbody.velocity = rigidbody.velocity * -1;
                    counter = counter + 1;


                }
            }
        }

        else if (shurikenType == 4)
        {
            Vector2 enemyToPlayer = _player.position - transform.position;
            directionToPlayer = enemyToPlayer.normalized;
            RotateTowardsTarget();
            setVelocity();


        }
    }

    public void enemyHit(float damage){
        health -= damage;
        if (health <= 0){
            Destroy(gameObject);
        }
    }
    public void Health(float healthP)
    {
        health = healthP;
    }
}
