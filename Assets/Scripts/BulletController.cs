using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifetime = 5f;
    private bool piercing = false;
    
    //Using game controllers audio source so audio doesnt end as soon as bullet gets destroyed
    public AudioSource audioSource;
    public AudioClip enemyHitSound;
    private float damage;

    void Start()
    {
        Destroy(gameObject, lifetime);
        audioSource = GetComponent<AudioSource>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit an Enemy!");
            other.GetComponent<Shuriken>().enemyHit(damage);
            
            GameObject soundObject = new GameObject("DestroySound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            // Configure the audio source
            audioSource.clip = enemyHitSound;
            audioSource.Play();

            // Destroy the sound object after the clip has finished playing
            Destroy(soundObject, enemyHitSound.length);

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

    public void SetDamage(float value){
        damage = value;
    }
}
