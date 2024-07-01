using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] public int life;
    [SerializeField] public bool alive;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;   
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0 && alive)
        {
            alive = false;
            DestroyShield();
        }
    }

    public void DestroyShield()
    {
        Debug.Log("CHAU ESCUDO");
        EnemyShield enemy = gameObject.GetComponentInParent<EnemyShield>();

        if(enemy != null && !enemy._isDeath)
        {
            enemy.BackToNormal();
        }

        Destroy(gameObject);
    }

    public void DoDamage(int dmg)
    {
        life -= dmg;
    }
}
