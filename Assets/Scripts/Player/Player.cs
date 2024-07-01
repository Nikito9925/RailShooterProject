using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] public Movement _movement;
    [SerializeField] public CameraController _cameraController;



    private void Awake()
    {
        if(_agent == null) _agent = GetComponent<NavMeshAgent>();
        _movement = new Movement(_walkSpeed, _runSpeed, _agent);
    }

    void Update()
    {
        _movement.ArtificialUpdate();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //_cameraController.Shoot();
        }
    }
}
