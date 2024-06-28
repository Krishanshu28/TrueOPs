using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyNetwork
{
    public class KeyGateRegulator : MonoBehaviour
    {
        [Header("Animations")]
        private Animator gateAnimations;
        private bool OpenGate = false;
        [SerializeField] private string openAnimationName = "GateOpen";
        [SerializeField] private string closeAnimationName = "GateClose";

        [Header("Time and UI")]
        [SerializeField] private int timeToShowUI = 1;
        [SerializeField] private GameObject showGateLockedUI = null;
        [SerializeField] private KeyList keyList = null;
        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;

        [Header("Sound Effect")]
        public AudioSource audioSource;
        public AudioClip gateSound;

        private void Awake()
        {
            gateAnimations = GetComponent<Animator>();
        }

        public void StartAnimation()
        {
            if(keyList.hasKey)
            {
                Open_Gate();
            }
            else
            {
                StartCoroutine(showGateLocked());
            }
        }

        IEnumerator StopGateInterConnection()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

        void Open_Gate()
        {
            if(!OpenGate && !pauseInteraction)
            {
                gateAnimations.Play(openAnimationName, 0, 0.0f);
                audioSource.PlayOneShot(gateSound);
                OpenGate = true;
                ObjectivesComplaete.occurence.GetObjectivesDone(true, false, false, false);
                StartCoroutine(StopGateInterConnection());
            }
            else if(OpenGate && !pauseInteraction)
            {
                gateAnimations.Play(closeAnimationName, 0, 0.0f);
                audioSource.PlayOneShot(gateSound);
                OpenGate = false;
                StartCoroutine(StopGateInterConnection());
            }
        }

        IEnumerator showGateLocked()
        {
            showGateLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToShowUI);
            showGateLockedUI.SetActive(false);

        }
    }
}
