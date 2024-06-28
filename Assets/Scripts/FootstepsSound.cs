using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSound : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("FootStep Sources")]
    [SerializeField] private AudioClip[] footStepsSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomFootSteps()
    {
        return footStepsSound[Random.Range(0, footStepsSound.Length)];
    }

    private void Step()
    {
        AudioClip clip =  GetRandomFootSteps();
        audioSource.PlayOneShot(clip);
    }
}
