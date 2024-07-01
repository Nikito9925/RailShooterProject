using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _crossHair;

    void Start()
    {
        _camera = Camera.main;
    }

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
        }
    }

    public void Shoot()
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("EnemyCollider")))
        {
            Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            ShieldController shield = hit.collider.gameObject.GetComponent<ShieldController>();

            if(shield != null)
            {
                shield.DoDamage(2);
            }
            else
            {
                string colliderTag = hit.collider.gameObject.tag;                   //Guardo el tag en una variable, y eso lo paso al enum, para poder pasarlo como parametro
                string manteca = colliderTag.Substring(8);                          //Aca la cague  y tengo que borrar la palabra "Collider" del tag al inicio

                Enemy.EnemyPart bodyTag = (Enemy.EnemyPart)Enum.Parse(typeof(Enemy.EnemyPart), manteca);

                enemy.DoDamage(bodyTag, hit);
            }


        }
    }
}
