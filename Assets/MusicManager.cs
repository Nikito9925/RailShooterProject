using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    #region Singleton
    public static MusicManager Instance;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _audioSource;

    public void PlayMusicClip(AudioClip clip)
    {
        if(_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }

        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0) value = 0.0001f;
        _audioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0) value = 0.0001f;
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0) value = 0.0001f;
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }
}
