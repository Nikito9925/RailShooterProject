using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [SerializeField] private AudioClip _clipToPlay;
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.Instance.PlayMusicClip(_clipToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
