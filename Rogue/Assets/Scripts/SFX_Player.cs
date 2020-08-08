using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SFX_Player : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayRandomSound()
    {
        audioSource.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
