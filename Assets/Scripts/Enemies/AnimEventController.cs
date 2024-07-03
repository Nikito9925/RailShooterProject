using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    public void Shoot()
    {
        _enemy.Shoot();
    }

    public void Death()
    {
        
    }

    public void DoDamage()
    {
        _enemy._player.GetComponent<Player>().DoDamage();
    }
}
