using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour
{
    [Header("Computer On/Off")]
    public bool lightsOn = true;
    public float radius = 2.5f;
    public Light lights;

    [Header("Computer Assign Things")]
    public PlayerScript player;

    [Header("Sounds")]
    public AudioClip objectiveCompletedSound;
    public AudioSource audioSource;

    [SerializeField] private GameObject computerUI;
    [SerializeField] private int showComputerUIFor = 5;

    private void Awake()
    {
        lights = GetComponent<Light>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position,player.transform.position) < radius)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(ShowComputerUI());
                lightsOn = false;
                lights.intensity = 0;
                ObjectivesComplaete.occurence.GetObjectivesDone(true, true, false, false);
                //objective completed
                audioSource.PlayOneShot(objectiveCompletedSound);
            }
        }
    }

    IEnumerator ShowComputerUI()
    {
        computerUI.SetActive(true);
        yield return new WaitForSeconds(showComputerUIFor);
        computerUI.SetActive(false);
    }

}
