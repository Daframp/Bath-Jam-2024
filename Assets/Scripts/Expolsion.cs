using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expolsion : MonoBehaviour
{
    private Vector3 center;
    private float radius = 1;
    [SerializeField] private GameObject explosionParticleEffect;
    private AudioSource audioSource;
    private AudioClip explosionSound;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        explosionSound = Resources.Load<AudioClip>("SoundEffects/Explosion");
    }

    private void Awake()
    {
        center = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Explode();
        
    }
    public void SetCenter(Vector3 v)
    {
        center = v;
    }
    public void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        GameObject self = new GameObject();
        foreach (var hitCollider in hitColliders)
        {
            GameObject temp = hitCollider.gameObject;
            if (temp.tag != "Player" && temp.tag != "Background")
            {
                Destroy(temp);
            }
            else if (temp.tag == "Explosive")
            {
                self = temp;
            }
            else
            {
                if (temp.tag == "Player")
                {
                    temp.GetComponent<PlayerController>().SetHealth(0);
                }
            }
        }
        // GameObject particleInstance = Instantiate(Resources.Load("explosionParticleEffect") as GameObject, new Vector3(transform.position.x, transform.position.y, -10), Quaternion.identity);
        // float lifetime = particleInstance.GetComponent<ParticleSystem>().main.duration;
        // Destroy(particleInstance, lifetime);


        Debug.Log(explosionSound);
        GameObject soundObject = new GameObject("DestroySound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();

            // Configure the audio source
            audioSource.clip = explosionSound;
            audioSource.Play();

            // Destroy the sound object after the clip has finished playing
            Destroy(soundObject, explosionSound.length); //, explosionSound.length This isnt working 

        Destroy(self);
    }
}
