using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalObjective : MonoBehaviour
{
    [Header("Vechile Button")]
    [SerializeField] private KeyCode vechicleButton = KeyCode.F;

    [Header("Vechile Sound Effects and Radius")]
    private float radius = 3f;
    public PlayerScript player;
    public AudioSource audioSource;
    public AudioClip objectiveCompletedSound;



    private void Update()
    {
        if (Input.GetKeyDown(vechicleButton) && Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            audioSource.PlayOneShot(objectiveCompletedSound);
            Time.timeScale = 1f;
            SceneManager.LoadScene("EndGameMenu");
            ObjectivesComplaete.occurence.GetObjectivesDone(true, true, true, true);
        }
        
    }
}
