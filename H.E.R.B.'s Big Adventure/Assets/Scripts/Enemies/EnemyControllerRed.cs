using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Controller for red enemy type
public class EnemyControllerRed : MonoBehaviour
{
    private EnemyHealthManager ehm;

    private AudioManager am;

    [SerializeField]
    private int explodeAudio;

    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private int currentPatrolPoint;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    public Animator anim, animatorForEffect;

    [SerializeField]
    private GameObject hurtPlayer;

    [SerializeField]
    private GameObject aboutToExplode, explosionEffect;

    public float blinkInterval;

    // States of the enemies
    public enum AIState
    {
        isIdle, isPatrolling, isChasing, isExploding
    };

    [SerializeField]
    private AIState currentState;

    [SerializeField]
    private float waitAtPoint = 2f;
    private float waitCounter;

    [SerializeField]
    public float chaseRange;

    [SerializeField]
    private float explodingRange;

    //[SerializeField]
    //private float timeBetweenAttacks = 2f;
    //private float attackCounter;

    // Start is called before the first frame update
    void Start()
    {
        ehm = FindObjectOfType<EnemyHealthManager>();
        am = FindObjectOfType<AudioManager>();

        waitCounter = waitAtPoint;
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

                if (distanceToPlayer <= explodingRange)
                {
                    currentState = AIState.isExploding;
                    //anim.SetTrigger("Bury");

                    //anim.SetBool("IsMoving", false);

                    //agent.velocity = Vector3.zero;
                    //agent.isStopped = true;

                    //attackCounter = timeBetweenAttacks;
                }

                if (distanceToPlayer > chaseRange)
                {
                    currentState = AIState.isIdle;
                    waitCounter = waitAtPoint;

                    agent.velocity = Vector3.zero;
                    agent.SetDestination(transform.position);
                }

                break;

            case AIState.isExploding:
                transform.LookAt(PlayerController.instance.transform, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

                if (distanceToPlayer <= explodingRange)
                {
                    aboutToExplode.SetActive(true);

                    animatorForEffect.SetTrigger("Bury");

                    anim.SetTrigger("Bury");

                    StartCoroutine(BlinkAndDestroy());
                }
                break;
        }
    }

    private IEnumerator BlinkAndDestroy()
    {
        

        yield return new WaitForSeconds(2f);
        
        am.PlaySFX(explodeAudio);

        hurtPlayer.SetActive(true);

        explosionEffect.SetActive(true);

        yield return new WaitForSeconds(1f);

        hurtPlayer.SetActive(false);

        Destroy(gameObject);
    }
}
