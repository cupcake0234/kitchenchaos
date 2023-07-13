using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    chop,
    deliveryFail,
    deliverySuccess,
    footStep,
    objectDrop,
    objectPickUp,
    panSizzle,
    trash,
    warning
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OptionUI.Instance.OnSoundVolumeChange += (newVolume) => volume = newVolume;
    }

    public void PlaySound(SoundType soundType, Vector3 position)
    {
        switch (soundType)
        {
            case SoundType.chop:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.chop[Random.Range(0, audioClipRefsSO.chop.Length)], position, volume);
                break;
            case SoundType.deliveryFail:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.deliveryFail[Random.Range(0, audioClipRefsSO.deliveryFail.Length)], position, volume);
                break;
            case SoundType.deliverySuccess:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.deliverySuccess[Random.Range(0, audioClipRefsSO.deliverySuccess.Length)], position, volume);
                break;
            case SoundType.footStep:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.footStep[Random.Range(0, audioClipRefsSO.footStep.Length)], position, volume);
                break;
            case SoundType.objectDrop:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.objectDrop[Random.Range(0, audioClipRefsSO.objectDrop.Length)], position, volume);
                break;
            case SoundType.objectPickUp:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.objectPickUp[Random.Range(0, audioClipRefsSO.objectPickUp.Length)], position, volume);
                break;
            case SoundType.panSizzle:
                break;
            case SoundType.trash:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.trash[Random.Range(0, audioClipRefsSO.trash.Length)], position, volume);
                break;
            case SoundType.warning:
                AudioSource.PlayClipAtPoint(audioClipRefsSO.warning[Random.Range(0, audioClipRefsSO.warning.Length)], position, volume);
                break;
            default:
                break;
        }
    }
}
