using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyAudioController : MonoBehaviour
{
    public AudioMixerGroup audioGroup; // Asigna el grupo de audio desde el Inspector

    // Referencia al AudioSource
    private AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip damageSound;

    private void Start()
    {
        // Obtén o crea un AudioSource en el mismo GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }   
    public void PlayEnemyAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }
    public void PlayEnemyDeahtSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void PlayEnemyDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }

}