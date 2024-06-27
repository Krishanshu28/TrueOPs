using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTurnOff : MonoBehaviour
{
    [Header("Generator Lights and Button")]
    public GameObject greenLight;
    public GameObject redLight;
    public bool button;

    [Header("Generator Sound Effects and Radius")]
    private float radius = 2f;
    public PlayerScript player;
    public Animator animator;
    public AudioSource audioSource;

    private void Awake()
    {
        button = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && Vector3.Distance(transform.position, player.transform.position) < radius) 
        {
            button = true;
            animator.enabled = false;
            greenLight.SetActive(false);
            redLight.SetActive(true);
            audioSource.Stop();
            //objective completed
        }
        else if(button == false)
        {
            greenLight.SetActive(true);
            redLight.SetActive(false);
        }
    }

}
