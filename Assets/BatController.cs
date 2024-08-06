using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : Enemy
{
    [SerializeField] private Transform _mesh;
    [SerializeField] private Transform _batHolder;
    //[SerializeField] private Transform _player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _batHolder.position;
        _mesh.LookAt(_player.position);
    }

    public override void Attack()
    {
        //_animator.SetTrigger("Attack");
        //Debug.Log("Enemy Attack");
        //_audioController.PlayEnemyAttackSound();
    }

    public override void Death()
    {
        /*
        _audioController.PlayEnemyDeathSound();

        if (_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }
        if (_nodeController != null && _nodeController._enemyList.Contains(this.gameObject)) //Si pertenece a un nodo, y esta en la lista
        {
            _nodeController._enemyList.Remove(this.gameObject);
        }

        _standBy = true;
        _animator.SetTrigger("Death");

        //Invoke("DestroyEnemy", 3f);
        */
    }

    public override void UpdateState()
    {
        //if (_standBy) _enemyState = EnemyState.Inactive;
        //else if (!_standBy && _agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
        //else if (!_standBy && _agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
    }
}
