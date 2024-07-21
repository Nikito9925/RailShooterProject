using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] public int _life;

    [Header("References")]
    [SerializeField] private Camera _camera;
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] public Movement _movement;
    [SerializeField] public CameraController _cameraController;
    [SerializeField] public CanvasController _canvas;



    private void Awake()
    {
        if(_agent == null) _agent = GetComponent<NavMeshAgent>();
        _movement = new Movement(_walkSpeed, _runSpeed, _agent);
        //_life = 5;
    }

    void Update()
    {
        _movement.ArtificialUpdate();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            //_cameraController.Shoot();
        }
    }

    public void DoDamage()
    {
        _life--;
        _canvas.UpdateCanvas(_life, GetComponent<Combat>()._bullets);
    }
}
