using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip battleMusic;
    public AudioClip menuMusic;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(battleMusic);
    }

    void Update()
    {
        
    }
}
