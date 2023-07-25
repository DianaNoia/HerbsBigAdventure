using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controller for purple enemy type
public class EnemyControllerPurple : MonoBehaviour
{
    [SerializeField]
    private float timer;

    private AudioManager am;

    [SerializeField]
    private int laserAudio;

    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed;

    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private int currentPatrolPoint;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    public Animator anim;

    // States of the enemies
    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isShooting
    };

    [SerializeField]
    private AIState currentState;

    [SerializeField]
    private float waitAtPoint = 2f;
    private float waitCounter;

    [SerializeField]
    public float chaseRange;

    [SerializeField]
    private float shootingRange = 1f;

    [SerializeField]
    private float timeBetweenAttacks = 2f;
    private float attackCounter;

    private bool alreadyAttacked,
                    playerInSightRange,
                    playerInAttackRange;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoint;
        am = FindObjectOfType<AudioManager>();
    }


    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);

        switch (currentState)
        {
            case AIState.isIdle:
                anim.SetBool("IsMoving", false);
                if (waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    currentState = AIState.isPatrolling;
                    agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                }

                if (distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.isChasing;
                    anim.SetBool("IsMoving", true);
                }

                break;

            case AIState.isPatrolling:
                // agent.SetDestination(patrolPoints[currentPatrolPoint].position);

                if (agent.remainingDistance <= .2f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }

                    //agent.SetDestination(patrolPoints[currentPatrolPoint].position);
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;

                }

                if (distanceToPlayer <= chaseRange)
                {
                    currentState = AIState.isChasing;
                }

                anim.SetBool("IsMoving", true);
                break;

            case AIState.isChasing:
                agent.SetDestination(PlayerController.instance.transform.position);

                if (distanceToPlayer <= shootingRange)
                {
                    currentState = AIState.isShooting;
                    anim.SetTrigger("Blast Attack");

                    anim.SetBool("IsMoving", false);

                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;

                    attackCounter = timeBetweenAttacks;
                }

                if (distanceToPlayer > chaseRange)
                {
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;

                    agent.velocity = Vector3.zero;
                    agent.SetDestination(transform.position);
                }

                break;

            case AIState.isShooting:
                transform.LookAt(PlayerController.instance.transform, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

                bulletTime -= Time.deltaTime;

                attackCounter -= Time.deltaTime;

                if (attackCounter <= 0)
                {
                    if (distanceToPlayer < shootingRange)
                    {
                        anim.SetTrigger("Blast Attack");

                        ShootAtPlayer();

                        Debug.Log("I've shot");
                        attackCounter = timeBetweenAttacks;
                    }
                    else
                    {
                        currentState = AIState.isIdle;
                        waitCounter = waitAtPoint;

                        agent.isStopped = false;
                    }
                }
                break;
        }
    }

    void ShootAtPlayer()
    {
        am.PlaySFX(laserAudio);

        if (bulletTime > 0)
            return;
        bulletTime = timer;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, Quaternion.Euler(spawnPoint.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();

        bulletRig.AddForce(bulletRig.transform.forward * enemySpeed);
        Destroy(bulletObj, 2f);
    }
}

