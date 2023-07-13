using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopSoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLoopSound()
    {
        audioSource.Play();
    }
    public void StopLoopSound()
    {
        audioSource.Stop();
    }
}
