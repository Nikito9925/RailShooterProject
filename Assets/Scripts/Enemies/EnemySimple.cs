using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : Enemy
{
    private void Start()
    {
        _life = 10;

        _agent.SetDestination(_player.position);

        BoxCollider[] colliderList = GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider collider in colliderList)
        {
            string colliderTag = collider.tag;
            string manteca = colliderTag.Substring(8);

            EnemyPart enumPart = (EnemyPart)Enum.Parse(typeof(EnemyPart), manteca);

            _bodyParts.Add(enumPart, collider);
        }
    }

    void Update()
    {
        _animator.SetBool("Walking", _isWalking);
        UpdateState();

        switch(_enemyState)
        {
            case EnemyState.Inactive:

                _agent.speed = 0f;
                _isWalking = false;
                break;
            case EnemyState.Walking:
                _agent.speed = _speed;
                _agent.SetDestination(_player.position);
                _isWalking = true;
                if (_railController._enemyList.Contains(this))
                {
                    _railController._enemyList.Remove(this);
                }
                break;
            case EnemyState.WaitForAttack:
                _agent.speed = 0;
                _agent.SetDestination(_player.position);
                _isWalking = false;
                if(!_railController._enemyList.Contains(this))
                {
                    _railController._enemyList.Add(this);
                }
                break;
        }

        if (_life <= 0)
        {
            //Debug.Log("LIFE == 0");
            Death();
        }
    }

    public override void Attack()
    {
        _animator.SetTrigger("Attack");
        Debug.Log("Enemy Attack");
    }

    public override void Death()
    {
        if(_nodeController == null) //Si no pertenece a un nodo
        {
            Destroy(this.gameObject);
        }
        else if(!(_nodeController == null) && _nodeController._enemyList.Contains(this.gameObject)) //Si pertenece a un nodo, y esta en la lista
        {
            _nodeController._enemyList.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

        if(_railController._enemyList.Contains(this))
        {
            _railController._enemyList.Remove(this);
        }

    }

    public override void UpdateState()
    {
        if (_standBy) _enemyState = EnemyState.Inactive;
        else if (!_standBy && _agent.remainingDistance >= _minDistance) _enemyState = EnemyState.Walking;
        else if (!_standBy && _agent.remainingDistance < _minDistance) _enemyState = EnemyState.WaitForAttack;
    }
}
