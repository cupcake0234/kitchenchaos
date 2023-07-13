using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        OptionUI.Instance.OnMusicVolumeChange += (newVolume) => audioSource.volume = newVolume;
    }
}
