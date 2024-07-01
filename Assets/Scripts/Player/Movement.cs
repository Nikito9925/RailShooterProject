using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Movement
{
    [Header("Values")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _speed;

    [Header("References")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Vector3 _target;

    public Movement(float walkSpeed, float runSpeed, NavMeshAgent agent)
    {
        _walkSpeed = walkSpeed;
        _runSpeed = runSpeed;
        _agent = agent;
    }

    public void ArtificialUpdate()
    {

    }

    public void SetTarget(Vector3 newTarget)
    {
        _agent.SetDestination(newTarget);
    }
}
