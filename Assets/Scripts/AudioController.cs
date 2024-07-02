using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public AudioMixerGroup audioGroup; // Asigna el grupo de audio desde el Inspector

    // Referencia al AudioSource
    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip reloadVoiceSound;

    private void Start()
    {
        // Obtén o crea un AudioSource en el mismo GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound, .5f);
    }
    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(reloadSound);
    }
    public void PlayReloadVoiceSound()
    {
        audioSource.PlayOneShot(reloadVoiceSound);
    }
}