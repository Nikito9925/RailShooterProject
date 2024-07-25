using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrilExplosivo : MonoBehaviour
{
    public float radioExplosion = 5f;
    public int danoEnemigos = 10;
    public GameObject efectoExplosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RaycastBullet"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(efectoExplosion, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radioExplosion);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DoDamage(Enemy.EnemyPart.Chest, new RaycastHit());
                }
            }
        }

        Destroy(gameObject);
    }
}