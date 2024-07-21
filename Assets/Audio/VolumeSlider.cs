using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [Header("Values")]
    [Range(0f, 1f)][SerializeField] private float _initMasterVolume = 1f;
    [Range(0f, 1f)][SerializeField] private float _initMusicVolume = .2f;
    [Range(0f, 1f)][SerializeField] private float _initSFXVolume = .75f;

    [Header("UI")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    public void Start()
    {
        SetMasterVolume(_initMasterVolume);
        _masterSlider.value = _initMasterVolume;

        SetMusicVolume(_initMusicVolume);
        _musicSlider.value = _initMusicVolume;

        SetSFXVolume(_initSFXVolume);
        _sfxSlider.value = _initSFXVolume;
    }


    public void SetMasterVolume(float value)
    {
        MusicManager.Instance.SetMasterVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        MusicManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        MusicManager.Instance.SetSFXVolume(value);
    }
}
