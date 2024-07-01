using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _crossHair;
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCrosshair();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    public void MoveCrosshair()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            _crossHair.transform.position = _camera.WorldToScreenPoint(hit.point);
            //_shootEffect.transform.position = hit.point;
        }

        /*

        Vector3 mousePos = Input.mousePosition;

        mousePos.z = _camera.nearClipPlane; // Ajustar si es necesario
        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mousePos);

        _crossHair.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
        */
    }

    public void Shoot()
    {
        //_shootEffect.Play();

        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("EnemyCollider")))
        {
            Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            //_shootEffect.transform.position = hit.point;
            //_shootEffect.transform.forward = hit.normal;
            //_shootEffect.Play();

            string colliderTag = hit.collider.gameObject.tag;                                           //Guardo el tag en una variable, y eso lo paso al enum, para poder pasarlo como parametro
            string manteca = colliderTag.Substring(8);                          //Aca la cague  y tengo que borrar la palabra "Collider" del tag al inicio

            Enemy.EnemyPart bodyTag = (Enemy.EnemyPart)Enum.Parse(typeof(Enemy.EnemyPart), manteca);

            enemy.DoDamage(bodyTag, hit);

        }
    }
}
