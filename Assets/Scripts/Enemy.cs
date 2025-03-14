using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Health and Damage")]
    private float enemyHealth = 120f;
    private float presentHealth;
    private float giveDamage = 5f;
    public HealthBar healthBar;


    [Header("Enemy things")]
    public NavMeshAgent enemyAgent;
    public Transform LookPoint;
    public Camera ShootingRaycastArea;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    
    [Header("Enemy Guarding Var")]
    public GameObject[] walkPoints;
    int curentEnemyPosition = 0;
    public float enemySpeed;
    float walkingPointRadius = 5;

    [Header("Sounds and UI")]
    public AudioSource audioSource;
    public AudioClip shootingSound;

    [Header("Enemy Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;

    [Header("Enemy Animation and Spark effect")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Enemy mood/situation")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootRadius;


   
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        presentHealth = enemyHealth;
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
        healthBar.GivefullHealth(enemyHealth);
        
    }

    private void Update() 
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);
        
        if(!playerInvisionRadius && !playerInshootRadius) Guard();
        if (playerInvisionRadius && !playerInshootRadius) PursuePlayer();
        if (playerInvisionRadius && playerInshootRadius) Shootplayer();
        
    }

    private void Guard()
    {
        

        if (Vector3.Distance(walkPoints[curentEnemyPosition].transform.position, transform.position) < walkingPointRadius)
        {
            
            //print("guard");
            //print(walkPoints[curentEnemyPosition]);
            curentEnemyPosition = Random.Range(0, walkPoints.Length);
            if(curentEnemyPosition >= walkPoints.Length)
            {
                curentEnemyPosition = 0;
            }
            //print(walkPoints[curentEnemyPosition]);
            
           
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPoints[curentEnemyPosition].transform.position, Time.deltaTime * enemySpeed);
        //changing enemy facing
        transform.LookAt(walkPoints[curentEnemyPosition].transform.position);
    }

    private void PursuePlayer()
    {
        if(enemyAgent.SetDestination(playerBody.position))
        {
            //animations
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", true);
            anim.SetBool("Shoot", false);
            
            anim.SetBool("Die", false);

            //++ shoting and vision radius
            visionRadius = 30;
            shootingRadius = 16;
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            
            anim.SetBool("Die", true);
        }

    }

    private void Shootplayer()
    {
        enemyAgent.SetDestination(transform.position);

        transform.LookAt(LookPoint);

        if(!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);
            RaycastHit hit;
            if(Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                //Debug.Log("Shooting" + hit.transform.name);

                PlayerScript playerBody = hit.transform.GetComponent<PlayerScript>();
                
                if(playerBody != null)
                {
                    playerBody.playerHitDamage(giveDamage);
                }
                anim.SetBool("Shoot", true);
                anim.SetBool("Walk", false);
                anim.SetBool("AimRun", false);
                
                anim.SetBool("Die", false);

            }
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    public void enemyHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        //++ shoting and vision radius
        visionRadius = 50;
        shootingRadius = 20;

        if (presentHealth <= 0)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("AimRun", false);
            anim.SetBool("Shoot", false);
            anim.SetBool("Die", true);
            EnemyDie();
        }
    }

    private void EnemyDie()
    {
        enemyAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        playerInshootRadius = false;
        playerInvisionRadius = false;
        Object.Destroy(gameObject, 5.0f);
    }
}