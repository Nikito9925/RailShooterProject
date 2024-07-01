using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _originalDir = Vector3.forward;
    [SerializeField] private Vector3 _lookDir;
    [SerializeField] private Transform _target;
    [SerializeField] private ParticleSystem _shootEffect;
    [SerializeField] private LayerMask _enemyLayer;

    [SerializeField] private GameObject _crossHair;

    [Header("Values")]
    [SerializeField] public bool _focus;

    public CameraController(Camera camera, Transform focusTaget, ParticleSystem shootEffect, Transform playerTransform)
    {
        _camera = camera;
        _shootEffect = shootEffect;
        _playerTransform = playerTransform;
        _lookDir = _playerTransform.forward;
    }

    public void Start()
    {
        SetTarget(transform);
    }
    public void Update()
    {
        if (_focus && _target != null) CalculateDir();
        else RemoveTarget();

        MoveCamera();
        Focus();

        //MoveCrosshair();
    }

    public void Focus() //PASAR VECTOR DIRECCION
    {
        if(_target != null) _camera.transform.forward = Vector3.Lerp(_camera.transform.forward, _lookDir, .95f * Time.deltaTime * 5);
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    public void RemoveTarget()
    {
        _lookDir = _playerTransform.forward;
    }

    public void CalculateDir()
    {
        _lookDir = (_target.position - transform.position).normalized;
    }


    public void Shoot()
    {
        //_shootEffect.Play();

        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("EnemyCollider")))
        {
            Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            _shootEffect.transform.position = hit.point;
            _shootEffect.transform.forward = hit.normal;
            _shootEffect.Play();

            string colliderTag = hit.collider.gameObject.tag;                                           //Guardo el tag en una variable, y eso lo paso al enum, para poder pasarlo como parametro
            string manteca = colliderTag.Substring(8);                          //Aca la cague  y tengo que borrar la palabra "Collider" del tag al inicio

            Enemy.EnemyPart bodyTag = (Enemy.EnemyPart)Enum.Parse(typeof(Enemy.EnemyPart), manteca);

            enemy.DoDamage(bodyTag, hit);

        }
    }

    public void MoveCamera()
    {
        transform.position = _playerTransform.position + Vector3.up * 1.8f;
    }

    public void Aim()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            _shootEffect.transform.position = hit.point;
        }

        //WorldToScreenPoint
    }

    public void MoveCrosshair()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            _crossHair.transform.position = _camera.WorldToScreenPoint(hit.point);
            _shootEffect.transform.position = hit.point;
        }

        /*

        Vector3 mousePos = Input.mousePosition;

        mousePos.z = _camera.nearClipPlane; // Ajustar si es necesario
        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mousePos);

        _crossHair.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        */
    }
}
