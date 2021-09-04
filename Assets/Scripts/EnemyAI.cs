using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float chaseRange = 25f;
    [SerializeField] float attackRange = 6f;
    [SerializeField] float attackRecoveryTime = 3.0f;
    [SerializeField] float sleepTime = 5f;
    [SerializeField] float baseSpeed = 5f;

    NavMeshAgent navMeshAgent;
    Animator animator;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    bool isAttacking = false;
    bool isSleeping = false;
    float timeSinceAttack = 0.0f;
    float timeSinceSleep = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = baseSpeed;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (isSleeping)
        {
            timeSinceSleep += Time.deltaTime;
            if (timeSinceSleep >= sleepTime)
            {
                isSleeping = false;
                animator.SetBool("sleep", isSleeping);
                timeSinceSleep = 0.0f;
                navMeshAgent.speed = baseSpeed;
            }
        }

        else
        {
            distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (isProvoked)
            {
                EngageTarget();
            }

            else if (distanceToTarget <= chaseRange)
            {
                isProvoked = true;
            }
        }
    }

    void EngageTarget()
    {
        if(distanceToTarget <= chaseRange)
        {
            ChaseTarget();
        }
    }

    void ChaseTarget()
    {
        if (isAttacking)
        {
            timeSinceAttack += Time.deltaTime;
            if(timeSinceAttack >= attackRecoveryTime)
            {
                isAttacking = false;
                animator.SetBool("attack", isAttacking);
                timeSinceAttack = 0.0f;
                navMeshAgent.speed = baseSpeed;
            }
        }

        else
        {
            GetComponentInChildren<Animator>().SetTrigger("move");
            navMeshAgent.SetDestination(target.position);

            if (distanceToTarget <= attackRange && !isAttacking)
            {
                AttackTarget();
            }
        }
    }

    void AttackTarget()
    {
        isAttacking = true;
        animator.SetBool("attack", isAttacking);
        navMeshAgent.speed = baseSpeed * 2.0f;
    }

    public void Sedate(float sedativeStrength)
    {
        float currentSpeed = navMeshAgent.speed;
        if (sedativeStrength >= currentSpeed)
        {
            navMeshAgent.speed = 0.0f;
            isSleeping = true;
            GetComponentInChildren<Animator>().SetBool("sleep", isSleeping);
        }

        else { navMeshAgent.speed -= sedativeStrength; }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f);
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = new Color(1f, 0f, 0f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
