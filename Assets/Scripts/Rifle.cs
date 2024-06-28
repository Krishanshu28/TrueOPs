 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
   [Header("Rifle Things")]
   public Camera came;
   public float giveDamageOf = 10f;
   public float shootingRange = 100f;
   public float fireCharge = 15f;
   public Animator animator;
   public PlayerScript player;
    

   [Header("Rifle Ammunition and shooting")]
   private int maximumAmmunition = 20;
   private int mag = 15;
   private int presentAmmunition;
   public float reloadingTime = 1.3f;
   private bool setReloading = false;
   private float nextTimeToShoot = 0f;

   [Header("Rifle Effects")]
   public ParticleSystem muzzleSpark;
   public GameObject imapactEffect;
   public GameObject droneEffect;
    public GameObject goreEffect;

    [Header("Sounds and UI")]
    [SerializeField] private GameObject ammoOutUI;
    [SerializeField] private int timeToShowUI = 1;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;
    public AudioSource audioSource;
    

    private void Awake() 
   {        
        presentAmmunition = maximumAmmunition;
   }

    // Update is called once per frame
    void Update()
    {
        if(setReloading)
        return;

        if(presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if(Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Reloading", false);
            nextTimeToShoot = Time.time + 1f/fireCharge;
            Shoot();
        }

        else if(Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }

        else if(Input.GetButton("Fire2") && Input.GetButton("Fire1"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("FireWalk", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        
        else if(Input.GetButton("Fire1"))
        {
            animator.SetBool("Fire", true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Fire", false);
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Reloading", false);
            
        }
    }

    void Shoot()
    {
        if(mag == 0)
        {
            StartCoroutine(ShowAmmoOut());
            return;
        }

        presentAmmunition--;

        if(presentAmmunition == 0)
        {
            mag --;
        }

        //updating UI
        AmmoCount.occurrence.UpdateAmmoText(presentAmmunition);
        AmmoCount.occurrence.UpdateMagText(mag);

        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);
        RaycastHit hitinfo;
        if(Physics.Raycast(came.transform.position, came.transform.forward, out hitinfo, shootingRange))
        {
            //Debug.Log(hitinfo.transform.name);

            Objects objects = hitinfo.transform.GetComponent<Objects>();  
            Enemy enemy = hitinfo.transform.GetComponent<Enemy>();
            EnemyDrone enemyDrone = hitinfo.transform.GetComponent<EnemyDrone>();
            if(objects != null)
            {
                objects.objectHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(imapactEffect, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactGO, 1f);
            }
            else if (enemy != null)
            {
                enemy.enemyHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(goreEffect, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactGO, 2f);
            }
            else if(enemyDrone != null)
            {
                enemyDrone.enemyDroneHitDamage(giveDamageOf);
                GameObject impactGO = Instantiate(droneEffect, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
                Destroy(impactGO, 2f);
            }

        }
    }

    IEnumerator Reload()
    {
        player.playerSpeed = 0f;
        player.playerSprint = 0f;
        setReloading = true;
        audioSource.PlayOneShot(reloadingSound);
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadingTime);
        animator.SetBool("Reloading", false);
        presentAmmunition = maximumAmmunition;
        player.playerSpeed = 2f;
        player.playerSprint = 3f;
        setReloading = false;
    }

    IEnumerator ShowAmmoOut()
    {
        ammoOutUI.SetActive(true);
        yield return new WaitForSeconds(timeToShowUI);
        ammoOutUI.SetActive(false);
    }
}
