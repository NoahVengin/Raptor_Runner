using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemySpeedManager : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }

    public void Sedate(float sedativeStrength)
    {
        float currentSpeed = navMeshAgent.speed;
        if (sedativeStrength > currentSpeed) { navMeshAgent.speed = 0.0f; }

        else { navMeshAgent.speed -= sedativeStrength; }
    }

}
