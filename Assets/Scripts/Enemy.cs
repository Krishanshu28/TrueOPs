using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    //[Header("Enemy Health and Damage")]

    [Header("Enemy things")]
    public NavMeshAgent enemyAgent;
    public Transform playerBody;
    public LayerMask PlayerLayer;
    
    [Header("Enemy Guarding Var")]
    public GameObject[] walkPoints;
    int curentEnemyPosition = 0;
    public float enemySpeed;
    float walkingPointRadius = 2;

    //[Header("Sounds and UI")]

    //[Header("Enemy Shooting Var")]

    //[Header("Enemy Animation and Spark effect")]

    [Header("Enemy mood/situation")]
    public float visionRadius;
    public float shootingRadius;
    public bool playerInvisionRadius;
    public bool playerInshootRadius;

    private void Awake()
    {       
        playerBody = GameObject.Find("Player").transform;
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() 
    {
        playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInshootRadius = Physics.CheckSphere(transform.position, shootingRadius, PlayerLayer);
        
        if(!playerInvisionRadius && !playerInshootRadius) Guard();
    }

    private void Guard()
    {
        if(Vector3.Distance(walkPoints[curentEnemyPosition].transform.position, transform.position) < walkingPointRadius)
        {
            curentEnemyPosition = Random.Range(0, walkPoints.Length);
            if(curentEnemyPosition >= walkPoints.Length)
            {
                curentEnemyPosition =0;
            }
            transform.position = Vector3.MoveTowards(transform.position, walkPoints[curentEnemyPosition].transform.position, Time.deltaTime * enemySpeed);
            //changing enemy facing
            transform.LookAt(walkPoints[curentEnemyPosition].transform.position);

        }
    }
}